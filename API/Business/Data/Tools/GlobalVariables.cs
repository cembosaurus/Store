using Business.Data.Tools.Interfaces;
using Business.Management.Models;
using Microsoft.Extensions.Configuration;
using System.Globalization;



namespace Business.Data
{
    public class GlobalVariables : IGlobalVariables
    {

        private readonly Guid _serviceId = Guid.NewGuid();
        // physical file-name of a project as it's stated in f.e: docker file or file system, for acurate identification:
        private readonly string _service_ProjectFileName = Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName) ?? "";
        private readonly string _serviceName;
        private readonly DateTime _serviceDeployed = DateTime.UtcNow;
        private bool _dbState = true;


        public GlobalVariables(IConfiguration conf)
        {
            _serviceName = conf.GetSection("Name").Value ?? "";
        }



		public Guid ServiceID
		{
			get { return _serviceId; }
		}

        public string ServiceName
        {
            get { return _serviceName; }
        }

        public string ServiceName_ProjectFileName 
        {
            get { return _service_ProjectFileName; }
        }

        public DateTime ServiceDeployed
        {
            get { return _serviceDeployed; }
        }

        public ServiceID_MODEL ServiceID_Model
        {
            get {
                return new ServiceID_MODEL
                {
                    Id = _serviceId,
                    Name = _serviceName,
                    Name_ProjectFileName = _service_ProjectFileName,
                    Deployed = _serviceDeployed.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)
                };
            }
        }


        public bool DBState
		{
			get { return _dbState; }
			set { _dbState = value; }
		}

	}
}
