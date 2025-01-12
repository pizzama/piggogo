using System;
using App.MainScene;
using Fungus;
using SFramework;
using SFramework.Event;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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
                if (events.State == 0)
                    ctl.DisplayUIGuideImage();
            }
            else
            {
                if (events.State == 1 && events.Index == 3)
                {
                    SeatBar bar = (events.Something as MainSceneView).GetSeatBarByIndex(6);
                    if (bar != null && bar.IsOperateComplete())
                    {
                        SFEventManager.RemoveListener(listener);
                        ((listener as SEventListener<GuideEvent>)?.Src as Command).Continue();
                    }
                }
            }
        }
    }
}