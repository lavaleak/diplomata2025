using System;
using System.Linq;
using Godot;
using Diplomata.Editor.Models.Database;
using Diplomata.Editor.Utils;
using Diplomata.Editor.GodotEngine.AutoLoads;
using Diplomata.Editor.GodotEngine.Nodes;

namespace Diplomata.Editor.GodotEngine.Scenes
{
    public partial class MainScene : UiBaseNode
    {
        private Label _exampleText;
        private Database _db;
        private EnvironmentLoader _env;

        public override void _Ready()
        {
            _db = Main.Dependency<Database>();
            _env = Main.Dependency<EnvironmentLoader>();

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