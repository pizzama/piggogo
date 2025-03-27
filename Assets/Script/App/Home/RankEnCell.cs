using System.Collections;
using System.Collections.Generic;
using App.Home;
using EnhancedUI.EnhancedScroller;
using GameNet;
using SFramework;
using SFramework.Game;
using UnityEngine;
using UnityEngine.UI;

public class RankEnCell : EnhancedScrollerCellView, ILinker
{
    [SerializeField] private Image _myImage;
    [SerializeField] private Transform _rankTrans1;
    [SerializeField] private Transform _rankTrans2;
    [SerializeField] private Transform _rankTrans3;
    [SerializeField] private Transform _rankTrans4;
    [SerializeField] private Text _rankText;
    [SerializeField] private Text _nameText;
    private HomeMenuView _view;
    private RankTopPlayersData _topPlayersData;
    private RankSingleData _signelData;
    private int _index;
    public void Attache(ISEntity entity)
    {
        if(entity is RootEntity root)
        {
            _view = root.ParentView as HomeMenuView;
        }
    }

    public void DeAttache()
    {
        _view = null;
    }

    public void SetData(RankTopPlayersData data, int index)
    {
        _index = index;
        _topPlayersData = data;
        _signelData = _topPlayersData.top_players[index];
        if (_signelData.role_id == _topPlayersData.current_player.role_id)
        {
            if(_topPlayersData.current_player.rank == 0)
            {
                _rankText.text = "未上榜";
            }
            else
                _rankText.text = _topPlayersData.current_player.rank.ToString();
            _myImage.gameObject.SetActive(true);
        }
        else
        {
            _myImage.gameObject.SetActive(false);
            _rankText.text = (index + 1).ToString();
        }
        _nameText.text = _signelData.role_id;
        refreshRankIcon();
    }

    public void refreshRankIcon()
    {
        switch(_index)
        {
            case 0:
                _rankTrans1.gameObject.SetActive(true);
                _rankTrans2.gameObject.SetActive(false);
                _rankTrans3.gameObject.SetActive(false);
                _rankTrans4.gameObject.SetActive(false);
                _rankText.gameObject.SetActive(false);
                break;
            case 1:
                _rankTrans1.gameObject.SetActive(false);
                _rankTrans2.gameObject.SetActive(true);
                _rankTrans3.gameObject.SetActive(false);
                _rankTrans4.gameObject.SetActive(false);
                _rankText.gameObject.SetActive(false);
                break;  
            case 2:
                _rankTrans1.gameObject.SetActive(false);
                _rankTrans2.gameObject.SetActive(false);
                _rankTrans3.gameObject.SetActive(true);
                _rankTrans4.gameObject.SetActive(false);
                _rankText.gameObject.SetActive(false);
                break;
            default:
                _rankTrans1.gameObject.SetActive(false);
                _rankTrans2.gameObject.SetActive(false);
                _rankTrans3.gameObject.SetActive(false);
                _rankTrans4.gameObject.SetActive(true);
                _rankText.gameObject.SetActive(true);
                break;
        }
    }
}
