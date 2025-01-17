using System;
using Fungus;
using SFramework;
using SFramework.Event;
using UnityEngine;
using UnityEngine.UI;

namespace App.Guide.Fungus
{
    [CommandInfo("Guide", 
        "UI Delete Image", 
        "新手引导的点击操作")]
    [AddComponentMenu("")]
    public class GuideDeleteUIImage: Command
    {
        [SerializeField] private GuideLayer layer;
        [SerializeField] private Image guideImage;
        [SerializeField] private string imageName;
        public override void OnEnter ()
        {
            var ctl = SBundleManager.Instance.GetControl<GuideControl>();
            switch (layer)
            {
                case GuideLayer.GuideFrontLayer:
                    (ctl.View as GuideView)?.RemoveFrontByName(imageName);
                    break;
                case GuideLayer.GuideMiddelLayer:
                    (ctl.View as GuideView)?.RemoveMiddelByName(imageName);
                    break;
                case GuideLayer.GuideBackLayer:
                    (ctl.View as GuideView)?.RemoveBackByName(imageName);
                    break;
            }

            Continue();
        }
        
    }
}