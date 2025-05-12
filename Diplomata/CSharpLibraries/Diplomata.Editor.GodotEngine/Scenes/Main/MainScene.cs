using System;
using System.Linq;
using Godot;
using Diplomata.Editor.Models.Database;
using Diplomata.Editor.Utils;
using Diplomata.Editor.GodotEngine.AutoLoads;

namespace Diplomata.Editor.GodotEngine.Scenes
{
    public partial class MainScene : Control
    {
        private Label _exampleText;
        private Database _db;
        private EnvironmentLoader _env;

        public override void _Ready()
        {
            var main = GetNode<Main>("/root/Main");
            _db = main.Dependency<Database>();
            _env = main.Dependency<EnvironmentLoader>();

            var name = "Example Test";
            var example = new Example { Name = name };
            var stage = _env.GetEnvVar<string>(EnvVarKeys.STAGE);

            try
            {
                example = _db.Examples.Where(e => e.Name == name).Take(1).Single();
                _db.Examples.Update(example);
            }
            catch (Exception err)
            {
                GD.PrintErr(err);
                _db.Examples.Add(example);
            }

            _db.SaveChanges();

            _exampleText = GetNode<Label>("ExampleText");
            _exampleText.Text = $"Added example with GUID: {example.Guid}\nStage: {stage}";
        }
    }
}