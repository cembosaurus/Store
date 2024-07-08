using Business.Management.Models;

namespace Business.Data.Tools.Interfaces
{
    public interface IGlobalVariables
    {
        bool DBState { get; set; }
        ServiceID_MODEL ServiceID_Model { get; }
        DateTime ServiceDeployed { get; }
        Guid ServiceID { get; }
        string ServiceName { get; }
    }
}
