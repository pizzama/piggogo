using SFramework.Game;
using Cysharp.Threading.Tasks;

namespace App.NetLoading
{
	public class NetLoadingModel : RootModel
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
