using App.SelectArea;
using Config.LevelsArea;
using EnhancedUI.EnhancedScroller;
using UnityEngine;
using UnityEngine.UI;

public class SelectAreaEnCell : EnhancedScrollerCellView
{
    [SerializeField] private Text _areaName;
    private SelectAreaView _view;
    private Levels_Area _config;
    public void SetData(SelectAreaView view, Levels_Area config)
    {
        _view = view;
        _config = config;

        _areaName.text = config.Name;
    }
}