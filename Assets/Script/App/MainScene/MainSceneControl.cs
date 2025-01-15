using System.Collections.Generic;
using App.Guide;
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
		public static string NEXTLEVEL = "NEXTLEVEL";
		public static string CURRENTLEVEL = "CURRENTLEVEL";
		
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

		public int NextLevel()
		{
			InventoryControl inv = GetControl<InventoryControl>();
			int level = inv.AddLevel(1);
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
			OpenControl(SFStaticsControl.App_Guide_GuideControl, null, false, "", 0, (object value) =>
			{
				// 触发当前关卡是否有新手引导
				int level = RefreshLevel();
				BroadcastControl(GuideControl.StartGuide, level, SFStaticsControl.App_Guide_GuideControl);
			});
		}
		protected override void closing()
		{
			// Code Here
		}
		
		public override void HandleMessage(SBundleParams value)
		{
			if (value.MessageId == ADDSEATBAR)
			{
				Transform tran = (View as MainSceneView)?.HasVisibleSeatBar();
				if(tran != null)
					tran.gameObject.SetActive(true);
			}
			else if (value.MessageId == RANDOMSEATBARITEM)
			{
				(View as MainSceneView)?.RandomSeatBarItem();
			}
			else if (value.MessageId == NEXTLEVEL)
			{ 
				BroadcastControl(GuideControl.CloseGuide, null,SFStaticsControl.App_Guide_GuideControl);
				int level = NextLevel();
				(Model as MainSceneModel)?.RefreshLevel();
				(View as MainSceneView)?.DealWithBranch();
				BroadcastControl(GuideControl.StartGuide, level, SFStaticsControl.App_Guide_GuideControl);
			}
			else if (value.MessageId == CURRENTLEVEL)
			{
				BroadcastControl(GuideControl.CloseGuide, null,SFStaticsControl.App_Guide_GuideControl);
				int level = RefreshLevel();
				(Model as MainSceneModel)?.RefreshLevel();
				(View as MainSceneView)?.DealWithBranch();
				BroadcastControl(GuideControl.StartGuide, level, SFStaticsControl.App_Guide_GuideControl);
			}
		}
	}
}
