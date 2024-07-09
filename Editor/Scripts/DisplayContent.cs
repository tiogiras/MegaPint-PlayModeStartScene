#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;
using GUIUtility = MegaPint.Editor.Scripts.GUI.Utility.GUIUtility;

namespace MegaPint.Editor.Scripts
{

/// <summary> Partial class used to display the right pane in the BaseWindow </summary>
internal static partial class DisplayContent
{
    private static int s_playModeStartSceneObjectPicker;

    #region Private Methods

    /// <summary> Callback when toggle changed </summary>
    /// <param name="root"> RootVisualElement </param>
    private static void PlayModeSceneChange(VisualElement root)
    {
        if (Event.current.commandName != "ObjectSelectorClosed" ||
            EditorGUIUtility.GetObjectPickerControlID() != s_playModeStartSceneObjectPicker)
            return;

        Object scene = EditorGUIUtility.GetObjectPickerObject();

        var path = AssetDatabase.GetAssetPath(scene);

        SaveValues.PlayModeStartScene.StartSceneGuid = AssetDatabase.GUIDFromAssetPath(path).ToString();

        PlayModeStartSceneVisualUpdate(root);

        if (SaveValues.PlayModeStartScene.ToggleState)
            EditorSceneManager.playModeStartScene = SaveValues.PlayModeStartScene.GetStartScene();
    }

    // Called by reflection
    // ReSharper disable once UnusedMember.Local
    private static void PlayModeStartScene(DisplayContentReferences refs)
    {
        InitializeDisplayContent(
            refs,
            new TabSettings {info = true, settings = true},
            new TabActions
            {
                info = root =>
                {
                    GUIUtility.ActivateLinks(
                        root,
                        evt =>
                        {
                            switch (evt.linkID)
                            {
                                case "playModeToggle":
                                    EditorApplication.ExecuteMenuItem(
                                        Constants.PlayModeStartScene.Links.PlayModeToggle);

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
                            s_playModeStartSceneObjectPicker =
                                UnityEngine.GUIUtility.GetControlID(FocusType.Passive) + 100;

                            EditorGUIUtility.ShowObjectPicker <SceneAsset>(
                                null,
                                false,
                                "",
                                s_playModeStartSceneObjectPicker);

                            onRightPaneGUI += PlayModeSceneChange;
                        });

                    var btnOn = root.Q <Button>("BTN_On");
                    var btnOff = root.Q <Button>("BTN_Off");

                    PlayModeStartSceneUpdateToggle(btnOn, btnOff, SaveValues.PlayModeStartScene.ToggleState);

                    btnOn.clickable = new Clickable(() => {PlayModeStartSceneToggle(btnOn, btnOff, true);});

                    btnOff.clickable = new Clickable(() => {PlayModeStartSceneToggle(btnOn, btnOff, false);});

                    SaveValues.PlayModeStartScene.onToggleChanged += on =>
                    {
                        PlayModeStartSceneUpdateToggle(btnOn, btnOff, on);
                    };

                    var displayToolbarToggle = root.Q <Toggle>("DisplayToolbarToggle");
                    displayToolbarToggle.value = SaveValues.PlayModeStartScene.DisplayToolbarToggle;

                    displayToolbarToggle.RegisterValueChangedCallback(
                        evt => {SaveValues.PlayModeStartScene.DisplayToolbarToggle = evt.newValue;});
                }
            });
    }

    /// <summary> Toggle StartSceneToggle </summary>
    /// <param name="btnOn"> On button </param>
    /// <param name="btnOff"> Off button </param>
    /// <param name="on"> State of the toggle </param>
    private static void PlayModeStartSceneToggle(VisualElement btnOn, VisualElement btnOff, bool on)
    {
        PlayModeStartSceneUpdateToggle(btnOn, btnOff, on);

        SaveValues.PlayModeStartScene.ToggleState = on;

        EditorSceneManager.playModeStartScene = on ? SaveValues.PlayModeStartScene.GetStartScene() : null;
    }

    /// <summary> Update toggle </summary>
    /// <param name="btnOn"> On button </param>
    /// <param name="btnOff"> Off button </param>
    /// <param name="on"> State of the toggle </param>
    private static void PlayModeStartSceneUpdateToggle(VisualElement btnOn, VisualElement btnOff, bool on)
    {
        GUIUtility.ToggleActiveButtonInGroup(on ? 0 : 1, btnOn, btnOff);
    }

    /// <summary> Update visuals </summary>
    /// <param name="root"> RootVisualElement </param>
    private static void PlayModeStartSceneVisualUpdate(VisualElement root)
    {
        SceneAsset startScene = SaveValues.PlayModeStartScene.GetStartScene();

        var hasStartScene = startScene != null;

        var sceneName = root.Q <Label>("SceneName");
        sceneName.text = hasStartScene ? startScene.name : "None";

        sceneName.tooltip =
            hasStartScene ? AssetDatabase.GUIDToAssetPath(SaveValues.PlayModeStartScene.StartSceneGuid) : "";
    }

    #endregion
}

}
#endif
