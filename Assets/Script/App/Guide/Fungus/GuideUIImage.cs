using System;
using Fungus;
using SFramework;
using SFramework.Event;
using UnityEngine;
using UnityEngine.UI;

namespace App.Guide.Fungus
{
    [CommandInfo("Guide", 
        "Guide Hand", 
        "新手引导的点击操作")]
    [AddComponentMenu("")]
    public class GuideUIImage: Command
    {
        [SerializeField] private Image guideImage;
        [SerializeField] private String imageName;
        [SerializeField] private Vector3 pos;
        public override void OnEnter ()
        {
            if (string.IsNullOrEmpty(imageName))
                throw new NotFoundException("the name is not setting");
            guideImage.name = imageName;
            var ctl = SBundleManager.Instance.GetControl<GuideControl>();
            (ctl.View as GuideView)?.AddUIImage(guideImage, pos);
        }
        
    }
}