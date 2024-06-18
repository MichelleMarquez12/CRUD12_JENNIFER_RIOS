using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;
using TransportManager.ApplicationServices.Transport;
using TransportManager.Core.Transports;
using TransportManager.DataAccess;
using TransportManager.DataAccess.Repositories;
using TransportManager.Web;
using TransportManager.Web.Auth;
using TransportManager.Web.Controllers;
using TransportManager.Web.Data;
using TransportManager.Web.Shared.Config;

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

            var connectionStrings = builder.Configuration.GetConnectionString("Jwt");

            builder.Services.AddDbContext<JwtDbContext>(options => options.UseMySql(connectionStrings, ServerVersion.AutoDetect(connectionStrings)));

            builder.Services.AddIdentity<IdentityUser, IdentityRole>(
            opts =>
            {
                opts.Password.RequireDigit = true;
                opts.Password.RequireLowercase = true;
                opts.Password.RequireUppercase = true;
                opts.Password.RequireNonAlphanumeric = true;
                opts.Password.RequiredLength = 7;
                opts.Password.RequiredUniqueChars = 4;
            }).AddEntityFrameworkStores<JwtDbContext>().AddDefaultTokenProviders();



            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Transport Manager API", Version = "v1" });
            });

            builder.Services.AddHttpClient();
            builder.Services.AddScoped<Checker>();
            // Register services and repositories
            builder.Services.AddScoped<IJourneyAppService, JourneyAppService>();
            builder.Services.AddScoped<IPassengerAppService, PassengerAppService>();
            builder.Services.AddScoped<ITicketAppService, TicketAppService>();
            

            builder.Services.AddScoped<IRepository<int, JourneyDto>, Repository<int, JourneyDto>>();
            builder.Services.AddScoped<IRepository<int, PassengerDto>, Repository<int, PassengerDto>>();
            builder.Services.AddScoped<IRepository<int, TicketDto>, Repository<int, TicketDto>>();

            builder.Services.AddAutoMapper(typeof(TransportManager.ApplicationServices.MapperProfile));
            builder.Services.AddControllers();

            builder.Services.Configure<JwtTokenValidationSettings>(builder.Configuration.GetSection("JwTokenValidationSettings"));

            builder.Services.AddTransient<IJwtIssuerOptions, JwtIssuerFactory>();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.MySQL(connectionString, tableName: "logging")
                .CreateLogger();

            builder.Services.AddSwaggerGen(option =>
            {
                option.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n  Enter your token in the text input below. \r\n",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = JwtBearerDefaults.AuthenticationScheme
                }
                );

                option.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            }
                        },
                        new string[]{}
                    }
                });
            });

            var tokenValidationSettings = builder.Services.BuildServiceProvider().GetService<IOptions<JwtTokenValidationSettings>>().Value;

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = tokenValidationSettings.ValidIssuer,
                    ValidAudience = tokenValidationSettings.ValidAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenValidationSettings.SecretKey)),
                    ClockSkew = TimeSpan.Zero
                };
            });

            var app = builder.Build();

            //app.Urls.Add("http://*:5024");

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
                    c.RoutePrefix = string.Empty; // Para servir Swagger UI en la raíz
                });
            }

            app.Use(async (context, next) =>
            {
                try
                {
                    await next.Invoke();
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Unhandled exception");
                    throw;
                }
            });

            app.InitDb();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.Run();
        }
    }
}