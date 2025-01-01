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
        [SerializeField] private Vector3 handPos;
        private SEventListener<GuideEvent> _listener;
        public override void OnEnter ()
        {
            var flowchart = GetFlowchart();
            var ctl = SBundleManager.Instance.GetControl<GuideControl>();
            _listener = new SEventListener<GuideEvent>(handleEvent, this);
            SFEventManager.AddListener(_listener);
            (ctl.View as GuideView).PointHand(handPos);
        }

        private void handleEvent(ISEventListener<GuideEvent> listener, GuideEvent events)
        {
            var realListener = listener as SEventListener<GuideEvent>;
            SFEventManager.RemoveListener(_listener);
            if(realListener != null && realListener.Src != null)
                (realListener.Src as Command).Continue();
        }
    }
    
    [CommandInfo("Guide", 
        "Guide Move Hand", 
        "新手引导的点击操作")]
    [AddComponentMenu("")]
    public class GuideHandMove: Command
    {
        [SerializeField] private Vector3 startPos;
        [SerializeField] private Vector3 endPos;
        
        public override void OnEnter ()
        {
            var flowchart = GetFlowchart();
            var ctl = SBundleManager.Instance.GetControl<GuideControl>();
            (ctl.View as GuideView).PointHand(startPos);
            Continue();
        }
    }
}