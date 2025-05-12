using System;
using System.Collections.Generic;
using System.IO;

namespace Diplomata.Editor.Utils
{
    public static class EnvVarKeys
    {
        public const string STAGE = "STAGE";
    }

    public class EnvironmentLoader : IUtil
    {
        private Dictionary<string, string> _variables = null;
        private string _envFilePath;

        private readonly string[] _KEYS_SCOPE = new[]{
            EnvVarKeys.STAGE,
        };

        public EnvironmentLoader(string envFilePath = null)
        {
            this._envFilePath = envFilePath == null ?
                Path.Combine(Directory.GetCurrentDirectory(), ".env") : envFilePath;
            try
            {
                Load();
            }
            catch (Exception err)
            {
                Logger.Instance.LogErr(err);
            }
        }

        public T GetEnvVar<T>(string key)
        {
            string strValue;
            _variables.TryGetValue(key, out strValue);
            return (T)(object)strValue;
        }

        private void Load()
        {
            _variables = new Dictionary<string, string> { };

            var populateEnvFile = (string key) =>
            {
                var fallbackValue = GetFallbackValue(key);
                File.AppendAllText(_envFilePath, $"{key}=\"{fallbackValue}\"");
                _variables.Add(key, fallbackValue);
            };

            foreach (var key in _KEYS_SCOPE)
            {
                string value;

                value = System.Environment.GetEnvironmentVariable(key);
                if (!string.IsNullOrEmpty(value))
                {
                    _variables.Add(key, value);
                    continue;
                }

                if (!File.Exists(_envFilePath))
                {
                    File.Create(_envFilePath);
                    populateEnvFile.Invoke(key);
                    continue;
                }

                foreach (var line in File.ReadAllLines(_envFilePath))
                {
                    if (string.IsNullOrWhiteSpace(line) || line.TrimStart().StartsWith("#"))
                    {
                        continue;
                    }

                    var parts = line.Split(new[] { '=' }, 2, StringSplitOptions.RemoveEmptyEntries);

                    if (parts.Length != 2)
                    {
                        continue;
                    }

                    if (parts[0] == key)
                    {
                        _variables.Add(key, parts[1].Replace("\"", ""));
                        break;
                    }
                }

                _variables.TryGetValue(key, out value);
                if (string.IsNullOrEmpty(value))
                {
                    populateEnvFile.Invoke(key);
                }
            }
        }

        private string GetFallbackValue(string key)
        {
            switch (key)
            {
                case EnvVarKeys.STAGE:
                    return "Local";
                default:
                    return null;
            }
        }
    }
}