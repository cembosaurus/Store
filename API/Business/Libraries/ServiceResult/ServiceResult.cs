using Business.Libraries.ServiceResult.Interfaces;

namespace Business.Libraries.ServiceResult
{
    public class ServiceResult<T> : ServiceResult, IServiceResult<T>
    {

        private protected T? _data;

        public ServiceResult(T? data = default, bool status = false, string message = "")
            : base(status, message)
        {
            _data = data;
        }


        public T? Data => _data;

    }


    public class ServiceResult : IServiceResult
    {

        private protected bool _status;
        private protected string _message;


        public ServiceResult(bool status = false, string message = "")
        {
            _status = status;
            _message = message ?? "";
        }



        public bool Status => _status;

        public string Message => _message;

    }
}
