using Fungus;
using SFramework;
using SFramework.Event;
using UnityEngine;

namespace App.Guide.Fungus
{
    [CommandInfo("Guide", 
        "Guide Content", 
        "新手引导的文字提示")]
    [AddComponentMenu("")]
    public class GuideContent: Command
    {
        [SerializeField] private string language;
        [SerializeField] private bool needClickCallback = false;
        private SEventListener<GuideEvent> _listener;
        public override void OnEnter ()
        {
            var ctl = SBundleManager.Instance.GetControl<GuideControl>();
            if (ctl != null)
            {
                if (needClickCallback)
                {
                    _listener = new SEventListener<GuideEvent>(handleEvent, this);
                    SFEventManager.AddListener(_listener);
                    (ctl.View as GuideView).RefreshContent(language, needClickCallback);
                }
                else
                {
                    (ctl.View as GuideView).RefreshContent(language, needClickCallback);
                    Continue();
                }
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