using System.Collections.Generic;
using App.Inventory;
using Config.ItemsBase;
using Config.LevelsDetail;
using SFramework;
using SFramework.Game;
using SFramework.Statics;
using UnityEngine;

namespace App.MainScene
{
	public class MainSceneControl : RootControl
	{
		public static string ADDSEATBAR = "ADDSEATBAR";
		public static string RANDOMSEATBARITEM = "RandomSeatBarItem";
		
		public override ViewOpenType GetViewOpenType()
		{
			return ViewOpenType.Single;
		}

		public int RefreshLevel()
		{
			InventoryControl inv = GetControl<InventoryControl>();
			int level = inv.GetCurLevel();
			return level;
		}
		protected override void opening()
		{
			OpenControl(SFStaticsControl.App_NetLoading_NetLoadingControl);
		}
		protected override void alreadyOpened()
		{
			// Code Here
			// 打开hudmain得界面
			CloseControl(SFStaticsControl.App_NetLoading_NetLoadingControl);
			OpenControl(SFStaticsControl.App_HudMain_HudMainControl);
		}
		protected override void closing()
		{
			// Code Here
		}
		
		public override void HandleMessage(SBundleParams value)
		{
			if (value.MessageId == ADDSEATBAR)
			{
				var view = GetView<MainSceneView>();
				Transform tran = view.HasVisibleSeatBar();
				if(tran != null)
					tran.gameObject.SetActive(true);
			}
			else if (value.MessageId == RANDOMSEATBARITEM)
			{
				var view = GetView<MainSceneView>();
				view.RandomSeatBarItem();
			}
		}
	}
}
