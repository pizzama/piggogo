using SFramework.Game;
using Cysharp.Threading.Tasks;

namespace App.Snipe
{
	public class SnipeModel : RootModel
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
