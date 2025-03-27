using SFramework.Game;
using Cysharp.Threading.Tasks;
using Config.LevelsArea;
using SFramework;
using System.Collections.Generic;
using System.Linq;
using GameNet;

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

		public async UniTask<RankTopPlayersNetData> ChangeRankData(int old_category, int new_category)
		{
			// 调用带有进度参数的GetData方法
			RequestChangeRank postParams = new RequestChangeRank()
			{
				old_category = old_category.ToString(),
				new_category = new_category.ToString()
			};
			Dictionary<string, string> getParams = new Dictionary<string, string>();
			getParams.Add("count", "5");
			getParams.Add("include_self", "true");
			RankTopPlayersNetData rd = await PostNetData<RankTopPlayersNetData>(postParams, getParams, null, "/leaderboard/change_category");
			return rd;
		}
	}
}
