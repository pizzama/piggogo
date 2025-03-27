using App.Home;
using EnhancedUI.EnhancedScroller;
using GameNet;
using SFramework.Game;
using UnityEngine;

public class RankEnScroll : RootEntity, IEnhancedScrollerDelegate
{
    [SerializeField] private EnhancedScroller scroller;
    [SerializeField] private EnhancedScrollerCellView cellViewPrefab;
    private RankTopPlayersData _topPlayersData;
    void Start()
    {
        scroller.Delegate = this;
    }

    public void SetData(RankTopPlayersData data)
    {
        _topPlayersData = data;
    }
    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        RankEnCell cellView = scroller.GetCellView(cellViewPrefab) as RankEnCell;
        cellView.Attache(this);
        cellView.SetData(_topPlayersData, dataIndex);
        return cellView;
    }

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        return 120;
    }

    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        if (_topPlayersData == null)
        {
            return 0;
        }
        else
        {
            return _topPlayersData.top_players.Count;
        }
        
    }

    public override void Recycle()
    {
        
    }

    public override void Show()
    {
        //获取数据
        if (ParentControl != null)
        {
            HomeMenuModel model = ParentControl.GetModel<HomeMenuModel>();
            _topPlayersData = model.TopPlayersData;
        }
        float start = scroller.ScrollPosition;
        scroller.ReloadData();
        scroller.SetScrollPositionImmediately(start);
    }
}
