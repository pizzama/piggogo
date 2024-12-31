using Fungus;
using SFramework;
using Spine.Unity;
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
        public override void OnEnter ()
        {
            var flowchart = GetFlowchart();
            var ctl = SBundleManager.Instance.GetControl<GuideControl>();
            (ctl.View as GuideView).PointHand(handPos);
            Continue();
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
            (ctl.View as GuideView).MoveHand(startPos, endPos);
            Continue();
        }
    }
}