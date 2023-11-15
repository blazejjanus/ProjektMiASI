using DB;
using DB.DBO;
using Microsoft.AspNetCore.Http;
using Services.Interfaces;
using Shared.Configuration;
using Shared.Enums;
using System.Net;

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

        public void Log(string message, EventTypes type) {
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
            Display(text, EventTypes.ERROR);
            if (Config.UseLogFile) {
                LogFile(EventTypes.ERROR.ToString() + ": " + text);
            }
            LogDatabase(exc, message);
        }

        public void Log(int status, string senderClass, string senderMethod, string? message = null) {
            var text = senderClass + ":" + senderMethod + " - " + status + " " + ((HttpStatusCode)status).ToString();
            if (!string.IsNullOrEmpty(message)) { text += ": " + message; }
            Log(text, GetEventTypeByStatusCode(status));
        }


        private void LogFile(string message) {
            if (!message.EndsWith(System.Environment.NewLine)) {
                message += System.Environment.NewLine;
            }
            File.AppendAllTextAsync(LogFileName, DateTime.Now.ToString("yyyy-MM-dd HH:mm") + ": " + message);
        }

        private void LogDatabase(string message, EventTypes type) {
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

        private void Display(string message, EventTypes type) {
            switch (type) {
                case EventTypes.ERROR:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case EventTypes.WARNING:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                case EventTypes.SUCCESS:
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

        private EventTypes GetEventTypeByStatusCode(int statusCode) {
            if (statusCode >= 400 && statusCode < 500) {
                return EventTypes.ERROR;
            } else if (statusCode >= 300 && statusCode < 400) {
                return EventTypes.WARNING;
            } else if (statusCode >= 200 && statusCode < 300) {
                return EventTypes.SUCCESS;
            } else {
                return EventTypes.INFO;
            }
        }
    }
}
