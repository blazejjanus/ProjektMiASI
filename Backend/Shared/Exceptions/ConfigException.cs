namespace Shared.Exceptions {
    public class ConfigException : Exception {
        private static readonly string _messagePrefix = "Configuration Exception: ";
        public ConfigException(string message) : base(_messagePrefix + message) { }
        public ConfigException(string message, Exception innerException) : base(_messagePrefix + message, innerException) { }
    }
}
