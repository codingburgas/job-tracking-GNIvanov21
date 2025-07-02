using JobTracking.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.AspNetCore.Authentication.JwtBearer; 
// using Microsoft.IdentityModel.Tokens; 
// using System.Text; 
// using JobTracking.Domain.Constants; 

namespace JobTracking.API
{
    public static class ServiceConfiguratorExtensions
    {
        public static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddDbContext<ApplicationDbContext>(options =>
                //options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }
        
        public static void AddIdentity(this WebApplicationBuilder builder)
        {
            // Identity
        }
 
        public static void AddServices(this WebApplicationBuilder builder)
        {
            //  Application
        }
 
        public static void AddCors(this WebApplicationBuilder builder)
        {
            // CORS връзка с Angular
        }
    }
}