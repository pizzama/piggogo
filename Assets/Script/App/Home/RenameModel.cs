using SFramework.Game;
using Cysharp.Threading.Tasks;

namespace App.Home
{
	public class RenameModel : RootModel
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
