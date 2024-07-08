using Business.Management.Models;

namespace Business.Data.Tools.Interfaces
{
    public interface IGlobalVariables
    {
        bool DBState { get; set; }
        DateTime ServiceDeployed { get; }
        Guid ServiceID { get; }
        ServiceID_MODEL ServiceID_Model { get; }
        string ServiceName { get; }
        string ServiceName_ProjectFileName { get; }
    }
}
