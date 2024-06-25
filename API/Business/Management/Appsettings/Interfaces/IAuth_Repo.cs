using Business.Management.Appsettings.Models;

namespace Business.Management.Appsettings.Interfaces
{
    public interface IAuth_REPO
    {
        Auth_AS_MODEL Get { get; }

        void Initi8alize(Auth_AS_MODEL auth);
    }
}
