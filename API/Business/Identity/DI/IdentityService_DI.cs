using Business.Identity.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;



namespace Business.Identity.DI
{
    public static class IdentityService_DI
    {
        public static IServiceCollection AddIdentityServiceIntegration(this IServiceCollection services, IConfiguration configuration)
        {
            // Authorization:

            services.AddAuthorization(opt => {
                opt.AddPolicy(PolicyType.Administration.ToString(),
                    p => p.RequireRole(
                    RoleType.Admin.ToString()
                ));
                opt.AddPolicy(PolicyType.Management.ToString(),
                    p => p.RequireRole(
                    RoleType.Manager.ToString(),
                    RoleType.Accountant.ToString(),
                    RoleType.Seller.ToString()
                ));
                opt.AddPolicy(PolicyType.Support.ToString(),
                    p => p.RequireRole(
                    RoleType.ProductExpert.ToString()
                ));
                opt.AddPolicy(PolicyType.Shopping.ToString(),
                    p => p.RequireRole(
                    RoleType.Customer.ToString()
                ));
                opt.AddPolicy(PolicyType.Everyone.ToString(),
                p => p.RequireRole(
                    RoleType.Admin.ToString(),
                    RoleType.Manager.ToString(),
                    RoleType.Accountant.ToString(),
                    RoleType.Seller.ToString(),
                    RoleType.ProductExpert.ToString(),
                    RoleType.Customer.ToString(),
                    RoleType.ServiceApp.ToString()
                ));
            });



            // Authentication:

            var secret = configuration["Config:Global:Auth:JWTKey"];

            if (string.IsNullOrWhiteSpace(secret))
                throw new InvalidOperationException("Missing config: Config:Global:Auth:JWTKey");

            var secretByteArray = Encoding.UTF8.GetBytes(secret);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(secretByteArray),
                        ValidateIssuer = false,                 // BE - API
                        ValidateAudience = false,               // FE - angular
                        ClockSkew = TimeSpan.FromSeconds(30)    // A token can still be accepted up to 30 sec. after 'exp' and can be accepted up to 30 sec. before 'nbf'
                    };
                });



            return services;
        }
    }
}
