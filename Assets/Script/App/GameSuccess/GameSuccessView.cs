using UnityEngine;
using SFramework;
using SFramework.Game;
using UnityEngine.UI;

namespace App.GameSuccess
{
	public class GameSuccessView : SUIView
	{
		public Button _next;
		protected override UILayer GetViewLayer()
		{
			return UILayer.Popup;
		}
		protected override void opening()
		{
			// Code Here
			_next = getExportObject<Button>("Next");
			_next.onClick.AddListener(nextHandle);
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
