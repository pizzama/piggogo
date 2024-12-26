using SFramework.Game;
using Cysharp.Threading.Tasks;

namespace App.GameSuccess
{
	public class GameSuccessModel : RootModel
	{
		protected override void opening()
		{
			 GetData().Forget();
		}
		protected override void closing()
		{
			// Code Here
		}
	}
}
