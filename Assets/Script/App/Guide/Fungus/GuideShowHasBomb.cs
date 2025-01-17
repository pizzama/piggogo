using System;
using App.MainScene;
using Fungus;
using SFramework;
using SFramework.Event;
using SFramework.Tools;
using UnityEngine;

namespace App.Guide.Fungus
{
    [CommandInfo("Guide", 
        "Guide Show And Hide Bomb Item", 
        "显示还是关闭新手引导")]
    [AddComponentMenu("")]
    public class GuideShowHasBomb: Command
    {
        [SerializeField] private bool _isDisplay;
        [SerializeField] private int _index;
        public override void OnEnter ()
        {
            var ctl = SBundleManager.Instance.GetControl<GuideControl>();
            var view = ctl.GetView<GuideView>();
            var mainctl = SBundleManager.Instance.GetControl<MainSceneControl>();
            if (_isDisplay)
            {
                var items = mainctl.GetView<MainSceneView>().GetAllBombItem();
                if (items.Count > _index)
                {
                    var parent = view.GetGuideBack();
                    GameObject newInstance = Instantiate(items[_index].gameObject); 
                    ComponentTools.LoadPrefabInRectTransForm(parent, newInstance);
                }
            }
            else
            {
                var items = mainctl.GetView<MainSceneView>().GetAllBombItem();
                if (items.Count > _index)
                    items[_index].ChangeNormal();
            }
            Continue();
        }
    }
}