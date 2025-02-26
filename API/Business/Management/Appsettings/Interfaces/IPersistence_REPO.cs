using Business.Management.Appsettings.Models;



namespace Business.Management.Appsettings.Interfaces
{
    public interface IPersistence_REPO
    {
        Persistence_AS_MODEL Data { get; }
        int DefaultPageNumber { get; }
        int DefaultPageSize { get; }

        void Initialize(Persistence_AS_MODEL persistence);
    }
}
