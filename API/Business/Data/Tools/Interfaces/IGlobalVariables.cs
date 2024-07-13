using Business.Management.Models;

namespace Business.Data.Tools.Interfaces
{
    public interface IGlobalVariables
    {
        Guid AppID { get; }
        ServiceID_MODEL AppID_Model { get; }
        bool DBState { get; set; }
        DateTime Deployed { get; }
    }
}
