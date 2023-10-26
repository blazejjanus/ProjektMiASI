using System.Text.Json.Serialization;
using Shared.Exceptions;
using Shared.Interfaces;

namespace Shared.Configuration {
    public class JWTConfig : IConfigValidation {
        /// <summary>
        /// Number of days the token will be valid after generation
        /// </summary>
        public int DaysValid { get; set; }
        /// <summary>
        /// String or array of strings representing JWT Audience field
        /// </summary>
        public string? Audience { get; set; }
        /// <summary>
        /// Name of JWT Issuer
        /// </summary>
        public string? Issuer { get; set; }
        /// <summary>
        /// Secret key used to sign JWT tokens
        /// </summary>
        public string SecretKey { get; set; }

        public JWTConfig() {
            SecretKey = string.Empty;
        }

        [JsonIgnore]
        public bool IsValid {
            get {
                if (string.IsNullOrEmpty(SecretKey) || SecretKey.Length < 64) {
                    return false;
                }
                if (DaysValid < 1) {
                    return false;
                }
                return true;
            }
        }

        public void ValidateConfig() {
            if (string.IsNullOrEmpty(SecretKey)) throw new ConfigException("JWT SecretKey is empty!");
            if (SecretKey.Length < 64) throw new ConfigException("JWT SecretKey is too short! Minimal SecretKey length is 64.");
            if (DaysValid <= 0) throw new ConfigException("JWT DaysValid is invalid! Minimal value is 1.");
        }
    }
}
