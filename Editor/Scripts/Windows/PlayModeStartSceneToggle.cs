#if UNITY_EDITOR
using MegaPint.Editor.Scripts.GUI.Utility;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;
using GUIUtility = MegaPint.Editor.Scripts.GUI.Utility.GUIUtility;

namespace MegaPint.Editor.Scripts.Windows
{

/// <summary> Editor window to toggle on/off the playModeStartScene behaviour </summary>
public class PlayModeStartSceneToggle : EditorWindowBase
{
    /// <summary> Loaded uxml references </summary>
    private VisualTreeAsset _baseWindow;

    private Button _btnOff;

    private Button _btnOn;
    private Label _sceneName;

    #region Public Methods

    public override EditorWindowBase ShowWindow()
    {
        titleContent.text = "PlayMode Toggle";

        minSize = new Vector2(200, 120);

        if (!SaveValues.PlayModeStartScene.ApplyPSToggleWindow)
            return this;

        this.CenterOnMainWin(300, 120);
        SaveValues.PlayModeStartScene.ApplyPSToggleWindow = false;

        return this;
    }

    #endregion

    #region Protected Methods

    protected override string BasePath()
    {
        return Constants.PlayModeStartScene.UserInterface.PlayModeToggle;
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

        VisualButtonUpdate(SaveValues.PlayModeStartScene.ToggleState);

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

        SaveValues.PlayModeStartScene.onToggleChanged += VisualButtonUpdate;
        SaveValues.PlayModeStartScene.onStartSceneChanged += UpdateSceneName;
    }

    protected override void UnRegisterCallbacks()
    {
        _btnOn.clickable = null;
        _btnOff.clickable = null;

        SaveValues.PlayModeStartScene.onToggleChanged -= VisualButtonUpdate;
        SaveValues.PlayModeStartScene.onStartSceneChanged -= UpdateSceneName;
    }

    #endregion

    #region Private Methods

    /// <summary> Toggle the behaviour </summary>
    /// <param name="on"> Targeted status </param>
    private void Toggle(bool on)
    {
        VisualButtonUpdate(on);

        SaveValues.PlayModeStartScene.ToggleState = on;
    }

    /// <summary> Update the target scene name </summary>
    private void UpdateSceneName()
    {
        SceneAsset startScene = SaveValues.PlayModeStartScene.GetStartScene();

        if (startScene == null)
        {
            _sceneName.text = "None";

            return;
        }

        _sceneName.text = startScene.name;
        _sceneName.tooltip = AssetDatabase.GUIDToAssetPath(SaveValues.PlayModeStartScene.StartSceneGuid);
    }

    /// <summary> Update the visuals of the buttons </summary>
    /// <param name="on"> State of the behaviour </param>
    private void VisualButtonUpdate(bool on)
    {
        GUIUtility.ToggleActiveButtonInGroup(on ? 0 : 1, _btnOn, _btnOff);
    }

    #endregion
}

}
#endif
