using System.Collections;
using System.Collections.Generic;
using App.Home;
using EnhancedUI.EnhancedScroller;
using SFramework;
using SFramework.Game;
using UnityEngine;
using UnityEngine.UI;

public class RankEnCell : EnhancedScrollerCellView, ILinker
{
    [SerializeField] private Image _myImage;
    [SerializeField] private Transform _rankTrans;
    [SerializeField] private Text _rankText;
    [SerializeField] private Text _nameText;
    private HomeMenuView _view;
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
}
