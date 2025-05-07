using System;
using System.Linq;
using Diplomata.Models;
using Godot;

namespace Diplomata.Scenes
{
    public partial class MainScene : Control
    {
        private Label _exampleText;

        public override void _Ready()
        {
            var name = "Example Test";
            var example = new Example { Name = name };
            
            try {
                example = Main.Instance.Database.Examples.Where(e => e.Name == name).Take(1).Single();
                Main.Instance.Database.Examples.Update(example);
            } catch (Exception err) {
                GD.PrintErr(err);
                Main.Instance.Database.Examples.Add(example);
            }

            Main.Instance.Database.SaveChanges();

            _exampleText = GetNode<Label>("ExampleText");
            _exampleText.Text = $"Added example with GUID: {example.Guid}";
        }
    }
}