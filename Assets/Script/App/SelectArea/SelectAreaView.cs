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
			_closeBtn = getExportObject<Button>("CloseBtn");
			_closeBtn.onClick.AddListener(closeHandle);

			_enScroll = getExportObject<SelectAreaEnScroll>("EnScroll");
			
		}
		protected override void closing()
		{
			// Code Here
			_closeBtn.onClick.RemoveListener(closeHandle);
		}
		
		private void closeHandle()
		{
			Control.Close();
		}
	}
}
