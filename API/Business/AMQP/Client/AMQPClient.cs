using Business.AMQP.AMQPClient.Interfaces;
using Business.Libraries.ServiceResult.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Diagnostics;
using System.Text;

namespace Business.AMQP.Client
{
    public class AMQPClient : IAMQClient
    {

        private readonly int _resultAwaitSeconds;
        private readonly int _resultPollingIntervalSeconds;
        private readonly string _host;
        private readonly int _port;

        private ConnectionFactory _connFact;
        private bool _connectionStatus;
        private IBasicProperties _properties;
        private IConnection _connection;
        private IModel _channel;
        private EventingBasicConsumer _consumer;
        private QueueDeclareOk _replyQueue;
        private string _defaultExchangeName = "";
        private string _requestQueueName;
        private string _replyQueueName = "";

        private Stopwatch _stopWatch = new Stopwatch();
        private string result;
        private bool _received = false;
        private static IHttpContextAccessor _accessor;
        private readonly IServiceResultFactory _resultFact;

        private static string _requestMethod => _accessor == null ? "" : _accessor.HttpContext?.Request.Headers["RequestMethod"].ToString() ?? "";


        public AMQPClient(IConfiguration config, IHttpContextAccessor accessor, IServiceResultFactory resultFact)
        {
            _accessor = accessor;
            _resultFact = resultFact;

            _host = config.GetSection("RabbitMQ:Server:Host").Value;
            int.TryParse(config.GetSection("RabbitMQ:Server:Port").Value, out _port);
            int.TryParse(config.GetSection("RabbitMQ:Server:ResultAwaitSeconds").Value, out _resultAwaitSeconds);
            int.TryParse(config.GetSection("RabbitMQ:Server:ResultPollingIntervalSeconds").Value, out _resultPollingIntervalSeconds);
            _connFact = new ConnectionFactory()
            {
                HostName = _host,
                Port = _port
                //DispatchConsumersAsync = true 
            };

            _connectionStatus = CreateConnection();
        }





        public string RequestQueueName
        {
            set { _requestQueueName = value; }
        }



        private bool Connect()
        {
            if (!_connectionStatus)
                _connectionStatus = CreateConnection();

            if (_connectionStatus)
            {
                Subscribe();

                return true;
            }

            return false;
        }



        public async Task<IServiceResult<string>> Request(string reqParam)
        {
            if (!Connect())
                return _resultFact.Result("", false, "Unable to connect to RabbitMQ server !");

            PublishRequest(reqParam);

            await PollResult();

            RabbitMQ_ConnectionShutdown();

            if (string.IsNullOrWhiteSpace(result))
            {
                var message = "FAILED to fetch data from subscriber. Make sure subscriber is connected to RabbitMQ server !";
                Console.WriteLine($"--> {message}");

                return _resultFact.Result("", false, message);
            }

            return _resultFact.Result(result, true);
        }



        private void Subscribe()
        {
            if (_connectionStatus)
            {
                _consumer.Received += (sender, args) =>
                {
                    var body = args.Body.ToArray();
                    result = Encoding.UTF8.GetString(body);
                    _received = true;
                    Console.WriteLine($"--> RECEIVED message from subscriber. Id: {args.BasicProperties.CorrelationId}");
                };

                _channel.BasicConsume(queue: _replyQueue.QueueName, autoAck: true, consumer: _consumer);
            }
        }



        private bool CreateConnection()
        {
            if (string.IsNullOrWhiteSpace(_requestQueueName))
            {
                Console.WriteLine("--> AMQP Request Query name was NOT defined !");

                return false;
            }


            try
            {
                _connection = _connFact.CreateConnection();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> FAILED to create connection to RabbitMQ server: {ex.Message}");
            
                return false;
            }
            
            _channel = _connection.CreateModel();
            _consumer = new EventingBasicConsumer(_channel);
            _replyQueue = _channel.QueueDeclare(queue: _replyQueueName, exclusive: true);
            _channel.QueueDeclare(queue: _requestQueueName, exclusive: false);
            
            Console.WriteLine($"--> CREATED connection to RabbitMQ server.");
            
            return true;
        }



        private string PublishRequest(string reqParam)
        {
            _properties = _channel.CreateBasicProperties();
            _properties.CorrelationId = Guid.NewGuid().ToString();
            _properties.ReplyTo = _replyQueue.QueueName;

            Console.WriteLine($"--> Sending REQUEST: {_properties.CorrelationId} ...");

            _properties.Headers = new Dictionary<string, object>();
            _properties.Headers.Add(new KeyValuePair<string, object>("RequestMethod", _requestMethod));
            _properties.Headers.Add(new KeyValuePair<string, object>("Params", reqParam));

            _channel.BasicPublish(_defaultExchangeName, _requestQueueName, _properties, null);

            return _properties.CorrelationId;
        }



        private async Task PollResult()
        {
            _stopWatch.Start();

            while (_stopWatch.Elapsed < TimeSpan.FromSeconds(_resultAwaitSeconds) && !_received)
            {
                Console.Write($"-");
                await Task.Delay(_resultPollingIntervalSeconds * 1000);
            }

            _stopWatch.Stop();
        }



        private void RabbitMQ_ConnectionShutdown()
        {
            Console.WriteLine("--> AMQP Connection Shutdown!");

            if (!_channel.IsClosed)
            {
                _channel.Close();
                _connection.Close();
            }
        }


    }
}
