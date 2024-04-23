#if UNITY_EDITOR
using Editor.Scripts.GUI;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;
using GUIUtility = Editor.Scripts.GUI.GUIUtility;

namespace Editor.Scripts
{

internal static partial class DisplayContent
{
    private static int s_playModeStartSceneObjectPicker;

    private static readonly Color s_playModeStartSceneOnColor = RootElement.Colors.Primary;
    private static readonly Color s_playModeStartSceneOffColor = RootElement.Colors.Button;

    #region Private Methods

    // Called by reflection
    // ReSharper disable once UnusedMember.Local
    private static void PlayModeStartScene(DisplayContentReferences refs)
    {
        InitializeDisplayContent(
            refs,
            new TabSettings
            {
                info = true,
                settings = true
            },
            new TabActions
            {
                info = root =>
                {
                    GUIUtility.ActivateLinks(root, 
                                             evt =>
                                             {
                                                 switch (evt.linkID)
                                                 {
                                                     case "playModeToggle":
                                                         EditorApplication.ExecuteMenuItem(
                                                             "MegaPint/Packages/PlayMode Toggle");
                                                         break;
                                                 }
                                             });
                },
                settings = root =>
                {
                    PlayModeStartSceneVisualUpdate(root);
                    
                    root.Q <Button>("BTN_Change").clickable = new Clickable(
                        () =>
                        {
                            s_playModeStartSceneObjectPicker = UnityEngine.GUIUtility.GetControlID(FocusType.Passive) + 100;

                            EditorGUIUtility.ShowObjectPicker <SceneAsset>(null, false, "", s_playModeStartSceneObjectPicker);

                            onRightPaneGUI += PlayModeSceneChange;
                        });
                    
                    var btnOn = root.Q <Button>("BTN_On");
                    var btnOff = root.Q <Button>("BTN_Off");
                    
                    PlayModeStartSceneUpdateToggle(btnOn, btnOff, PlayModeStartSceneData.ToggleState);
                    
                    btnOn.clickable = new Clickable(() => {PlayModeStartSceneToggle(btnOn, btnOff, true);});

                    btnOff.clickable = new Clickable(() => {PlayModeStartSceneToggle(btnOn, btnOff, false);});

                    PlayModeStartSceneData.onToggleChanged += on => {PlayModeStartSceneUpdateToggle(btnOn, btnOff, on);};
                }
            });
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
    
    private static void PlayModeStartSceneVisualUpdate(VisualElement root)
    {
        SceneAsset startScene = PlayModeStartSceneData.GetStartScene();

        var hasStartScene = startScene != null;

        var sceneName = root.Q <Label>("SceneName");
        sceneName.text = hasStartScene ? startScene.name : "None";
        sceneName.tooltip = hasStartScene ? AssetDatabase.GUIDToAssetPath(PlayModeStartSceneData.StartSceneGuid) : "";
    }

    private static void PlayModeStartSceneToggle(VisualElement btnOn, VisualElement btnOff, bool on)
    {
        PlayModeStartSceneUpdateToggle(btnOn, btnOff, on);

        PlayModeStartSceneData.ToggleState = on;

        EditorSceneManager.playModeStartScene = on ? PlayModeStartSceneData.GetStartScene() : null;
    }

    private static void PlayModeStartSceneUpdateToggle(VisualElement btnOn, VisualElement btnOff, bool on)
    {
        btnOn.style.backgroundColor = on ? s_playModeStartSceneOnColor : s_playModeStartSceneOffColor;
        btnOff.style.backgroundColor = on ? s_playModeStartSceneOffColor : s_playModeStartSceneOnColor;
    }

    #endregion
}

}
#endif
