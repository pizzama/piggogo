using UnityEngine;
using SFramework;
using SFramework.Game;
using UnityEngine.UI;

namespace App.Home
{
	public class HomeView : SSCENEView
	{
		protected override void opening()
		{
			// Code Here
			originCameraSize();
		}
		protected override void closing()
		{
			// Code Here
		}

		private void originCameraSize()
		{
			var _aspect = CameraTools.AdaptCameraSize(10f, 1336f, 768f);
			Debug.Log("camer result size1::" + _aspect);
			UIRoot.MainCamera.orthographicSize = _aspect;
		}
	}
}
