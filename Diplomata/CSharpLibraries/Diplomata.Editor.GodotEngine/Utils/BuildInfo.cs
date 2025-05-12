using Godot;

namespace Diplomata.Editor.Utils
{
    public static class BuildInfo
    {
        public static BuildFlags CurrentFlags { get; private set; }

        static BuildInfo()
        {
            CurrentFlags = BuildFlags.None;

#if DEBUG
            CurrentFlags |= BuildFlags.Debug;
#else
            CurrentFlags |= BuildFlags.Release;
#endif

#if TOOLS
            CurrentFlags |= BuildFlags.Editor;
#endif
        }

        public static bool IsDebug() => (CurrentFlags & BuildFlags.Debug) == BuildFlags.Debug && (CurrentFlags & BuildFlags.Editor) == BuildFlags.None;
        public static bool IsRelease() => (CurrentFlags & BuildFlags.Release) == BuildFlags.Release && (CurrentFlags & BuildFlags.Editor) == BuildFlags.None;
        public static bool IsEditor() => (CurrentFlags & BuildFlags.Editor) != BuildFlags.None;
    }
}