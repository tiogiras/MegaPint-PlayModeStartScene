#if UNITY_EDITOR
using System.IO;

namespace MegaPint.Editor.Scripts
{

/// <summary> Partial lookup table for constants containing PlayModeStartScene values  </summary>
internal static partial class Constants
{
    public static class PlayModeStartScene
    {
        public static class Links
        {
            public static readonly string PlayModeToggle = Utility.CombineMenuItemPath(
                ContextMenu.MenuItemPackages,
                "PlayMode Toggle");
        }

        public static class UserInterface
        {
            private static readonly string s_windows = Path.Combine(s_userInterface, "Windows");
            public static readonly string PlayModeToggle = Path.Combine(s_windows, "Play Mode Toggle");
        }

        public static class Images
        {
            public static readonly string ToolbarButton = Path.Combine(s_images, "Toolbar Button");
        }

        private static readonly string s_base = Path.Combine("MegaPint", "PlayModeStartScene");
        private static readonly string s_userInterface = Path.Combine(s_base, "User Interface");
        private static readonly string s_images = Path.Combine(s_base, "Images");
    }
}

}
#endif
