using App.Inventory;
using SFramework;
using SFramework.Game;
using SFramework.Statics;

namespace App.Home
{
	public class HomeMenuControl : RootControl
	{
		public static string SAVEAREA = "SAVEAREA";
		public override ViewOpenType GetViewOpenType()
		{
			return ViewOpenType.Single;
		}
		protected override void opening()
		{
			// Code Here
		}

		public bool HasArea()
		{
			InventoryControl inv = GetControl<InventoryControl>();
			if (inv.GetArea() > 0)
			{
				return true;	
			}
			else
			{
				return false;
			}
		}

		protected override void alreadyOpened()
		{
			// Code Here
			if(HasArea() == false)
			{
				// 弹出选择
				OpenControl(SFStaticsControl.App_SelectArea_SelectAreaControl, true);	
			}
		}
		protected override void closing()
		{
			// Code Here
		}

		public override void HandleMessage(SBundleParams value)
		{
			if (value.MessageId == SAVEAREA)
			{
				
			}
		}
	}
}
