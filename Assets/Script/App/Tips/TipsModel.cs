using SFramework.Game;
using Cysharp.Threading.Tasks;

namespace App.Tips
{
	public class TipsModel : RootModel
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
