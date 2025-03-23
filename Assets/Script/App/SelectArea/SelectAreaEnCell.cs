using App.SelectArea;
using Config.LevelsArea;
using EnhancedUI.EnhancedScroller;
using UnityEngine;
using UnityEngine.UI;

public class SelectAreaEnCell : EnhancedScrollerCellView
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

    public void SetData(SelectAreaView view, Levels_Area config)
    {
        _view = view;
        _config = config;
        _areaName.text = config.Name;
    }

    private void okHandler()
    {
        _view.SendMessage(SelectAreaControl.SAVEAREA, _config.ID);
        _view.CloseHandle();
    }
}