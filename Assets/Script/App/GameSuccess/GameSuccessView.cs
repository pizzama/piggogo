using UnityEngine;
using SFramework;
using SFramework.Game;
using UnityEngine.UI;

namespace App.GameSuccess
{
	public class GameSuccessView : SUIView
	{
		public Button _next;
		private Text _content;
		protected override UILayer GetViewLayer()
		{
			return UILayer.Popup;
		}

		private bool isSuccess = false;
		protected override void opening()
		{
			// Code Here
			_next = getExportObject<Button>("Next");
			_content = getExportObject<Text>("Content");
			_next.onClick.AddListener(nextHandle);
			if((string)Control.Model.OpenParams.MessageData == GameSuccessControl.GAMESUCCESS)
			{
				_content.text = "下一关";
				isSuccess = true;
			}
			else
			{
				_content.text = "重新开始";
				isSuccess = false;
			}
		}
		protected override void closing()
		{
			// Code Here
			_next.onClick.RemoveListener(nextHandle);
		}

		private void nextHandle()
		{
			(Control as GameSuccessControl).NextLevel();
		}
	}
}
