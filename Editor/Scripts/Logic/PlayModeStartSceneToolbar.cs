#if UNITY_EDITOR
using System;
using MegaPint.Editor.Scripts.GUI.Utility;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.Toolbars;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace MegaPint.Editor.Scripts.Logic
{

/// <summary> Handles the toolbar entries of the PlayModeStartScene package </summary>
[InitializeOnLoad]
internal static class PlayModeStartSceneToolbar
{
    private const string _TogglePath = "MegaPint/PlayMode StartScene";
    private static ToolbarToggle s_toolbarToggle;

    private static int s_playModeStartSceneObjectPicker;
    
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
            })
        {
            populateContextMenu = PopulateContextMenu,
        };

        return element;
    }
    
    private static void PopulateContextMenu(DropdownMenu menu)
    {
        menu.AppendAction("Change StartScene", _ =>
        {
            var path = EditorUtility.OpenFilePanelWithFilters(
                "Select new StartScene",
                string.IsNullOrEmpty(SaveValues.PlayModeStartScene.StartSceneGuid) ? Application.dataPath : AssetDatabase.GUIDToAssetPath(SaveValues.PlayModeStartScene.StartSceneGuid),
                new[]
                {
                    "Unity Scene", "unity",
                });
            
            if (string.IsNullOrEmpty(path))
                return;
            
            var projectRoot = Application.dataPath.Replace("\\", "/");
            projectRoot = projectRoot[..^"/Assets".Length];

            path = path.Replace("\\", "/");

            if (!path.StartsWith(projectRoot, StringComparison.OrdinalIgnoreCase))
            {
                EditorUtility.DisplayDialog(
                    "Invalid selection",
                    "Please select a scene that is inside this Unity project (under the Assets folder).",
                    "OK"
                );
                
                return;
            }
            
            var relativePath = path[(projectRoot.Length + 1)..];
            var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(relativePath);
            
            if (sceneAsset == null)
            {
                EditorUtility.DisplayDialog("Not a Scene", "The selected file could not be loaded as a SceneAsset.", "OK");
                return;
            }
            
            SaveValues.PlayModeStartScene.StartSceneGuid = AssetDatabase.GUIDFromAssetPath(relativePath).ToString();

            if (SaveValues.PlayModeStartScene.ToggleState)
                EditorSceneManager.playModeStartScene = SaveValues.PlayModeStartScene.GetStartScene();
        });
        
        menu.AppendAction("Reset", _ =>
        {
            SaveValues.PlayModeStartScene.StartSceneGuid = "";
        });
    }
}

}
#endif
