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

        private readonly string[] _KEYS_SCOPE = new[]{
            EnvVarKeys.STAGE,
        };

        public EnvironmentLoader(bool useDotEnvFile = false, string envFilePath = null, Dictionary<string, string> forceVariables = null)
        {
            try
            {
                envFilePath = envFilePath == null ?
                Path.Combine(Directory.GetCurrentDirectory(), ".env") : envFilePath;
                Load(useDotEnvFile, envFilePath, forceVariables);
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

        private void Load(bool useDotEnvFile, string envFilePath, Dictionary<string, string> forceVariables)
        {
            _variables = new Dictionary<string, string> { };
            forceVariables = forceVariables == null ? new Dictionary<string, string> { } : forceVariables;

            var populateEnvFile = (string key) =>
            {
                try
                {
                    var fallbackValue = GetFallbackValue(key);
                    if (useDotEnvFile)
                    {
                        File.AppendAllText(envFilePath, $"{key}=\"{fallbackValue}\"\n");
                    }
                    _variables.Add(key, fallbackValue);
                }
                catch (Exception err)
                {
                    Logger.Instance.LogErr(err);
                }
            };

            foreach (var key in _KEYS_SCOPE)
            {
                string value;

                if (forceVariables.ContainsKey(key))
                {
                    _variables.Add(key, forceVariables[key]);
                    continue;
                }

                value = System.Environment.GetEnvironmentVariable(key);
                if (!string.IsNullOrEmpty(value))
                {
                    _variables.Add(key, value);
                    continue;
                }

                if (!useDotEnvFile)
                {
                    populateEnvFile.Invoke(key);
                    continue;
                }

                if (!File.Exists(envFilePath))
                {
                    using (File.Create(envFilePath)) {};
                    populateEnvFile.Invoke(key);
                    continue;
                }

                foreach (var line in File.ReadAllLines(envFilePath))
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
                    return Constants.DEV;
                default:
                    return null;
            }
        }
    }
}