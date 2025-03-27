using System.Collections.Generic;
using App.Home;
using App.Inventory;
using Cysharp.Threading.Tasks;
using GameNet;
using SFramework;
using SFramework.Game;
using SFramework.Statics;

namespace App.SelectArea
{
	public class SelectAreaControl : RootControl
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
		protected override void alreadyOpened()
		{
			base.alreadyOpened();
			// Code Here
		}
		protected override void closing()
		{
			// Code Here
		}

		public override void HandleMessage(SBundleParams value)
		{
			if (value.MessageId == SAVEAREA)
			{
				//保存区域
				int new_category = (int)value.MessageData;
				changeCategory(new_category).Forget();
			}
		}

		private async UniTask changeCategory(int new_category)
		{
			InventoryControl inv = GetControl<InventoryControl>();
			int old_category = inv.GetArea();
			SelectAreaModel model = GetModel<SelectAreaModel>();
			RankTopPlayersNetData rdata = await model.ChangeRankData(old_category, new_category);

			if (rdata != null && rdata.status == 0)
			{
				inv.SetArea(new_category);
				BroadcastControl(HomeMenuControl.REFRESHAREA, rdata.data, SFStaticsControl.App_Home_HomeMenuControl);
			}

		}	
	}
}
