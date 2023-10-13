using Microsoft.OpenApi.Models;
using Services.Interfaces;
using Services.Services;
using Shared.Configuration;
using System.Net;

namespace API {
    /// <summary>
    /// Main ASP.NET App class
    /// </summary>
    public class Program {
        /// <summary>
        /// Main method
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args) {
            try {
                var builder = WebApplication.CreateBuilder(args);
                //Setting environment
                var environment = Environment.GetInstance();
                var config = Config.ReadConfig();
                if (config.IsDevelopmentEnvironment) {
                    Console.WriteLine("Set up environment pathes:\n" + environment.ToString(true));
                    Console.WriteLine("config read:\n" + config.ToJson() + "\n");
                }
                // Add services to the container.
                builder.Services.AddSingleton(config);
                //Services goes here
                builder.Services.AddSingleton<ILoggingService, LoggingService>();
                builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
                builder.Services.AddScoped<IUserService, UserService>();
                builder.Services.AddScoped<ILoginService, LoginService>();
                builder.Services.AddScoped<IOrderService, OrderService>();
                builder.Services.AddScoped<ICarManagementService, CarManagementService>();
                builder.Services.AddScoped<IImageService, ImageService>();
                builder.Services.AddControllers();
                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen(x => {
                    x.SwaggerDoc("v1", new OpenApiInfo { Title = "MiASI", Version = "v1" });
                });
                builder.Services.AddSwaggerGen();
                builder.Services.AddCors(p => p.AddPolicy("corsapp", builder => {
                    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
                }));
                builder.Services.AddHttpsRedirection(options => {
                    options.RedirectStatusCode = (int)HttpStatusCode.TemporaryRedirect;
                    options.HttpsPort = 5000;
                });
                var app = builder.Build();
                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment() || config.IsDevelopmentEnvironment) {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                    Console.WriteLine($"Swagger is enabled.\n\tAvaliable on: /swagger/index.html");
                }
                app.UseHttpsRedirection();
                app.UseRouting();
                app.UseCors("corsapp");
                app.UseAuthorization();
                app.MapControllers();
                app.Run("https://localhost:5000");
            } catch(Exception exc) {
                Console.WriteLine("Exception occured during startup: " + exc.ToString());
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                return;
            }
        }
    }
}