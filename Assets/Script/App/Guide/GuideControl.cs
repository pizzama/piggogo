using SFramework;
using SFramework.Game;

namespace App.Guide
{
	public class GuideControl : RootControl
	{
		public static string StartGuide = "StartGuide";
		public static string NextGuide = "NextGuide";
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
				(View as GuideView).Play(level);
			}
			else if(value.MessageId == NextGuide)
			{
				(View as GuideView).PlayNext();
			}
		}
		
	}
}
