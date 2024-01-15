#if UNITY_EDITOR
using Editor.Scripts.Windows;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Scripts
{

public class PlayModeStartSceneToggle : MegaPintEditorWindowBase
{
    /// <summary> Loaded uxml references </summary>
    private VisualTreeAsset _baseWindow;

    private static readonly Color s_playModeStartSceneOnColor = new(.8196078431372549f, 0f, .4470588235294118f);
    private static readonly Color s_playModeStartSceneOffColor = new(.34f, .34f, .34f);
    
    private Button _btnOn;
    private Button _btnOff;
    private Label _sceneName;

    #region Public Methods

    public override MegaPintEditorWindowBase ShowWindow()
    {
        titleContent.text = "PlayMode Toggle";

        minSize = new Vector2(200, 30);
        maxSize = new Vector2(200, 30);

        return this;
    }

    #endregion

    #region Protected Methods

    protected override string BasePath()
    {
        return "PlayModeStartScene/User Interface/PlayModeToggle";
    }

    protected override void CreateGUI()
    {
        base.CreateGUI();

        VisualElement root = rootVisualElement;

        VisualElement content = _baseWindow.Instantiate();

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

    private void UpdateSceneName()
    {
        _sceneName.text = PlayModeStartSceneData.GetStartScene().name;
        _sceneName.tooltip = AssetDatabase.GUIDToAssetPath(PlayModeStartSceneData.StartSceneGuid);
    }

    private void VisualButtonUpdate(bool on)
    {
        _btnOn.style.backgroundColor = on ? s_playModeStartSceneOnColor : s_playModeStartSceneOffColor;
        _btnOff.style.backgroundColor = on ? s_playModeStartSceneOffColor : s_playModeStartSceneOnColor;
    }
    
    private void Toggle(bool on)
    {
        VisualButtonUpdate(on);
        
        PlayModeStartSceneData.ToggleState = on;

        EditorSceneManager.playModeStartScene = on ? PlayModeStartSceneData.GetStartScene() : null;
    }

    #endregion
}

}
#endif
