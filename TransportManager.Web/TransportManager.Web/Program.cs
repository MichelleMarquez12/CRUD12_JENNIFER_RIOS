using TransportManager.ApplicationServices.Transport;
using TransportManager.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;

namespace Program
{
    public class Program
    {
        static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var environment = builder.Environment.EnvironmentName = "Development";

            builder.Configuration
                .SetBasePath(builder.Environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            var connectionString = builder.Configuration.GetConnectionString("Default");

            builder.Services.AddDbContext<TransportManagerContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), mySqlOptions =>
                {
                    mySqlOptions.EnableRetryOnFailure();
                }));

            builder.Services.AddRazorPages();
            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
            builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

            // Register services and repositories
            builder.Services.AddScoped<IJourneyAppService, JourneyAppService>();
            builder.Services.AddScoped<IPassengerAppService, PassengerAppService>();
            builder.Services.AddScoped<ITicketAppService, TicketAppService>();

            // Add Swagger services
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Transport Manager API",
                    Description = "API documentation for Transport Manager"
                });
            });

            var app = builder.Build();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Transport Manager API V1");
                c.RoutePrefix = string.Empty; // Set Swagger UI at app's root
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "error",
                    pattern: "/Error",
                    defaults: new { controller = "Error", action = "Error" });
            });

            app.MapRazorPages();
            app.UseStaticFiles();
            app.Run();
        }
    }
}
