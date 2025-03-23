using SFramework;
using SFramework.Game;

namespace App.SelectArea
{
	public class SelectAreaControl : RootControl
	{
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
			base.alreadyOpened();
			// Code Here
		}
		protected override void closing()
		{
			// Code Here
		}
	}
}
