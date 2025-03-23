using UnityEngine;
using SFramework;
using SFramework.Game;
using UnityEngine.UI;
using SFramework.Statics;
using App.MainScene;

namespace App.Home
{
	public class HomeMenuView : SUIView
	{
		private Button _specialBtn;
		private Button _levelBtn;
		private Button _areaBtn;
		private Text _areaName;

		protected override UILayer GetViewLayer()
		{
			return UILayer.Popup;
		}
		protected override void opening()
		{
			// Code Here
			_specialBtn = getExportObject<Button>("SpecialBtn");
			_levelBtn = getExportObject<Button>("LevelBtn");
			_areaBtn = getExportObject<Button>("ProvinceBtn");

			_specialBtn.onClick.AddListener(specialHandle);
			_levelBtn.onClick.AddListener(levelHandle);
			_areaBtn.onClick.AddListener(areaHandle);

			_areaName = getExportObject<Text>("AreaName");
		}
		protected override void closing()
		{
			// Code Here
			_specialBtn.onClick.RemoveListener(specialHandle);
			_levelBtn.onClick.RemoveListener(levelHandle);
		}

		private void specialHandle()
		{
			Control.OpenControl(SFStaticsControl.App_MainScene_MainSceneControl, MainSceneControl.SPECIALLEVEL);
		}

		private void levelHandle()
		{
			Control.OpenControl(SFStaticsControl.App_MainScene_MainSceneControl, MainSceneControl.NORMALLEVEL);
		}

		private void areaHandle()
		{
			Control.OpenControl(SFStaticsControl.App_SelectArea_SelectAreaControl);
		}

		public void RefreshArea(string value)
		{
			_areaName.text = value;
		}
	}
}
