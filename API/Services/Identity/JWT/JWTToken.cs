using Services.Identity.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Services.Identity.JWT
{
    public class JWTToken
    {
        private readonly AppUser _user;
        private readonly string _secret;

        public JWTToken(AppUser user, string secret)
        {
            _user = user;
            _secret = secret;
        }

        public string Build()
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, _user.Id.ToString()),
                new Claim(ClaimTypes.Name, _user.UserName)
            };

            var mySecret = Encoding.UTF8.GetBytes(_secret);
            var key = new SymmetricSecurityKey(mySecret);
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var JWT_Token = tokenHandler.CreateToken(tokenDescriptor);

            //... http://jwt.io ...
            return tokenHandler.WriteToken(JWT_Token);

        }
    }
}
