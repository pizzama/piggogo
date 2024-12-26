using UnityEngine;
using SFramework;
using SFramework.Game;
using UnityEngine.UI;

namespace App.Tips
{
	public class TipsView : SUIView
	{
		protected override UILayer GetViewLayer()
		{
			return UILayer.Toast;
		}
		protected override void opening()
		{
			// Code Here
		}
		protected override void closing()
		{
			// Code Here
		}
	}
}
