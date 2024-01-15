#if UNITY_EDITOR
using Editor.Scripts.Settings;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Editor.Scripts
{

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

            if (PlayModeStartSceneData.ToggleState)
                Debug.Log("Entered selected StartScene via MegaPint-PlayMode Start Scene");
        };
    }

    #region Private Methods

    private static void SetStartScene()
    {
        if (PlayModeStartSceneData.ToggleState)
            EditorSceneManager.playModeStartScene = PlayModeStartSceneData.GetStartScene();

        MegaPintSettings.onLoaded -= SetStartScene;
    }

    #endregion
}

}
#endif
