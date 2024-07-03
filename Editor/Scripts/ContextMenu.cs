#if UNITY_EDITOR
using MegaPint.Editor.Scripts.PackageManager.Packages;
using MegaPint.Editor.Scripts.Windows;
using UnityEditor;

namespace MegaPint.Editor.Scripts
{

/// <summary> Partial class used to store MenuItems </summary>
internal static partial class ContextMenu
{
    public static class PlayModeStartScene
    {
        private static readonly MenuItemSignature s_playModeToggleSignature = new()
        {
            package = PackageKey.PlayModeStartScene, signature = "PlayMode Toggle"
        };

        #region Private Methods

        [MenuItem(MenuItemPackages + "/PlayMode Toggle", false, 13)]
        private static void OpenPlayModeToggle()
        {
            TryOpen <PlayModeStartSceneToggle>(false, s_playModeToggleSignature);
        }

        #endregion
    }
}

}
#endif
