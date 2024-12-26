using SFramework.Game;
using Cysharp.Threading.Tasks;

namespace App.HudMain
{
	public class HudMainModel : RootModel
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
