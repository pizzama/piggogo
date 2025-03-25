using SFramework.Game;
using Cysharp.Threading.Tasks;
using GameNet;
using UnityEngine;
using System.Collections.Generic;

namespace App.Home
{
	public class HomeMenuModel : RootModel
	{

		protected override void opening()
		{
			GetData().Forget();
			int area = (Control as HomeMenuControl).GetArea();
			requestRankData(area).Forget();
		}

		private async UniTask requestRankData(int area)
		{
			// 调用带有进度参数的GetData方法
			Dictionary<string, string> getParams = new Dictionary<string, string>();
			getParams.Add("category", area.ToString());
			RankTopPlayersNetData rd = await GetNetData<RankTopPlayersNetData>(getParams, null, "/leaderboard/get_top_players");
			Debug.Log("RankData: " + rd.data.category + ";" + rd.data.top_players + ";" + rd.data.current_player);
		}

		protected override void closing()
		{
			// Code Here
		}
	}
}
