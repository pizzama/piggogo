using System.Collections;
using System.Collections.Generic;
using App.Home;
using EnhancedUI.EnhancedScroller;
using SFramework;
using SFramework.Game;
using UnityEngine;

public class RankEnCell : EnhancedScrollerCellView, ILinker
{
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
