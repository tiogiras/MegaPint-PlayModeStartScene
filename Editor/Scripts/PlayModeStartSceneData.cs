#if UNITY_EDITOR
using System;
using Editor.Scripts.Settings;
using UnityEditor;
using Action = Unity.Plastic.Newtonsoft.Json.Serialization.Action;

namespace Editor.Scripts
{

public class PlayModeStartSceneData
{
    private struct SettingsValue <T>
    {
        public string key;
        public T defaultValue;
    }

    private const string SettingsName = "MegaPint.PlayModeStartScene";

    private static readonly SettingsValue <string> s_startSceneGuid = new() {key = "startSceneGUID", defaultValue = ""};
    private static readonly SettingsValue <bool> s_toggleState = new() {key = "toggleState", defaultValue = false};

    public static Action<bool> onToggleChanged;
    public static Action onStartSceneChanged;
    
    public static string StartSceneGuid
    {
        get => _Settings.GetValue(s_startSceneGuid.key, s_startSceneGuid.defaultValue);
        set
        {
            _Settings.SetValue(s_startSceneGuid.key, value);
            onStartSceneChanged?.Invoke();
        }
    }

    public static bool ToggleState
    {
        get => _Settings.GetValue(s_toggleState.key, s_toggleState.defaultValue);
        set
        {
            _Settings.SetValue(s_toggleState.key, value);
            onToggleChanged?.Invoke(value);
        }
    }

    private static MegaPintSettingsBase _Settings => MegaPintSettings.instance.GetSetting(SettingsName);

    public static SceneAsset GetStartScene()
    {
        return AssetDatabase.LoadAssetAtPath <SceneAsset>(AssetDatabase.GUIDToAssetPath(StartSceneGuid));
    }
}

}
#endif
