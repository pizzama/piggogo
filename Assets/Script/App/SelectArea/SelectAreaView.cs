using UnityEngine;
using SFramework;
using SFramework.Game;
using UnityEngine.UI;

namespace App.SelectArea
{
	public class SelectAreaView : SUIView
	{
		private Button _closeBtn;
		private SelectAreaEnScroll _enScroll;
		protected override UILayer GetViewLayer()
		{
			return UILayer.Popup;
		}
		protected override void opening()
		{
			// Code Here
			bool rt = GetModel<SelectAreaModel>().IsMustBeSelect;
			_closeBtn = getExportObject<Button>("CloseBtn");
			if (rt == false)
				_closeBtn.onClick.AddListener(CloseHandle);

			_enScroll = getExportObject<SelectAreaEnScroll>("EnScroll");
		}
		protected override void closing()
		{
			// Code Here
			_closeBtn.onClick.RemoveListener(CloseHandle);
		}
		
		public void CloseHandle()
		{
			Control.Close();
		}
	}
}
