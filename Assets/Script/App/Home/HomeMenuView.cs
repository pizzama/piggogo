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
		private RankEnScroll _rankEnScroll;
		private Text _icon_text1;
		private Button _iconbg;

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
			_iconbg = getExportObject<Button>("iconbg");

			_specialBtn.onClick.AddListener(specialHandle);
			_levelBtn.onClick.AddListener(levelHandle);
			_areaBtn.onClick.AddListener(areaHandle);
			_iconbg.onClick.AddListener(renameHandle);

			_areaName = getExportObject<Text>("AreaName");
			_rankEnScroll = getExportObject<RankEnScroll>("RankEnScroll");

			_icon_text1 = getExportObject<Text>("icon_text1");
		}
		protected override void closing()
		{
			// Code Here
			_specialBtn.onClick.RemoveListener(specialHandle);
			_levelBtn.onClick.RemoveListener(levelHandle);
			_areaBtn.onClick.RemoveListener(areaHandle);
			_iconbg.onClick.RemoveListener(renameHandle);
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

		private void renameHandle()
		{
			Control.OpenControl(SFStaticsControl.App_Home_RenameControl);
		}

		public void RefreshArea(string value)
		{
			_areaName.text = value;
			_icon_text1.text = GetControl<HomeMenuControl>().GetUserName();
			_rankEnScroll.Show();
		}
	}
}
