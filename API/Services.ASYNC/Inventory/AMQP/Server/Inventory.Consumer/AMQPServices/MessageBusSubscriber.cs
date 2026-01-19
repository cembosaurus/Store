using Business.Libraries.ServiceResult.Interfaces;
using Inventory.Consumer.AMQPServices.Interfaces;
using Inventory.Consumer.Services.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;



namespace Inventory.Consumer.AMQPServices
{
    public class MessageBusSubscriber : BackgroundService, IMessageBusSubscriber
    {
        private readonly bool _isProdEnv;
        private readonly IItemService _itemService;
        private readonly IServiceResultFactory _resultFact;
        private readonly IConfiguration _config;
        private readonly ConnectionFactory _connFact;

        private IConnection _connection;
        private IModel _channel;
        private EventingBasicConsumer _consumer;
        private bool _connectionStatus;

        private string _defaultExchangeName = "";
        private string _requestQueueName;


        public MessageBusSubscriber(IConfiguration config, IItemService itemService, IServiceResultFactory resultFact, IWebHostEnvironment env)
        {
            _isProdEnv = env.IsProduction();
            _itemService = itemService;
            _resultFact = resultFact;
            _config = config;

            _requestQueueName = _config.GetSection($"Config:Global:RabbitMQ:Server:{(_isProdEnv ? "Prod" : "Dev")}:ItemRequestQueueName").Value;
            _connFact = new ConnectionFactory()
            {
                HostName = _config.GetSection($"Config:Global:RabbitMQ:Server:{(_isProdEnv ? "Prod" : "Dev")}:Host").Value,
                Port = int.Parse(_config.GetSection($"Config:Global:RabbitMQ:Server:{(_isProdEnv ? "Prod" : "Dev")}:Port").Value)
            };

            CreateConnection();
        }




        protected override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            return Initialize(cancellationToken);
        }



        public async Task<IServiceResult<bool>> Connect(CancellationToken cancellationToken)
        {
            if (!_connectionStatus)
                CreateConnection();

            if (_connectionStatus)
            { 
                var result = Initialize(cancellationToken).IsCompletedSuccessfully;
            
                return _resultFact.Result(result, true, "Connected ...");
            }

            return _resultFact.Result(false, false, "RabbitMQ server is NOT connected !");
        }


        public bool IsConnectedToRabbitMQServer
        {
            get { return _connectionStatus; }
        }


        private Task Initialize(CancellationToken cancellationToken)
        {
            if (_connectionStatus)
            {
                cancellationToken.ThrowIfCancellationRequested();

                _consumer.Received += (sender, args) =>
                {

                    Console.WriteLine($"--> RECEIVED. Correlation Id: {args.BasicProperties.CorrelationId}");


                    var reqMethod = Encoding.Default.GetString((byte[])args.BasicProperties.Headers["RequestMethod"]);
                    var reqParam = Encoding.Default.GetString((byte[])args.BasicProperties.Headers["Params"]);


                    var result = _itemService.Request(reqMethod, reqParam).Result;


                    if (!result.Status || result.Data == null)
                        result = _resultFact.Result<object>(null, false, result.Message);

                    var jsonData = JsonSerializer.Serialize(result);
                    var body = Encoding.UTF8.GetBytes(jsonData);

                    Console.WriteLine($"--> Replying to client: {args.BasicProperties.CorrelationId}");

                    _channel.BasicPublish(_defaultExchangeName, args.BasicProperties.ReplyTo, null, body);
                };


                _channel.BasicConsume(queue: _requestQueueName, autoAck: true, consumer: _consumer);
            }

            return Task.CompletedTask;
        }



        private void CreateConnection()
        {
            try
            {
                _connection = _connFact.CreateConnection();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> FAILED to create connection to RabbitMQ server: {ex.Message}");

                _connectionStatus = false;

                return;
            }

            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: _requestQueueName, exclusive: false);
            _consumer = new EventingBasicConsumer(_channel);

            Console.WriteLine("--> CONNECTED ... Listenning on the Message Bus...");

            _connectionStatus =  true;
        }



        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> Connection Shutdown!");

            Dispose();
        }


        public override void Dispose()
        {
            if (!_channel.IsClosed)
            {
                _channel.Close();
                _connection.Close();
            }

            base.Dispose();
        }





    }
}
