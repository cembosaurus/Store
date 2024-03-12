using API_Gateway.Services.Management.Interfaces;
using Business.appsettings;
using Business.Libraries.ServiceResult.Interfaces;



namespace API_Gateway.Services.Management
{
    public class RemoteServices : IRemoteServices
    {


        private readonly IConfiguration _config;
        private readonly IServiceResultFactory _resultFact;


        public RemoteServices(IConfiguration config, IServiceResultFactory resutlFact)
        {
            _config = config;
            _resultFact = resutlFact;
        }




        public IServiceResult<IEnumerable<IConfigurationSection>> GetAllRemoteServicesInfo()
        {
            var result = _config.GetSection("RemoteServices").GetChildren();

            var kokot = _config.GetSection("Test").Get<List<RemoteService>>();

            return _resultFact.Result<IEnumerable<IConfigurationSection>>(result, true);
        }
    }
}
