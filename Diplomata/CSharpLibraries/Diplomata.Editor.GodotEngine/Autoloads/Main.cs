using System.Collections.Generic;
using System.IO;
using Diplomata.Editor.GodotEngine.Utils;
using Diplomata.Editor.Utils;
using Godot;

namespace Diplomata.Editor.GodotEngine.AutoLoads
{
    public partial class Main : Node
    {
        private Dictionary<string, IUtil> _dependencies = new Dictionary<string, IUtil>();

        override public void _Ready()
        {
            var database = new Database(OS.GetUserDataDir().Replace("\\", "/"));
            database.Database.EnsureCreated();
            _addDependency(database);

            var envFilePath = Path.Combine(
                    ProjectSettings.GlobalizePath("res://"),
                    ".env"
            );
            var useDotEnvFile = BuildInfo.IsEditor();
            var variables =
            BuildInfo.IsRelease() ?
                new Dictionary<string, string>{
                    { EnvVarKeys.STAGE, Constants.RELEASE }
                } : BuildInfo.IsDebug() ?
                new Dictionary<string, string>{
                    { EnvVarKeys.STAGE, Constants.DEBUG }
                } : null;

            var environment = new EnvironmentLoader(useDotEnvFile, envFilePath, variables);
            _addDependency(environment);

            Logger.Instance.AddTransport(new GodotLogger());
            var _logger = Logger.Instance;
            _addDependency(_logger);
        }

        public T Dependency<T>() where T : IUtil
        {
            return (T)_dependencies[typeof(T).Name];
        }

        private void _addDependency<T>(T dependency) where T : IUtil
        {
            _dependencies.Add(dependency.GetType().Name, dependency);
        }
    }
}