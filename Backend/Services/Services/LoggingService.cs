using DB;
using DB.DBO;
using Services.Interfaces;
using Shared.Configuration;
using Shared.Enums;

namespace Services.Services {
    public class LoggingService : ILoggingService {
        private Config Config { get; }
        private Environment Environment { get; }
        private string LogFileName { get; set; } = string.Empty;

        public LoggingService(Config config) {
            Config = config;
            Environment = Environment.GetInstance();
            if (Config.UseLogFile) {
                LogFileName = GenLogFileName();
            } else {
                LogFileName = string.Empty;
            }
        }

        public void Log(string message, EventType type) {
            Display(message, type);
            if (Config.UseLogFile) {
                LogFile(type.ToString() + ": " + message);
            }
            LogDatabase(message, type);
        }

        public void Log(Exception exc, string? message = null) {
            var text = string.Empty;
            if (message != null) {
                text += message + ": ";
            }
            text += exc.ToString();
            Display(text, EventType.ERROR);
            if (Config.UseLogFile) {
                LogFile(EventType.ERROR.ToString() + ": " + text);
            }
            LogDatabase(exc, message);
        }


        private void LogFile(string message) {
            if (!message.EndsWith(System.Environment.NewLine)) {
                message += System.Environment.NewLine;
            }
            File.AppendAllTextAsync(LogFileName, DateTime.Now.ToString("yyyy-MM-dd HH:mm") + ": " + message);
        }

        private void LogDatabase(string message, EventType type) {
            var dbo = new EventDBO(message, type);
            using (var ctx = new DataContext(Config)) {
                ctx.Events.Add(dbo);
                ctx.SaveChanges();
            }
        }

        private void LogDatabase(Exception exc, string? message) {
            var dbo = new EventDBO(exc, message);
            using (var ctx = new DataContext(Config)) {
                ctx.Events.Add(dbo);
                ctx.SaveChanges();
            }
        }

        private void Display(string message, EventType type) {
            switch (type) {
                case EventType.ERROR:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case EventType.WARNING:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                case EventType.SUCCESS:
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    break;
            }
            Console.WriteLine(type.ToString() + ": " + message);
            Console.ResetColor();
        }

        private string GenLogFileName() {
            string file = DateTime.Now.ToString("yyyy_MM_dd_HH_mm");
            if (File.Exists(Path.Combine(Environment.LogPath, file + ".log"))) {
                for (int i = 0; i < int.MaxValue; i++) {
                    string temp = file + "_" + i;
                    if (!File.Exists(Path.Combine(Environment.LogPath, temp + ".log"))) {
                        file = temp; break;
                    }
                }
            }
            return Path.Combine(Environment.LogPath, file + ".log");
        }
    }
}
