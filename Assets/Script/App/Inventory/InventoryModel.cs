using SFramework.Game;
using Cysharp.Threading.Tasks;
using ProtoGameData;
using UnityEngine;
using System.Collections.Generic;
using Config.LevelsBase;

namespace App.Inventory
{
	public class InventoryModel : RootModel
	{
		public const int MaxFishTime = 120;
		private bool _isNewUser;
		private Levels_Base_datas _allLevelBase;
		
		private ProtoUserData _userData;
		private int _maxLevel = 1;

		public int MaxLevel
		{
			get { return _maxLevel; }
		}
		public ProtoUserData GetProtoUserData()
		{
			return _userData;
		}
		protected override void opening()
		{
			ReadUserData().Forget();
		}

		public bool IsNewUser()
		{
			return _isNewUser;
		}

		public async UniTask ReadUserData()
		{
			//读取配置文件
			_allLevelBase = await configManager.GetConfigAsync<Levels_Base_datas>();
			if (_allLevelBase.Datas.Count > 0)
			{
				var data = _allLevelBase.Datas[_allLevelBase.Datas.Count - 1];
				_maxLevel = data.ID;
			}
			_isNewUser = false;
			if (_userData == null)
			{
				_userData = ReadData<ProtoUserData>();
			}

			if (_userData == null)
			{
				_userData = new ProtoUserData();
				//初始化背包
				initInventory();
			}

			if (_userData.Level == 0)
			{
				_userData.Level = 1;
			}

			await GetData();
		}

		public void SaveUserData()
		{
			SaveData<ProtoUserData>(_userData);
		}

		public void DeleteUserData()
		{
			Delete<ProtoUserData>();
		}

		private void initInventory()
		{
			_userData.Level = 1; //初始化的等级是1级
			_isNewUser = true;
		}
		protected override void closing()
		{
			// Code Here
		}

		public int GetCurLevel()
		{
			return (int)_userData.Level;
		}

		public int AddLevel(int value)
		{
			int level = (int)_userData.Level + value;
			if (level > _maxLevel)
				level = _maxLevel;
			_userData.Level = (uint)level;
			return level;
		}

		public bool AddDiamond(int diamond)
		{
			int rt = _userData.Diamond + diamond;
			if (diamond < 0)
			{
				if (rt < 0)
				{
					return false;
				}
			}

			_userData.Diamond = rt;
			return true;
		}


		public bool AddCoin(long coin)
		{
			long rt = _userData.Coin + coin;
			if (coin < 0)
			{

				if (rt < 0)
				{
					Debug.Log("金币不足");
					return false;
				}
			}
			_userData.Coin = rt;
			return true;
		}
	}
}
