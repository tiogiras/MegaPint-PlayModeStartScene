#if UNITY_EDITOR
using MegaPint.Editor.Scripts.GUI.Utility;
using UnityEditor;
using UnityEditor.Toolbars;
using UnityEditor.UIElements;
using UnityEngine;

namespace MegaPint.Editor.Scripts.Logic
{

/// <summary> Handles the toolbar entries of the PlayModeStartScene package </summary>
[InitializeOnLoad]
internal static class PlayModeStartSceneToolbar
{

    private const string _TogglePath = "MegaPint/PlayMode StartScene";
    private static ToolbarToggle s_toolbarToggle;

    static PlayModeStartSceneToolbar()
    {
        if (!SaveValues.PlayModeStartScene.ToolbarInitialized)
        {
            MainToolbarUtility.ForceShowElement(_TogglePath, () =>
            {
                SaveValues.PlayModeStartScene.ToolbarInitialized = true;
            });
        }

        SaveValues.PlayModeStartScene.onToggleChanged += _ => {MainToolbar.Refresh(_TogglePath);};
        SaveValues.PlayModeStartScene.onStartSceneChanged += () => {MainToolbar.Refresh(_TogglePath);};
        SaveValues.BasePackage.onUseIconsChanged += _ => {MainToolbar.Refresh(_TogglePath);};
    }

    [MainToolbarElement(_TogglePath, defaultDockPosition = MainToolbarDockPosition.Middle)]
    public static MainToolbarElement PlayModeStartSceneButton()
    {
        var icon = Resources.Load <Texture2D>(Constants.PlayModeStartScene.Images.ToolbarButton);
        const string text = "PM StartScene";

        MainToolbarContent content = SaveValues.BasePackage.UseToolbarIcons ? new MainToolbarContent(icon) : new MainToolbarContent(text);
        content.tooltip = $"Status: {(SaveValues.PlayModeStartScene.ToggleState ? "active" : "inactive")}\nStartScene: {SaveValues.PlayModeStartScene.GetStartScene()?.name ?? "None"}";

        var element = new MainToolbarToggle(
            content,
            SaveValues.PlayModeStartScene.ToggleState,
            newValue =>
            {
                SaveValues.PlayModeStartScene.ToggleState = newValue;
                MainToolbar.Refresh(_TogglePath);
            });

        return element;
    }
}

}
#endif
