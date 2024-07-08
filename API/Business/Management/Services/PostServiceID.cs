using Business.Data.Tools.Interfaces;
using Business.Management.Http.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Business.Management.Services
{
    public static class PostServiceID
    {
        public static void Send(IApplicationBuilder app)
        {  


            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var service = serviceScope.ServiceProvider.GetService<HttpManagementService>();
                var gv = serviceScope.ServiceProvider.GetService<IGlobalVariables>();



                var result = service.PostRemoteServiceID(gv.ServiceID_Model);


            }


        }


    }
}
