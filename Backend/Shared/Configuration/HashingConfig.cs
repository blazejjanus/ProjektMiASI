using Shared.Exceptions;
using Shared.Interfaces;

namespace Shared.Configuration {
    public class HashingConfig : IConfigValidation {
        /// <summary>
        /// Size of the salt in bytes. Range is 8-24.
        /// </summary>
        public int SaltSize { get; set; }
        /// <summary>
        /// Size of resulting hash in bytes. Range is 8-128.
        /// </summary>
        public int HashSize { get; set; }
        /// <summary>
        /// Number of iterations to perform. Minimum is 1000.
        /// </summary>
        public int Iterations { get; set; }
        public HashingConfig() {
            SaltSize = 24;
            HashSize = 32;
            Iterations = 10000;
        }

        public void ValidateConfig() {
            if (SaltSize < 8 || SaltSize > 24) throw new ConfigException("SaltSize is invalid! Range is 8-24.");
            if (HashSize < 8 || HashSize > 128) throw new ConfigException("HashSize is invalid! Range is 8-128.");
            if (Iterations < 1000) throw new ConfigException("Iterations is invalid! Minimal value is 1000.");
        }
    }
}
