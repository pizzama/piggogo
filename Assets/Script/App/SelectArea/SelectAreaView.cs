using UnityEngine;
using SFramework;
using SFramework.Game;
using UnityEngine.UI;

namespace App.SelectArea
{
	public class SelectAreaView : SUIView
	{
		private Button _closeBtn;
		protected override UILayer GetViewLayer()
		{
			return UILayer.Popup;
		}
		protected override void opening()
		{
			// Code Here
			_closeBtn = getExportObject<Button>("CloseBtn");
			_closeBtn.onClick.AddListener(closeHandle);
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
