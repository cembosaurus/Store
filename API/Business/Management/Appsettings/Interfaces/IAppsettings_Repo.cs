namespace Business.Management.Appsettings.Interfaces
{
    public interface IAppsettings_Repo
    {
        Auth_Repo Auth { get; set; }
        RabbitMQ_Repo RabbitMQ { get; set; }
        RemoteServices_Repo RemoteServices { get; set; }
    }
}
