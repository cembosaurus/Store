using Business.Management.Appsettings.Models;

namespace Business.Management.Appsettings.Interfaces
{
    public interface IConfig_Global_REPO
    {
        IAuth_REPO Auth { get; }
        Config_Global_AS_MODEL GlobalConfig { get; }
        Persistence_REPO Persistence { get; }
        IRabbitMQ_REPO RabbitMQ { get; }
        IRemoteServices_REPO RemoteServices { get; }

        void Initialize(Config_Global_AS_MODEL globalConfig);
    }
}
