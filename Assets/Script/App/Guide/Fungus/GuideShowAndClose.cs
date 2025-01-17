using System;
using Fungus;
using SFramework;
using SFramework.Event;
using UnityEngine;

namespace App.Guide.Fungus
{
    [CommandInfo("Guide", 
        "Guide Show And Hide", 
        "显示还是关闭新手引导")]
    [AddComponentMenu("")]
    public class GuideShowAndClose: Command
    {
        [SerializeField] private bool _isDisplay;
        public override void OnEnter ()
        {
            var ctl = SBundleManager.Instance.GetControl<GuideControl>();
            if (_isDisplay)
            {
                (ctl.View as GuideView).DisplayGuide();
            }
            else
            {
                (ctl.View as GuideView).HideGuide();
            }
            Continue();
        }
    }
}