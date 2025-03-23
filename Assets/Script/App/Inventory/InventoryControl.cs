using NativeHelper;
using UnityEngine;
using SFramework.Game;
using ProtoGameData;
using SFramework;

namespace App.Inventory
{
	public class InventoryControl : RootControl
	{
		public static string DELETEUSERDATA = "DELETEUSERDATA";
		public override ViewOpenType GetViewOpenType()
		{
			return ViewOpenType.SingleNeverClose;
		}
		protected override void opening()
		{
			// Code Here
		}
		protected override void closing()
		{
			// Code Here
		}

		public int GetCurLevel()
		{
			InventoryModel model = GetModel<InventoryModel>();
			return model.GetCurLevel();
		}

		public int AddLevel(int value)
		{
			InventoryModel model = GetModel<InventoryModel>();
			int level = model.AddLevel(value);
			model.SaveUserData();
			return level;
		}

		public bool IsNewUser()
		{
			InventoryModel model = GetModel<InventoryModel>();
			return model.IsNewUser();
		}
		
		public ProtoUserData GetProtoUserData()
		{
			InventoryModel model = GetModel<InventoryModel>();
			return model.GetProtoUserData();
		}
		
		public bool AddCoin(long coin)
		{
			InventoryModel model = GetModel<InventoryModel>();
			bool rt = model.AddCoin(coin);
			model.SaveUserData();
			return rt;
		}

		public bool AddDiamond(int diamond)
		{
			InventoryModel model = GetModel<InventoryModel>();
			bool rt = model.AddDiamond(diamond);
			model.SaveUserData();
			return rt;
		}

		public void DeleteUserData()
		{
			InventoryModel model = GetModel<InventoryModel>();
			model.DeleteUserData();
		}

		public void SetArea(int area)
		{
			InventoryModel model = GetModel<InventoryModel>();
			model.SetArea(area);
		}

		public int GetArea()
		{
			InventoryModel model = GetModel<InventoryModel>();
			return model.GetArea();
		}

		public string GetAreaName()
		{
			InventoryModel model = GetModel<InventoryModel>();
			return model.GetAreaName();
		}
		
		public override void HandleMessage(SBundleParams value)
		{
			if (value.MessageId == DELETEUSERDATA)
			{
				(Model as InventoryModel)?.DeleteUserData();
				NativeHelperFactory.Instance.RestartGame();
			}
		}
	}
}
