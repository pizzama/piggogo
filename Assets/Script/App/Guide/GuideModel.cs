using SFramework.Game;
using Cysharp.Threading.Tasks;

namespace App.Guide
{
	public class GuideModel : RootModel
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
