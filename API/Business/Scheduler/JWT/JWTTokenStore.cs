using Business.Scheduler.JWT.Interfaces;
using System.IdentityModel.Tokens.Jwt;



namespace Business.Scheduler.JWT
{

	// singleton

	public class JWTTokenStore : IJWTTokenStore
    {

        private string _token;


		public string Token
		{
			get { return _token; }
			set { _token = value; }
		}

		public bool IsExipred
		{
			get { return IsExpired(_token); }
		}



		private bool IsExpired(string token)
		{
			if (string.IsNullOrWhiteSpace(token))
				return true;

            var jwthandler = new JwtSecurityTokenHandler();

            var jwttoken = jwthandler.ReadToken(token);

			if (jwttoken == null)
				return true;

            var expDate = jwttoken.ValidTo;

			if (expDate < DateTime.UtcNow.AddMinutes(1))
				return true;

			return false;        
		}

	}
}
