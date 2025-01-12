using System.Collections.Generic;
using Config.ItemsBase;
using Config.LevelsBase;
using Config.LevelsDetail;
using SFramework.Game;
using Cysharp.Threading.Tasks;

namespace App.MainScene
{
	public class MainSceneModel : RootModel
	{
		private Items_Base_datas _allItemsBase;
		private Levels_Base_datas _allLevelBase;
		private Levels_Detail_datas _allLevelDetail;
		private List<Levels_Detail> _curLevelDetails;

		private int _round;
		public int Round {
			get {return _round;}
			set {_round = value;}
		}
		
		public List<Levels_Detail> CurLevelDetails
		{
			get { return _curLevelDetails; }
		}
		
		protected override void opening()
		{
			readConfig().Forget();
		}
		protected override void closing()
		{
			// Code Here
		}
		
		private async UniTask readConfig()
		{
			//读取配置文件
			_allItemsBase = await configManager.GetConfigAsync<Items_Base_datas>();
			_allLevelBase = await configManager.GetConfigAsync<Levels_Base_datas>();
			_allLevelDetail = await configManager.GetConfigAsync<Levels_Detail_datas>();
			RefreshLevel();
			await GetData();
		}

		public void RefreshLevel()
		{
			var ctl = GetControl<MainSceneControl>();
			int level = ctl.RefreshLevel();
			_curLevelDetails = GetCurrentLevelDetailsById(level);
		}

		public Levels_Base GetCurrentLevelById(int levelId)
		{
			Levels_Base value = null;
			_allLevelBase.Datamap.TryGetValue(levelId, out value);
			return value;
		}

		public List<Levels_Detail> GetCurrentLevelDetailsById(int levelId)
		{
			List<Levels_Detail> lds = new List<Levels_Detail>();
			for (int i = 0; i < _allLevelDetail.Datas.Count; i++)
			{
				var ld = _allLevelDetail.Datas[i];
				if (ld.LevelID == levelId)
				{
					lds.Add(ld);
				}
			}

			return lds;
		}

		public Levels_Detail GetLevelDetails(int index)
		{
			for (int i = 0; i < _curLevelDetails.Count; i++)
			{
				var detail = _curLevelDetails[i];
				if (detail.Index == index)
				{
					return detail;
				}
			}

			return null;
		}

		public Items_Base GetItemBaseById(int itemId)
		{
			Items_Base config;
			_allItemsBase.Datamap.TryGetValue(itemId.ToString(), out config);
			return config;
		}

		public void NextRound()
		{
			_round +=1;
		}
		
	}
}
