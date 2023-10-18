﻿using System.Text.Json;
using System.Text.Json.Serialization;

namespace Shared {
    public class Config {
        [JsonPropertyName("IsDevelopmentEnvironment")]
        public bool IsDevelopmentEnvironment { get; set; }
        [JsonPropertyName("JWT")]
        public JWTConfig JWT { get; set; }
        [JsonPropertyName("ConnectionString")]
        public string ConnectionString { get; set; }
        [JsonPropertyName("UseLogFile")]
        public bool UseLogFile { get; set; }
        [JsonIgnore]
        public bool IsJWTValid => JWT.IsValid;

        public Config() { 
            JWT = new JWTConfig();
            ConnectionString = string.Empty;
            IsDevelopmentEnvironment = false;
        }

        public static Config ReadConfig() {
            var env = Environment.GetInstance();
            string json = File.ReadAllText(env.ConfigFilePath);
            if(string.IsNullOrEmpty(json)) { throw new Exception("Obtained json config was empty!"); }
            var config = JsonSerializer.Deserialize<Config>(json);
            if(config == null) { throw new Exception("Deserialized config was null!"); }
            if(config.JWT == null) { throw new Exception("Cannot Deserialize JWT config!"); }
            config.AdjustConnectionString();
            return config;
        }

        public static void WriteDefaultConfig(string? path = null) {
            if(path == null) {
                path = Environment.GetInstance().ConfigFilePath;
            }
            var config = new Config();
            var json = JsonSerializer.Serialize(config, new JsonSerializerOptions() { WriteIndented = true });
            File.WriteAllText(path, json);
        }

        public void AdjustConnectionString() {
            var env = Environment.GetInstance();
            if (ConnectionString.Contains("{DbPath}")) {
                ConnectionString = ConnectionString.Replace("{DbPath}", env.DbPath);
            }
        }

        public string ToJson() {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions() { WriteIndented = true });
        }
    }
}
