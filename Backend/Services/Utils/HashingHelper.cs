using Shared.Configuration;
using System.Security.Cryptography;

namespace Services.Utils {
    internal class HashingHelper : IDisposable {
        public Config Config { get; }

        public HashingHelper(Config config) {
            Config = config;
        }

        public (string Hash, string Salt) HashPassword(string password) {
            byte[] salt = GenerateSalt();
            byte[] hash = Array.Empty<byte>();
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Config.Hashing.Iterations, HashAlgorithmName.SHA256)) {
                hash = pbkdf2.GetBytes(Config.Hashing.HashSize);
            }
            return (Convert.ToBase64String(hash), Convert.ToBase64String(salt));
        }

        public string HashPassword(string password, string salt) {
            byte[] userSalt = Convert.FromBase64String(salt);
            byte[] hash = Array.Empty<byte>();
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, userSalt, Config.Hashing.Iterations, HashAlgorithmName.SHA256)) {
                hash = pbkdf2.GetBytes(Config.Hashing.HashSize);
            }
            return Convert.ToBase64String(hash);
        }

        private byte[] GenerateSalt() {
            byte[] salt = new byte[Config.Hashing.SaltSize];
            RandomNumberGenerator.Fill(salt);
            return salt;
        }

        public void Dispose() { }
    }
}
