using App.Inventory;
using App.MainScene;
using UnityEngine;
using SFramework;
using SFramework.Game;
using SFramework.Statics;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace App.HudMain
{
	public class HudMainView : SUIView
	{
		private Button _fun1;
		private Button _fun2;
		private Button _fun3;
		private Button _fun4;
		protected override UILayer GetViewLayer()
		{
			return UILayer.Hud;
		}
		protected override void opening()
		{
			// Code Here
			_fun1 = getExportObject<Button>("Fun1");
			_fun2 = getExportObject<Button>("Fun2");
			_fun3 = getExportObject<Button>("Fun3");
			_fun4 = getExportObject<Button>("Fun4");
			_fun1.onClick.AddListener(fun1Handle);
			_fun2.onClick.AddListener(fun2Handle);
			_fun3.onClick.AddListener(fun3Handle);
			_fun4.onClick.AddListener(fun4Handle);
		}
		protected override void closing()
		{
			// Code Here
			_fun1.onClick.RemoveListener(fun1Handle);
			_fun2.onClick.RemoveListener(fun2Handle);
			_fun3.onClick.RemoveListener(fun3Handle);
			_fun4.onClick.RemoveListener(fun4Handle);
		}

		private void fun1Handle()
		{
			Control.OpenControl(SFStaticsControl.App_Snipe_SnipeControl);
		}
		
		private void fun2Handle()
		{
			Control.BroadcastControl(MainSceneControl.ADDSEATBAR, null,
				SFStaticsControl.App_MainScene_MainSceneControl);
		}
		
		private void fun3Handle()
		{
			Control.BroadcastControl(MainSceneControl.RANDOMSEATBARITEM, null,
				SFStaticsControl.App_MainScene_MainSceneControl);
		}

		private void fun4Handle()
		{
			Control.BroadcastControl(InventoryControl.DELETEUSERDATA, null, SFStaticsControl.App_Inventory_InventoryControl);
		}
	}
}
