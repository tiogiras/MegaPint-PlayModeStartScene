// TODO commenting

#if UNITY_EDITOR
using MegaPint.Editor.Scripts.GUI;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace MegaPint.Editor.Scripts.Logic
{

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
                        s_toolbarToggle = ToolbarExtension.CreateToolbarToggle("PlayMode StartScene", OnToolbarToggleChanged);

                    s_toolbarToggle.SetValueWithoutNotify(SaveValues.PlayModeStartScene.ToggleState);
                    root.Add(s_toolbarToggle);

                    SaveValues.PlayModeStartScene.onToggleChanged += newValue =>
                    {
                         s_toolbarToggle.SetValueWithoutNotify(newValue);
                    };
                }
            });
    }

    #region Private Methods

    private static void OnToolbarCreation(VisualElement element)
    {
    }

    private static void OnToolbarToggleChanged(bool newValue)
    {
        SaveValues.PlayModeStartScene.ToggleState = newValue;
    }

    #endregion
}

}
#endif
