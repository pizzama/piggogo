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
        [SerializeField] private Image guideImage;
        [SerializeField] private String imageName;
        public override void OnEnter ()
        {
            var ctl = SBundleManager.Instance.GetControl<GuideControl>();
            (ctl.View as GuideView)?.RemoveUIImageByName(imageName);
            Continue();
        }
        
    }
}