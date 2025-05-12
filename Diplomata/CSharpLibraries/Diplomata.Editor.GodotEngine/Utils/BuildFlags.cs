using System;

namespace Diplomata.Editor.Utils
{
    [Flags]
    public enum BuildFlags
    {
        None = 0,
        Debug = 1,
        Release = 2,
        Editor = 4
    }
}