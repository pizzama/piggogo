using App.Inventory;
using SFramework;
using SFramework.Game;
using SFramework.Statics;

namespace App.Home
{
	public class HomeControl : RootControl
	{
		public override ViewOpenType GetViewOpenType()
		{
			return ViewOpenType.Single;
		}
		protected override void opening()
		{
			// Code Here
			OpenControl(SFStaticsControl.App_NetLoading_NetLoadingControl);
		}
		protected override void alreadyOpened()
		{
			// Code Here
			CloseControl(SFStaticsControl.App_NetLoading_NetLoadingControl);
			OpenControl(SFStaticsControl.App_Home_HomeMenuControl);
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
	}
}
