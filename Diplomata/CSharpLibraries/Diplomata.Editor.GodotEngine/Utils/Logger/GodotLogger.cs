using System;
using Godot;
using Diplomata.Editor.Utils;

namespace Diplomata.Editor.GodotEngine.Utils
{
    internal class GodotLogger : ITransport
    {
        void ITransport.Log(string message)
        {
            GD.Print(message);
        }

        void ITransport.LogErr(Exception error)
        {
            GD.PrintErr(error);
        }

        void ITransport.LogWarn(string message)
        {
            GD.Print(message);
        }

        void ITransport.LogWarn(Exception error)
        {
            GD.PrintErr(error);
        }
    }
}
