using SFramework.Game;
using Cysharp.Threading.Tasks;
using Config.LevelsArea;
using SFramework;
using System.Collections.Generic;
using System.Linq;

namespace App.SelectArea
{
	public class SelectAreaModel : RootModel
	{
		private Levels_Area_datas _allLevelArea;
		public List<Levels_Area> AllLevelArea()
		{
			return _allLevelArea.Datas.ToList();
		}
		protected override void opening()
		{
			readCnofig().Forget();
		}
		protected override void closing()
		{
			// Code Here
		}

		private async UniTask readCnofig()
		{
			_allLevelArea = await ConfigManager.Instance.GetConfigAsync<Levels_Area_datas>();
			await GetData();
		}
	}
}
