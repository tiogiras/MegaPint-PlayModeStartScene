#if UNITY_EDITOR
using com.tiogiras.megapint_playmodestartscene.Editor.Scripts;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Scripts
{

internal static partial class DisplayContent
{
    private static int s_playModeStartSceneObjectPicker;
    
    #region Private Methods

    // Called by reflection
    // ReSharper disable once UnusedMember.Local
    private static void PlayModeStartScene(VisualElement root)
    {
        var btnChange = root.Q <Button>("BTN_Change");
        var btnOpen = root.Q <Button>("BTN_Open");

        PlayModeStartSceneVisualUpdate(root);

        //onrightpanegui
        btnChange.clickable = new Clickable(
            () =>
            {
                s_playModeStartSceneObjectPicker = GUIUtility.GetControlID(FocusType.Passive) + 100;

                //use the ID you just created
                EditorGUIUtility.ShowObjectPicker <SceneAsset>(null, false, "", s_playModeStartSceneObjectPicker);

                onRightPaneGUI += PlayModeSceneChange;
            });

        btnOpen.clickable = new Clickable(
            () => {ContextMenu.TryOpen <PlayModeStartSceneToggle>(false);});
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
        var path = AssetDatabase.GetAssetPath(scene);

        PlayModeStartSceneData.StartSceneGuid = AssetDatabase.GUIDFromAssetPath(path).ToString();
            
        PlayModeStartSceneVisualUpdate(root);
    }

    #endregion
}

}
#endif
