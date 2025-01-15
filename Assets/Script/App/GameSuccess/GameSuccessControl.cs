using App.Inventory;
using App.MainScene;
using SFramework;
using SFramework.Game;
using SFramework.Statics;
using UnityEngine.UI;

namespace App.GameSuccess
{
	public class GameSuccessControl : RootControl
	{
		public static string GAMESUCCESS = "GAMESUCCESS";
		public static string GAMEOVER = "GAMEOVER";
		public override ViewOpenType GetViewOpenType()
		{
			return ViewOpenType.Single;
		}
		protected override void opening()
		{
			// Code Here
		}
		protected override void alreadyOpened()
		{
			// Code Here
		}
		protected override void closing()
		{
			// Code Here
		}

		public void NextLevel()
		{
			//保存数据
			
			//开始下一关
			BroadcastControl(MainSceneControl.NEXTLEVEL, null, SFStaticsControl.App_MainScene_MainSceneControl);
			Close();
		}

		public void ResetLevel()
		{
			//重新当前关
			BroadcastControl(MainSceneControl.CURRENTLEVEL, null, SFStaticsControl.App_MainScene_MainSceneControl);
			Close();
		}
	}
}
