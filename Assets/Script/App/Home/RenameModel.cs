using SFramework.Game;
using Cysharp.Threading.Tasks;
using GameNet;

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

		public async UniTask RequsetRename(string name)
		{
			RequestRenameData postParams = new RequestRenameData();
			postParams.name = name;
			RenameNetData rd = await PostNetData<RenameNetData>(postParams, null, "/role/update_name");
			GetControl<RenameControl>().SetUserName(rd.data.new_name);
		}
	}
}
