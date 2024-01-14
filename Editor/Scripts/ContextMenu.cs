#if UNITY_EDITOR
using com.tiogiras.megapint_playmodestartscene.Editor.Scripts;
using UnityEditor;

namespace Editor.Scripts
{

internal static partial class ContextMenu
{
    #region Private Methods

    [MenuItem(MenuItemPackages + "/PlayMode Toggle", false, 13)]
    private static void OpenPlayModeToggle()
    {
        TryOpen <PlayModeStartSceneToggle>(false);
    }

    #endregion
}

}
#endif
