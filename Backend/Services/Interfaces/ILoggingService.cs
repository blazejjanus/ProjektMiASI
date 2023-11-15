using Shared.Enums;

namespace Services.Interfaces {
    public interface ILoggingService {
        public void Log(string message, EventTypes type);
        public void Log(Exception exc, string? message = null);
    }
}
