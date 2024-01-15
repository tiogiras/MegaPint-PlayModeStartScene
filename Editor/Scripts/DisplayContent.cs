#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Scripts
{

internal static partial class DisplayContent
{
    private static int s_playModeStartSceneObjectPicker;
    
    private static readonly Color s_playModeStartSceneOnColor = new(.8196078431372549f, 0f, .4470588235294118f);
    private static readonly Color s_playModeStartSceneOffColor = new(.34f, .34f, .34f);
    
    #region Private Methods

    // Called by reflection
    // ReSharper disable once UnusedMember.Local
    private static void PlayModeStartScene(VisualElement root)
    {
        PlayModeStartSceneVisualUpdate(root);
        
        root.Q <Button>("BTN_Change").clickable = new Clickable(
            () =>
            {
                s_playModeStartSceneObjectPicker = GUIUtility.GetControlID(FocusType.Passive) + 100;
                
                EditorGUIUtility.ShowObjectPicker <SceneAsset>(null, false, "", s_playModeStartSceneObjectPicker);

                onRightPaneGUI += PlayModeSceneChange;
            });

        root.Q <Button>("BTN_Open").clickable = new Clickable(
            () => {ContextMenu.TryOpen <PlayModeStartSceneToggle>(false);});

        var btnOn = root.Q <Button>("BTN_On");
        var btnOff = root.Q <Button>("BTN_Off");
        
        PlayModeStartSceneUpdateToggle(btnOn, btnOff, PlayModeStartSceneData.ToggleState);
        
        btnOn.clickable = new Clickable(() => {PlayModeStartSceneToggle(btnOn, btnOff, true);});

        btnOff.clickable = new Clickable(() => {PlayModeStartSceneToggle(btnOn, btnOff, false);});

        PlayModeStartSceneData.onToggleChanged += on => {PlayModeStartSceneUpdateToggle(btnOn, btnOff, on);};
    }

    private static void PlayModeStartSceneUpdateToggle(VisualElement btnOn, VisualElement btnOff, bool on)
    {
        btnOn.style.backgroundColor = on ? s_playModeStartSceneOnColor : s_playModeStartSceneOffColor;
        btnOff.style.backgroundColor = on ? s_playModeStartSceneOffColor : s_playModeStartSceneOnColor;
    }

    private static void PlayModeStartSceneToggle(VisualElement btnOn, VisualElement btnOff, bool on)
    {
        PlayModeStartSceneUpdateToggle(btnOn, btnOff, on);
        
        PlayModeStartSceneData.ToggleState = on;

        EditorSceneManager.playModeStartScene = on ? PlayModeStartSceneData.GetStartScene() : null;
    }

    private static void PlayModeStartSceneVisualUpdate(VisualElement root)
    {
        SceneAsset startScene = PlayModeStartSceneData.GetStartScene();

        var hasStartScene = startScene != null;

        var sceneName = root.Q <Label>("SceneName");
        sceneName.text = hasStartScene ? startScene.name : "";
        sceneName.tooltip = hasStartScene ? AssetDatabase.GUIDToAssetPath(PlayModeStartSceneData.StartSceneGuid) : "";
    }

    private static void PlayModeSceneChange(VisualElement root)
    {
        if (Event.current.commandName != "ObjectSelectorClosed" || EditorGUIUtility.GetObjectPickerControlID() != s_playModeStartSceneObjectPicker)
            return;

        Object scene = EditorGUIUtility.GetObjectPickerObject();
        
        if (scene == null)
            return;
        
        var path = AssetDatabase.GetAssetPath(scene);

        PlayModeStartSceneData.StartSceneGuid = AssetDatabase.GUIDFromAssetPath(path).ToString();
            
        PlayModeStartSceneVisualUpdate(root);

        if (PlayModeStartSceneData.ToggleState)
            EditorSceneManager.playModeStartScene = PlayModeStartSceneData.GetStartScene();
    }

    #endregion
}

}
#endif
