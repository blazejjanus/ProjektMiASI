using System.Text.Json.Serialization;

namespace Shared {
    public class JWTConfig {
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
                if(DaysValid < 1) {
                    return false;
                }
                return true;
            }
        }
    }
}
