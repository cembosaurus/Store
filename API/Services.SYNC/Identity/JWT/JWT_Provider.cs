using Business.Identity.Enums;
using Business.Libraries.ServiceResult.Interfaces;
using Identity.JWT.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Services.Identity.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;



namespace Identity.JWT
{
    public class JWT_Provider : IJWT_Provider
    {

        private readonly SymmetricSecurityKey _key;
        private readonly UserManager<AppUser> _userManager;
        private readonly IServiceResultFactory _resultFact;


        public JWT_Provider(IConfiguration config, UserManager<AppUser> userManager, IServiceResultFactory resultFact)
        {
            var value = config.GetSection("Config:Global:Auth:JWTKey").Value ?? "";
            var secretKey = Encoding.UTF8.GetBytes(value);
            _key = new SymmetricSecurityKey(secretKey);
            _userManager = userManager;
            _resultFact = resultFact;
        }




        public async Task<IServiceResult<string>> CreateToken_ForUser(AppUser user)
        {
            if (user == null)
                return _resultFact.Result(string.Empty, false, $"App User model for authentication was not provided !");

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, user.UserName ?? "unknown")
            };

            var usersRoles = await _userManager.GetRolesAsync(user);


            return GenerateToken(claims, usersRoles, $"Token for user '{user.UserName}' was NOT generated !");
        }


        // NOT USED. ApiKey (instead of JWT) is used to directly authenticate api service:
        public IServiceResult<string> CreateToken_ForService()
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Name, "ServiceApp")
            };

            var serviceRoles = new List<string>
            {
                RoleType.ServiceApp.ToString()
            };


            return GenerateToken(claims, serviceRoles, $"Token for API Service was NOT generated !");
        }



        private IServiceResult<string> GenerateToken(List<Claim> claims, IList<string> roles, string failMessage)
        {
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = creds,
                Expires = DateTime.Now.AddDays(7)
            };

            var handler = new JwtSecurityTokenHandler();

            var tokenResult = handler.CreateToken(tokenDescriptor);

            var token = handler.WriteToken(tokenResult);

            if (string.IsNullOrWhiteSpace(token))
                return _resultFact.Result("", false, failMessage);

            return _resultFact.Result(token, true);
        }

    }
}