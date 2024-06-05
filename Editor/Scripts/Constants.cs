using System.IO;

namespace MegaPint.Editor.Scripts
{

/// <summary> Partial lookup table for constants containing PlayModeStartScene values  </summary>
internal static partial class Constants
{
    public static class PlayModeStartScene
    {
        private static readonly string s_base = Path.Combine("MegaPint", "PlayModeStartScene");
        private static readonly string s_resources = Path.Combine(s_base, "Resources");
        private static readonly string s_userInterface = Path.Combine(s_resources, "User Interface");
        
        public static class UserInterface
        {
            private static readonly string s_windows = Path.Combine(s_userInterface, "Windows");
            public static readonly string PlayModeToggle = Path.Combine(s_windows, "Play Mode Toggle");
        }

        public static class Links
        {
            public const string PlayModeToggle = "MegaPint/Packages/PlayMode Toggle";
        }
    }
}

}
