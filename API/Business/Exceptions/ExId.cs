using Business.Exceptions.Interfaces;
using System.ComponentModel;

namespace Business.Exceptions
{
    public class ExId : IExId
    {
        public bool Http_503(Exception error)
        {
            var w32ex = error as Win32Exception;

            if (w32ex == null)
                w32ex = error.InnerException as Win32Exception;

            if (w32ex != null)
                return w32ex.ErrorCode == 10061;

            return false;
        }
    }
}
