using Services.Interfaces;
using Services.Services;
using Shared.Configuration;

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
                builder.Services.AddSwaggerGen();
                var app = builder.Build();
                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment()) {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }
                app.UseHttpsRedirection();
                app.UseAuthorization();
                app.MapControllers();
                app.UseCors(app => app.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
                app.Run();
            } catch(Exception exc) {
                Console.WriteLine("Exception occured during startup: " + exc.ToString());
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                return;
            }
        }
    }
}