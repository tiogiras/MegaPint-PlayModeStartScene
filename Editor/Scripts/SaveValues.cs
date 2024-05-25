using System;
using MegaPint.Editor.Scripts.Settings;
using UnityEditor;

namespace MegaPint.Editor.Scripts
{

/// <summary> Partial class storing saveData values (PlayModeStartScene) </summary>
internal static partial class SaveValues
{
    public static class PlayModeStartScene
    {
        public static Action <bool> onToggleChanged;
        public static Action onStartSceneChanged;
        private static CacheValue <string> s_startSceneGuid = new() {defaultValue = ""};
        private static CacheValue <bool> s_toggleState = new() {defaultValue = false};

        private static SettingsBase s_settings;

        public static string StartSceneGuid
        {
            get => ValueProperty.Get("startSceneGUID", ref s_startSceneGuid, _Settings);
            set
            {
                ValueProperty.Set("startSceneGUID", value, ref s_startSceneGuid, _Settings);
                onStartSceneChanged?.Invoke();
            }
        }

        public static bool ToggleState
        {
            get => ValueProperty.Get("toggleState", ref s_toggleState, _Settings);
            set
            {
                ValueProperty.Set("toggleState", value, ref s_toggleState, _Settings);
                onToggleChanged?.Invoke(value);
            }
        }

        private static SettingsBase _Settings
        {
            get
            {
                if (MegaPintSettings.Exists())
                    return s_generalSettings ??= MegaPintSettings.instance.GetSetting("PlayModeStartScene");

                return null;
            }
        }

        #region Public Methods

        public static SceneAsset GetStartScene()
        {
            return AssetDatabase.LoadAssetAtPath <SceneAsset>(AssetDatabase.GUIDToAssetPath(StartSceneGuid));
        }

        #endregion
    }
}

}
