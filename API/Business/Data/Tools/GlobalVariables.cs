using Business.Data.Tools.Interfaces;
using Business.Management.Models;



namespace Business.Data
{
    public class GlobalVariables : IGlobalVariables
    {

        private readonly Guid _appId = Guid.NewGuid();
        private readonly DateTime _deployed = DateTime.UtcNow;
        private bool _dbState = true;




		public Guid AppID
		{
			get { return _appId; }
		}

        public DateTime Deployed
        {
            get { return _deployed; }
        }

        public ServiceID_MODEL AppID_Model
        {
            get {
                return new ServiceID_MODEL
                {
                    AppId = _appId,
                    Deployed = _deployed
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
