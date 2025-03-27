using App.Inventory;
using Cysharp.Threading.Tasks;
using GameNet;
using SFramework;
using SFramework.Game;
using SFramework.Statics;

namespace App.Home
{
	public class HomeMenuControl : RootControl
	{
		public static string REFRESHAREA = "REFRESHAREA";
		public static string REFRESHNAME = "REFRESHNAME";
		public override ViewOpenType GetViewOpenType()
		{
			return ViewOpenType.Single;
		}
		protected override void opening()
		{
			// Code Here
		}

		public int GetArea()
		{
			InventoryControl inv = GetControl<InventoryControl>();
			return inv.GetArea();
		}

		public string GetUserName()
		{
			InventoryControl inv = GetControl<InventoryControl>();
			return inv.GetUserName();
		}

		public string GetRoleId()
		{
			InventoryControl inv = GetControl<InventoryControl>();
			return inv.GetRoleId();
		}

		protected override void alreadyOpened()
		{
			// Code Here
			if(GetArea() == 0)
			{
				// 弹出选择
				OpenControl(SFStaticsControl.App_SelectArea_SelectAreaControl, true);	
			}

			RefreshAreaName();
		}
		protected override void closing()
		{
			// Code Here
		}

		public override void HandleMessage(SBundleParams value)
		{
			if (value.MessageId == REFRESHAREA)
			{
				HomeMenuModel model = GetModel<HomeMenuModel>();
				model.TopPlayersData = value.MessageData as RankTopPlayersData;
				RefreshAreaName();
			}
			else if (value.MessageId == REFRESHNAME)
			{
				RefreshAreaName();
			}
		}

		public void RefreshAreaName()
		{
			InventoryControl inv = GetControl<InventoryControl>();
			string area = inv.GetAreaName();
			HomeMenuView view = GetView<HomeMenuView>();
			view.RefreshArea(area);
		}
		
	}
}
