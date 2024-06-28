using Business.Management.Appsettings.Models;

namespace Business.Management.Appsettings.Interfaces
{
    public interface IAuth_REPO
    {
        string Apikey { get; }
        Auth_AS_MODEL Data { get; }
        string JWTKey { get; }

        void Initialize(Auth_AS_MODEL auth);
    }
}
