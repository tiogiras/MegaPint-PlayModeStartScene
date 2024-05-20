#if UNITY_EDITOR
using Editor.Scripts.Windows;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;
using GUIUtility = Editor.Scripts.GUI.GUIUtility;

namespace Editor.Scripts
{

public class PlayModeStartSceneToggle : MegaPintEditorWindowBase
{
    /// <summary> Loaded uxml references </summary>
    private VisualTreeAsset _baseWindow;

    private Button _btnOff;

    private Button _btnOn;
    private Label _sceneName;

    #region Public Methods

    public override MegaPintEditorWindowBase ShowWindow()
    {
        titleContent.text = "PlayMode Toggle";

        return this;
    }

    #endregion

    #region Protected Methods

    protected override string BasePath()
    {
        return "PlayModeStartScene/User Interface/PlayMode Toggle";
    }

    protected override void CreateGUI()
    {
        base.CreateGUI();

        VisualElement root = rootVisualElement;

        VisualElement content = _baseWindow.Instantiate();
        content.style.flexGrow = 1f;
        content.style.flexShrink = 1f;

        _btnOn = content.Q <Button>("BTN_On");
        _btnOff = content.Q <Button>("BTN_Off");
        _sceneName = content.Q <Label>("SceneName");

        UpdateSceneName();

        VisualButtonUpdate(PlayModeStartSceneData.ToggleState);

        RegisterCallbacks();

        root.Add(content);
    }

    protected override bool LoadResources()
    {
        _baseWindow = Resources.Load <VisualTreeAsset>(BasePath());

        return _baseWindow != null;
    }

    protected override void RegisterCallbacks()
    {
        _btnOn.clickable = new Clickable(() => {Toggle(true);});
        _btnOff.clickable = new Clickable(() => {Toggle(false);});

        PlayModeStartSceneData.onToggleChanged += VisualButtonUpdate;
        PlayModeStartSceneData.onStartSceneChanged += UpdateSceneName;
    }

    protected override void UnRegisterCallbacks()
    {
        _btnOn.clickable = null;
        _btnOff.clickable = null;

        PlayModeStartSceneData.onToggleChanged -= VisualButtonUpdate;
        PlayModeStartSceneData.onStartSceneChanged -= UpdateSceneName;
    }

    #endregion

    #region Private Methods

    private void Toggle(bool on)
    {
        VisualButtonUpdate(on);

        PlayModeStartSceneData.ToggleState = on;

        EditorSceneManager.playModeStartScene = on ? PlayModeStartSceneData.GetStartScene() : null;
    }

    private void UpdateSceneName()
    {
        SceneAsset startScene = PlayModeStartSceneData.GetStartScene();

        if (startScene == null)
        {
            _sceneName.text = "None";
            return;
        }
        
        _sceneName.text = startScene.name;
        _sceneName.tooltip = AssetDatabase.GUIDToAssetPath(PlayModeStartSceneData.StartSceneGuid);
    }

    private void VisualButtonUpdate(bool on)
    {
        GUIUtility.ToggleActiveButtonInGroup(on ? 0 : 1, _btnOn, _btnOff);
    }

    #endregion
}

}
#endif
