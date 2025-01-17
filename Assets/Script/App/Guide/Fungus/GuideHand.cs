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
        [SerializeField] private bool _isGuideDisplay;
        [SerializeField] private HandState _handState;
        [SerializeField] private Vector3 _handPos;
        private SEventListener<GuideEvent> _listener;
        public override void OnEnter ()
        {
            var ctl = SBundleManager.Instance.GetControl<GuideControl>();
            var view = (ctl.View as GuideView);
            if (_isGuideDisplay)
            {
                switch(_handState)
                {
                    case HandState.All:
                        view.DisplayHand();
                        view.DisplayMaskHole();
                        _listener = new SEventListener<GuideEvent>(handleEvent, this);
                        SFEventManager.AddListener(_listener);
                        view.PointHand(_handPos);
                    break;
                    case HandState.HoleAndTarget:
                        view.HideHand();
                        view.DisplayMaskHole();
                        _listener = new SEventListener<GuideEvent>(handleEvent, this);
                        SFEventManager.AddListener(_listener);
                        view.PointHand(_handPos);
                    break;
                    case HandState.Hole:
                        view.HideHand();
                        view.DisplayMaskHole();
                        view.PointHole(_handPos);
                        Continue();
                    break;
                    default:
                        view.HideHand();
                        view.HideMaskHole();
                        Continue();
                    break;
                }
            }
            else
            {
                view.HideHand();
                view.HideMaskHole();
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