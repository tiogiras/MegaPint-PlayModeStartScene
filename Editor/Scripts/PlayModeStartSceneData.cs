using Editor.Scripts.Settings;
using UnityEditor;

namespace com.tiogiras.megapint_playmodestartscene.Editor.Scripts
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

    public static string StartSceneGuid
    {
        get => _Settings.GetValue(s_startSceneGuid.key, s_startSceneGuid.defaultValue);
        set => _Settings.SetValue(s_startSceneGuid.key, value);
    }

    private static MegaPintSettingsBase _Settings => MegaPintSettings.instance.GetSetting(SettingsName);

    public static SceneAsset GetStartScene()
    {
        return AssetDatabase.LoadAssetAtPath <SceneAsset>(AssetDatabase.GUIDToAssetPath(StartSceneGuid));
    }
}

}
