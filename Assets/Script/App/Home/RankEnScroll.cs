using EnhancedUI.EnhancedScroller;
using SFramework.Game;
using UnityEngine;

public class RankEnScroll : RootEntity, IEnhancedScrollerDelegate
{
    [SerializeField] private EnhancedScroller scroller;
    [SerializeField] private EnhancedScrollerCellView cellViewPrefab;
    void Start()
    {
        scroller.Delegate = this;
    }
    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        RankEnCell cellView = scroller.GetCellView(cellViewPrefab) as RankEnCell;
        cellView.Attache(this);
        // cellView.SetData(conf);
        return cellView;
    }

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        return 120;
    }

    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        return 10;
    }

    public override void Recycle()
    {
        
    }

    public override void Show()
    {
        
    }
}
