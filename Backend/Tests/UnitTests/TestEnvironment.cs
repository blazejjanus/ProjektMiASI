using Services.Interfaces;
using Services.Services;
using Shared.Configuration;

namespace UnitTests {
    internal class TestEnvironment {
        #region Singleton
        private static TestEnvironment? _instance;
        public static TestEnvironment GetInstance() {
            if (_instance == null) {
                _instance = new TestEnvironment();
            }
            return _instance;
        }
        #endregion

        private TestEnvironment() { 
            Config = Config.ReadConfig();
            var environment = Shared.Environment.GetInstance();
            TestLogPath = Path.Combine(environment.LogPath, "UnitTests.log");
            AuthenticationService = new AuthenticationService(Config);
            LoggingService = new LoggingService(Config);
            UserService = new UserService(Config);
            LoginService = new LoginService(AuthenticationService, Config);
            CarManagementService = new CarManagementService(Config);
            ImageService = new ImageService(Config);
            OrderService = new OrderService(Config);
        }

        public Config Config { get; }
        public IAuthenticationService AuthenticationService { get; }
        public ILoggingService LoggingService { get; }
        public IUserService UserService { get; }
        public ILoginService LoginService { get; }
        public ICarManagementService CarManagementService { get; }
        public IImageService ImageService { get; }
        public IOrderService OrderService { get; }
        public string TestLogPath { get; }
    }
}
