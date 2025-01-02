using App.Inventory;
using App.MainScene;
using SFramework;
using SFramework.Game;

namespace App.GameSuccess
{
	public class GameSuccessControl : RootControl
	{
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
			BroadcastControl(MainSceneControl.NEXTLEVEL);
		}
	}
}
