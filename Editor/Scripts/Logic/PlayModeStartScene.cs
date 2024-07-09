// TODO commenting

#if UNITY_EDITOR
using MegaPint.Editor.Scripts.Settings;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace MegaPint.Editor.Scripts.Logic
{

/// <summary> Handles main logic of the PlayModeStartScene package </summary>
[InitializeOnLoad]
public static class PlayModeStartScene
{
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
                return;

            if (SaveValues.PlayModeStartScene.ToggleState)
                Debug.Log("Entered selected StartScene via MegaPint-PlayMode Start Scene");
        };
    }

    #region Private Methods

    private static void OnStartSceneChanged()
    {
        s_startScene = SaveValues.PlayModeStartScene.GetStartScene();
        SetStartScene();
    }

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
