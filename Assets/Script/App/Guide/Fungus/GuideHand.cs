using System;
using Fungus;
using SFramework;
using SFramework.Event;
using UnityEngine;

namespace App.Guide.Fungus
{
    [CommandInfo("Guide", 
        "Guide Hand", 
        "新手引导的点击操作")]
    [AddComponentMenu("")]
    public class GuideHand: Command
    {
        [SerializeField] private bool _handIsDisplay;
        [SerializeField] private Vector3 _handPos;
        private SEventListener<GuideEvent> _listener;
        public override void OnEnter ()
        {
            var ctl = SBundleManager.Instance.GetControl<GuideControl>();
            if (_handIsDisplay)
            {
                (ctl.View as GuideView).DisplayHand();
                _listener = new SEventListener<GuideEvent>(handleEvent, this);
                SFEventManager.AddListener(_listener);
                (ctl.View as GuideView).PointHand(_handPos);
            }
            else
            {
                (ctl.View as GuideView).HideHand();
                Continue();
            }

        }

        private void handleEvent(ISEventListener<GuideEvent> listener, GuideEvent events)
        {
            var realListener = listener as SEventListener<GuideEvent>;
            SFEventManager.RemoveListener(_listener);
            if(realListener != null && realListener.Src != null)
                (realListener.Src as Command).Continue();
        }
    }
}