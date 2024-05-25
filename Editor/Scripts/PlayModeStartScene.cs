#if UNITY_EDITOR
using MegaPint.Editor.Scripts.Settings;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace MegaPint.Editor.Scripts
{

/// <summary> Handles main logic of the PlayModeStartScene package </summary>
[InitializeOnLoad]
public static class PlayModeStartScene
{
    static PlayModeStartScene()
    {
        MegaPintSettings.onLoaded += SetStartScene;

        EditorApplication.playModeStateChanged += evt =>
        {
            if (evt != PlayModeStateChange.EnteredPlayMode)
                return;

            if (SaveValues.PlayModeStartScene.ToggleState)
                Debug.Log("Entered selected StartScene via MegaPint-PlayMode Start Scene");
        };
    }

    #region Private Methods

    /// <summary> Set the startScene of the editor </summary>
    private static void SetStartScene()
    {
        if (SaveValues.PlayModeStartScene.ToggleState)
            EditorSceneManager.playModeStartScene = SaveValues.PlayModeStartScene.GetStartScene();

        MegaPintSettings.onLoaded -= SetStartScene;
    }

    #endregion
}

}
#endif
