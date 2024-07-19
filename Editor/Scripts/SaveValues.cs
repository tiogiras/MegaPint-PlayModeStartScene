#if UNITY_EDITOR
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
        public static Action <bool> onDisplayToolbarToggleChanged;
        public static Action onStartSceneChanged;
        
        private static CacheValue <string> s_startSceneGuid = new() {defaultValue = ""};
        private static CacheValue <bool> s_toggleState = new() {defaultValue = false};
        private static CacheValue <bool> s_displayToolbarToggle = new() {defaultValue = true};

        private static CacheValue <bool> s_applyPsToggleWindow = new() {defaultValue = true};

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

        public static bool ApplyPSToggleWindow
        {
            get => ValueProperty.Get("ApplyPS_ToggleWindow", ref s_applyPsToggleWindow, _Settings);
            set => ValueProperty.Set("ApplyPS_ToggleWindow", value, ref s_applyPsToggleWindow, _Settings);
        }
        
        public static bool DisplayToolbarToggle
        {
            get => ValueProperty.Get("DisplayToolbarToggle", ref s_displayToolbarToggle, _Settings);
            set
            {
                ValueProperty.Set("DisplayToolbarToggle", value, ref s_displayToolbarToggle, _Settings);
                onDisplayToolbarToggleChanged?.Invoke(value);
            }
        }

        private static SettingsBase _Settings
        {
            get
            {
                if (MegaPintSettings.Exists())
                    return s_settings ??= MegaPintSettings.instance.GetSetting("PlayModeStartScene");

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
#endif
