#if UNITY_EDITOR
using MegaPint.Editor.Scripts.Tests.Utility;
using MegaPint.Editor.Scripts.Windows;
using NUnit.Framework;

namespace MegaPint.Editor.Scripts.Tests
{

/// <summary> Unit tests regarding the menuItems of the package </summary>
internal class MenuItemTests
{
    #region Tests

    [Test] [Order(1)]
    public void PlayModeToggle()
    {
        TestsUtility.ValidateMenuItemLink(
            Constants.PlayModeStartScene.Links.PlayModeToggle,
            typeof(PlayModeStartSceneToggle));
    }

    #endregion
}

}
#endif
