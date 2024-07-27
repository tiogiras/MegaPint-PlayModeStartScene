#if UNITY_EDITOR
using System;
using MegaPint.Editor.Scripts.Settings;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace MegaPint.Editor.Scripts.Logic
{

/// <summary> Handles main logic of the PlayModeStartScene package </summary>
[InitializeOnLoad]
internal static class PlayModeStartScene
{
    public static Action onEnteredPlaymodeWithStartScene;
    public static Action onEnteredPlaymode;
    public static Action onExitedPlaymode;
    private static bool s_toggleState;
    private static SceneAsset s_startScene;

    static PlayModeStartScene()
    {
        MegaPintSettings.onLoaded += SetStartSceneInitially;

        SaveValues.PlayModeStartScene.onToggleChanged += OnToggleStateChanged;
        SaveValues.PlayModeStartScene.onStartSceneChanged += OnStartSceneChanged;

        EditorApplication.playModeStateChanged += evt =>
        {
            if (evt != PlayModeStateChange.EnteredPlayMode)
            {
                if (evt == PlayModeStateChange.EnteredEditMode)
                    onExitedPlaymode?.Invoke();

                return;
            }

            if (SaveValues.PlayModeStartScene.ToggleState)
            {
                Debug.Log("Entered selected StartScene via MegaPint-PlayMode Start Scene");
                onEnteredPlaymodeWithStartScene?.Invoke();
            }
            else
                onEnteredPlaymode?.Invoke();
        };
    }

    #region Private Methods

    /// <summary> Callback when startScene was changed </summary>
    private static void OnStartSceneChanged()
    {
        s_startScene = SaveValues.PlayModeStartScene.GetStartScene();
        SetStartScene();
    }

    /// <summary> Callback when the toggle state was changed </summary>
    /// <param name="newValue"> New value of the toggle </param>
    private static void OnToggleStateChanged(bool newValue)
    {
        s_toggleState = newValue;
        SetStartScene();
    }

    /// <summary> Set the startScene of the editor </summary>
    private static void SetStartScene()
    {
        EditorSceneManager.playModeStartScene = s_toggleState ? s_startScene : null;
    }

    /// <summary> Set the start scene in unity </summary>
    private static void SetStartSceneInitially()
    {
        s_toggleState = SaveValues.PlayModeStartScene.ToggleState;
        s_startScene = SaveValues.PlayModeStartScene.GetStartScene();

        SetStartScene();

        MegaPintSettings.onLoaded -= SetStartSceneInitially;
    }

    #endregion
}

}
#endif
