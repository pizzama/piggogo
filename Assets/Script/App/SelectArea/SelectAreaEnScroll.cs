using System.Collections.Generic;
using App.SelectArea;
using Config.LevelsArea;
using EnhancedUI.EnhancedScroller;
using SFramework.Game;
using UnityEngine;

public class SelectAreaEnScroll : RootEntity, IEnhancedScrollerDelegate
{
    [SerializeField] private EnhancedScroller scroller;
    [SerializeField] private EnhancedScrollerCellView cellViewPrefab;

    private List<Levels_Area> _configs;

    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        SelectAreaEnCell cellView = scroller.GetCellView(cellViewPrefab) as SelectAreaEnCell;
        Levels_Area conf = _configs[dataIndex];
        cellView.Attache(this);
        cellView.SetData(conf);
        return cellView;
    }

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        return 120;
    }

    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        return _configs.Count;
    }

    void Start()
    {
        scroller.Delegate = this;
    }

    public override void Recycle()
    {
        
    }

    public override void Show()
    {
        //获取数据
        if (ParentControl != null)
        {
            SelectAreaModel model = ParentControl.GetModel<SelectAreaModel>();
            _configs = model.AllLevelArea();
        }
        float start = scroller.ScrollPosition;
        scroller.ReloadData();
        scroller.SetScrollPositionImmediately(start);
    }
}