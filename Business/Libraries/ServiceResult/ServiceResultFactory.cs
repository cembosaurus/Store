using Business.Libraries.ServiceResult.Interfaces;

namespace Business.Libraries.ServiceResult
{
    public class ServiceResultFactory : IServiceResultFactory
    {

        public IServiceResult Result(bool status = false, string message = "") 
        { 
            return new ServiceResult(status, message); 
        }

        public IServiceResult<T> Result<T>(T data, bool status = false, string message = "") 
        { 
            return new ServiceResult<T>(data, status, message); 
        }


    }
}
