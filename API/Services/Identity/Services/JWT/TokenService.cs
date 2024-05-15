using Business.Identity.Enums;
using Business.Libraries.ServiceResult.Interfaces;
using Identity.Services.JWT.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Services.Identity.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Identity.Services.JWT
{
    public class TokenService : ITokenService
    {

        private readonly SymmetricSecurityKey _key;
        private readonly UserManager<AppUser> _userManager;
        private readonly IServiceResultFactory _resultFact;


        public TokenService(IConfiguration config, UserManager<AppUser> userManager, IServiceResultFactory resultFact)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("Auth:JWTKey").Value));
            _userManager = userManager;
            _resultFact = resultFact;
        }




        public async Task<IServiceResult<string>> CreateTokenForUser(AppUser user)
        {
            if (user == null)
                return _resultFact.Result(string.Empty, false, $"User data for authentication were not provided !");

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, user.UserName)
            };

            var usersRoles = await _userManager.GetRolesAsync(user);

            claims.AddRange(usersRoles.Select(role => new Claim(ClaimTypes.Role, role)));            

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = creds,
                Expires = DateTime.Now.AddDays(7)
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenResult = tokenHandler.CreateToken(tokenDescriptor);

            var token = tokenHandler.WriteToken(tokenResult);

            if (string.IsNullOrWhiteSpace(token))
                return _resultFact.Result("", false, $"Token for user '{user.UserName}' was NOT generated !");

            return _resultFact.Result(token, true);
        }




        public async Task<IServiceResult<string>> CreateTokenForService()
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Name, "ServiceApp")
            };

            var serviceRoles = new List<string>
            {
                RoleType.ServiceApp.ToString()
            };

            claims.AddRange(serviceRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = creds,
                Expires = DateTime.Now.AddDays(7)
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenResult = tokenHandler.CreateToken(tokenDescriptor);

            var token = tokenHandler.WriteToken(tokenResult);

            if(string.IsNullOrWhiteSpace(token))
                return _resultFact.Result("", false, $"Token for 'Service App' was NOT generated !");

            return _resultFact.Result(token, true);
        }

    }
}
