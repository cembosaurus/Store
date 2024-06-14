namespace Business.Management.Appsettings.Interfaces
{
    public interface IConfig_Global_REPO
    {
        IAuth_REPO Auth { get; set; }
        IRabbitMQ_REPO RabbitMQ { get; set; }
        IRemoteServices_REPO RemoteServices { get; set; }
    }
}
