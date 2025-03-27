using SFramework.Game;
using Cysharp.Threading.Tasks;
using GameNet;
using UnityEngine;
using System.Collections.Generic;

namespace App.Home
{
	public class HomeMenuModel : RootModel
	{
		private RankTopPlayersData _topPlayersData;
		public RankTopPlayersData TopPlayersData
		{
			get => _topPlayersData;
			set => _topPlayersData = value;
		}
		protected override void opening()
		{
			initData().Forget();
		}

		private async UniTask initData()
		{
			await GetData();
			int area = (Control as HomeMenuControl).GetArea();
			await RequestRankData(area);
		}

		public async UniTask RequestRankData(int area)
		{
			// 调用带有进度参数的GetData方法
			Dictionary<string, string> getParams = new Dictionary<string, string>();
			getParams.Add("category", area.ToString());
			getParams.Add("count", "5");
			getParams.Add("include_self", "true");
			RankTopPlayersNetData rd = await GetNetData<RankTopPlayersNetData>(getParams, null, "/leaderboard/get_top_players");
			_topPlayersData = rd.data;
		}

		protected override void closing()
		{
			// Code Here
		}
	}
}
