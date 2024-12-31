using SFramework;
using SFramework.Game;

namespace App.Guide
{
	public class GuideControl : RootControl
	{
		public static string StartGuide = "StartGuide";
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
			if (value.MessageId == "")
			{
				
			}
		}
		
	}
}
