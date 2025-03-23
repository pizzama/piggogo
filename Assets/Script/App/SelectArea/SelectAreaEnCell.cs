using App.SelectArea;
using Config.LevelsArea;
using EnhancedUI.EnhancedScroller;
using SFramework;
using SFramework.Game;
using UnityEngine;
using UnityEngine.UI;

public class SelectAreaEnCell : EnhancedScrollerCellView, ILinker
{
    [SerializeField] private Text _areaName;
    [SerializeField] private Button _okBtn;
    private SelectAreaView _view;
    private Levels_Area _config;

    private void OnEnable()
    {
        _okBtn.onClick.AddListener(okHandler);
    }

    private void OnDisable()
    {
        _okBtn.onClick.RemoveListener(okHandler);
    }

    public void SetData(Levels_Area config)
    {
        _config = config;
        _areaName.text = config.Name;
    }

    private void okHandler()
    {
        _view.SendMessage(SelectAreaControl.SAVEAREA, _config.ID);
        _view.CloseHandle();
    }

    public void Attache(ISEntity entity)
    {
        if(entity is RootEntity root)
        {
            _view = root.ParentView as SelectAreaView;
        }
    }

    public void DeAttache()
    {
        _view = null;
    }
}