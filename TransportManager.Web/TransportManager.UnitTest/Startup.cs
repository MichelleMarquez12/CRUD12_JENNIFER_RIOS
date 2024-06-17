using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TransportManager.ApplicationServices.Transport;
using TransportManager.Core.Transports;
using TransportManager.DataAccess.Repositories;
using Microsoft.AspNetCore.Hosting;
using TransportManager.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace TransportManager.UnitTest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddHttpClient();

            //services.AddTransient<Checker>();

            services.AddDbContext<TransportManagerContext>(options => options.UseInMemoryDatabase("TransportTest"));

            services.AddScoped<IJourneyAppService, JourneyAppService>();
            services.AddScoped<IPassengerAppService, PassengerAppService>();
            services.AddScoped<ITicketAppService, TicketAppService>();

            services.AddScoped<IRepository<int, JourneyDto>, Repository<int, JourneyDto>>();
            services.AddScoped<IRepository<int, PassengerDto>, Repository<int, PassengerDto>>();
            services.AddScoped<IRepository<int, TicketDto>, Repository<int, TicketDto>>();

            services.AddAutoMapper(typeof(TransportManager.ApplicationServices.MapperProfile));

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}