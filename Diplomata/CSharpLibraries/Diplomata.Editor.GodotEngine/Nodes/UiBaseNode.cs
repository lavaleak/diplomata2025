using Godot;
using Diplomata.Editor.GodotEngine.AutoLoads;

namespace Diplomata.Editor.GodotEngine.Nodes
{
    public partial class UiBaseNode : Control
    {
        private Main _main;

        protected Main Main
        {
            get
            {
                if (_main == null)
                {
                    _main = GetNode<Main>("/root/Main");
                }
                return _main;
            }
        }
    }
}