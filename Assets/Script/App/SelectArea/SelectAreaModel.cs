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
		private bool _isMustBeSelect = false;
		public bool IsMustBeSelect
		{
			get => _isMustBeSelect;
			set => _isMustBeSelect = value;
		}
		private Levels_Area_datas _allLevelArea;
		public List<Levels_Area> AllLevelArea()
		{
			return _allLevelArea.Datas.ToList();
		}
		protected override void opening()
		{
			readCnofig().Forget();
			if (OpenParams.MessageData != null)
			{
				_isMustBeSelect = (bool)OpenParams.MessageData;
			}
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
