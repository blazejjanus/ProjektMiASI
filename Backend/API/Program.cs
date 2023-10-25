using Services.Interfaces;
using Services.Services;
using Shared;

namespace API {
    public class Program {
        public static void Main(string[] args) {
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

            app.Run();
        }
    }
}