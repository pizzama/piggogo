using Fungus;
using SFramework;
using UnityEngine;

namespace App.Guide.Fungus
{
    [CommandInfo("Guide", 
        "Guide Handle Display", 
        "新手引导的点击操作")]
    [AddComponentMenu("")]
    public class GuideHandDisplay: Command
    {
        [SerializeField] private bool handIsDisplay;
        public override void OnEnter ()
        {
            var ctl = SBundleManager.Instance.GetControl<GuideControl>();
            if (handIsDisplay)
            {
                (ctl.View as GuideView).DisplayHand();
            }
            else
            {
                (ctl.View as GuideView).HideHand();
            }

            Continue();
        }
    }
}