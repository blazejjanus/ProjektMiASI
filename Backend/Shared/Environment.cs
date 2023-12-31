﻿using System.Text;

namespace Shared {
    public class Environment {
        private static Environment? _instance;
        public string RootPath { get; private set; }
        public string LogPath { get; private set; }
        public string DbPath { get; private set; }
        public string ConfigFilePath { get; private set; }

        private Environment() { 
            //Calculate propper RootPath
            var path = Directory.GetCurrentDirectory();
            path = SplitPath(path, ".build");
            path = SplitPath(path, "bin");
            path = SplitPath(path, "API");
            path = SplitPath(path, "Tests");
            path = SplitPath(path, "DB");
            path = SplitPath(path, "Shared");
            path = SplitPath(path, "Services");
            RootPath = path;
            //Fill and validate aditional pathes
            //LogPath
            var logs = Path.Combine(RootPath, ".log");
            if (!Directory.Exists(logs)) {
                Directory.CreateDirectory(logs);
            }
            LogPath = logs;
            //DbPath
            var db = Path.Combine(RootPath, ".database");
            if(!Directory.Exists(db)) {
                Directory.CreateDirectory(db);
            }
            DbPath = db;
            //ConfigFilePath
            ConfigFilePath = Path.Combine(RootPath, "config.json");
            if(!File.Exists(ConfigFilePath)) {
                throw new FileNotFoundException(ConfigFilePath);
            }
        }

        public static Environment GetInstance() {
            if (_instance == null) {
                _instance = new Environment();
            }
            return _instance;
        }

        public string ToString(bool writeIntended = false) {
            string prefix = string.Empty;
            if (writeIntended) {
                prefix = "\t";
            }
            var sb = new StringBuilder();
            sb.AppendLine(prefix + "RootPath:       " + RootPath);
            sb.AppendLine(prefix + "LogPath:        " + LogPath);
            sb.AppendLine(prefix + "DbPath:         " + DbPath);
            sb.AppendLine(prefix + "ConfigFilePath: " + ConfigFilePath);
            return sb.ToString();
        }

        private string SplitPath(string path, string splitter) {
            if (path.Contains(splitter)) {
                path = path.Split(splitter).First();
            }
            return path;
        }
    }
}