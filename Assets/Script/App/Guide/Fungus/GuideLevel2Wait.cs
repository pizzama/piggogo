using System;
using Fungus;
using SFramework;
using SFramework.Event;
using UnityEngine;
using UnityEngine.UI;

namespace App.Guide.Fungus
{
    [CommandInfo("Guide", 
        "UI Level2 Wait", 
        "新手引导的点击操作")]
    [AddComponentMenu("")]
    public class GuideLevel2Wait: Command
    {
        private SEventListener<GuideEvent> _listener;
        public override void OnEnter()
        {
            _listener = new SEventListener<GuideEvent>(handleEvent, this);
            SFEventManager.AddListener(_listener);
            var ctl = SBundleManager.Instance.GetControl<GuideControl>();
            ctl.HideUIGuideImage();
        }
        
        private void handleEvent(ISEventListener<GuideEvent> listener, GuideEvent events)
        {
            var realListener = listener as SEventListener<GuideEvent>;
            var ctl = SBundleManager.Instance.GetControl<GuideControl>();
            ctl.HideUIGuideImage();
            if (events.Index == 7)
            {
                if (events.state == 0)
                    ctl.DisplayUIGuideImage();
            }
        }
    }
}