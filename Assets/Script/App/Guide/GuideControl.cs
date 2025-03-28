using Cysharp.Threading.Tasks;
using SFramework;
using SFramework.Event;
using SFramework.Game;

namespace App.Guide
{
	public class GuideControl : RootControl
	{
		public static string StartGuide = "StartGuide";
		public static string NextGuide = "NextGuide";
		public static string CloseGuide = "CloseGuide";
		public override ViewOpenType GetViewOpenType()
		{
			return ViewOpenType.Single;
		}
		protected override void opening()
		{
			// Code Here
		}
		protected override void alreadyOpened()
		{
			// Code Here
		}
		protected override void closing()
		{
			// Code Here
		}

		public override void HandleMessage(SBundleParams value)
		{
			if (value.MessageId == StartGuide)
			{
				int level = (int)value.MessageData;
				(View as GuideView)?.Play(level).Forget();
			}
			else if (value.MessageId == NextGuide)
			{
				SFEventManager.TriggerEvent(new GuideEvent());
			}
			else if (value.MessageId == CloseGuide)
			{
				(View as GuideView)?.HideGuide();
				(View as GuideView)?.CloseGuildeElement();
			}
		}

		public void DisplayGuideFront()
		{
			(View as GuideView)?.DisplayGuideFront();
		}

		public void HideGuideFront()
		{
			(View as GuideView)?.HideGuideFront();
		}

		public void DisplayGuideBack()
		{
			(View as GuideView)?.DisplayGuideBack();
		}

		public void HideGuideBack()
		{
			(View as GuideView)?.HideGuideBack();
		}

		public void DisplayGuideMiddle()
		{
			(View as GuideView)?.DisplayGuideMiddle();
		}

		public void HideGuideMiddle()
		{
			(View as GuideView)?.HideGuideMiddle();
		}

	}
}
