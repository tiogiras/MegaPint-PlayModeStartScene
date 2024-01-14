using Editor.Scripts.Windows;
using UnityEngine;
using UnityEngine.UIElements;

namespace com.tiogiras.megapint_playmodestartscene.Editor.Scripts
{

public class PlayModeStartSceneToggle : MegaPintEditorWindowBase
{
    /// <summary> Loaded uxml references </summary>
    private VisualTreeAsset _baseWindow;

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
        return "PlayModeStartScene/User Interface/PlayModeToggle";
    }

    protected override void CreateGUI()
    {
        base.CreateGUI();

        VisualElement root = rootVisualElement;

        VisualElement content = _baseWindow.Instantiate();

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
    }

    protected override void UnRegisterCallbacks()
    {
    }

    #endregion
}

}
