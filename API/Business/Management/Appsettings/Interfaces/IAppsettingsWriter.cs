using Business.Libraries.ServiceResult.Interfaces;

namespace Business.Management.Appsettings.Interfaces
{
    public interface IAppsettingsWriter
    {
        IServiceResult<string> AddOrUpdateAppSetting<T>(string sectionPathKey, T value);
    }
}
