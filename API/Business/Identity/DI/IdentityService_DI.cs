using Business.Identity.Enums;
using Microsoft.Extensions.DependencyInjection;



namespace Business.Identity.DI
{
    public static class IdentityService_DI
    {
        public static IServiceCollection AddIdentityServiceIntegration(this IServiceCollection services)
        {
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

            return services;
        }
    }
}
