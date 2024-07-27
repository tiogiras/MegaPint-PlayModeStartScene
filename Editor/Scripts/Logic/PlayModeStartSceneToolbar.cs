#if UNITY_EDITOR
using System.Text;
using MegaPint.Editor.Scripts.GUI;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace MegaPint.Editor.Scripts.Logic
{

/// <summary> Handles the toolbar entries of the PlayModeStartScene package </summary>
[InitializeOnLoad]
internal static class PlayModeStartSceneToolbar
{
    private static ToolbarToggle s_toolbarToggle;

    static PlayModeStartSceneToolbar()
    {
        if (!SaveValues.PlayModeStartScene.DisplayToolbarToggle)
            return;

        ToolbarExtension.AddRightZoneAction(
            new ToolbarExtension.GUIAction
            {
                executionIndex = 1,
                action = root =>
                {
                    if (SaveValues.BasePackage.UseToolbarIcons)
                    {
                        s_toolbarToggle = ToolbarExtension.CreateToolbarToggle(
                            Constants.PlayModeStartScene.UserInterface.ToolbarButton,
                            OnToolbarCreation,
                            OnToolbarToggleChanged);
                    }
                    else
                        s_toolbarToggle = ToolbarExtension.CreateToolbarToggle(
                            "PlayMode StartScene",
                            OnToolbarToggleChanged);

                    s_toolbarToggle.SetValueWithoutNotify(SaveValues.PlayModeStartScene.ToggleState);
                    root.Add(s_toolbarToggle);

                    SaveValues.PlayModeStartScene.onToggleChanged += newValue =>
                    {
                        s_toolbarToggle.SetValueWithoutNotify(newValue);
                        SetTooltip();
                    };

                    SaveValues.PlayModeStartScene.onStartSceneChanged += SetTooltip;

                    SetTooltip();
                }
            });
    }

    #region Private Methods

    /// <summary> Called when the toolbar toggle was created </summary>
    /// <param name="element"> Toolbar toggle element </param>
    private static void OnToolbarCreation(VisualElement element)
    {
    }

    /// <summary> Callback for the toolbar toggle </summary>
    /// <param name="newValue"> New value of the toggle </param>
    private static void OnToolbarToggleChanged(bool newValue)
    {
        SaveValues.PlayModeStartScene.ToggleState = newValue;
    }

    /// <summary> Set the tooltip of the toolbar toggle </summary>
    private static void SetTooltip()
    {
        var tooltip = new StringBuilder();

        tooltip.AppendLine($"Status: {(SaveValues.PlayModeStartScene.ToggleState ? "active" : "inactive")}");
        tooltip.Append($"StartScene: {SaveValues.PlayModeStartScene.GetStartScene()?.name ?? "None"}");

        s_toolbarToggle.tooltip = tooltip.ToString();
    }

    #endregion
}

}
#endif
