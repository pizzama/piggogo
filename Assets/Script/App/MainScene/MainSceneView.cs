using System.Collections.Generic;
using App.Guide;
using Config.ItemsBase;
using Cysharp.Threading.Tasks;
using SFramework.Event;
using UnityEngine;
using SFramework.Extension;
using SFramework.Game;
using SFramework.GameCamera;
using SFramework.Statics;
using App.GameSuccess;
using Config.LevelsBase;

namespace App.MainScene
{
	public class MainSceneView : SSCENEView
	{
		private float[] posY1 = { 4.8f, 3f, 1.2f, -0.6f, -2.4f, -4.2f, -6f }; //根据配置计算是否偏移
		private float[] posY2 = { 4.36f, 2.56f, 0.76f, -1.04f, -2.84f, -5.94f, -6.44f};
		private Transform _branchs;
		private Transform _itemsContainer;
		private MainSceneModel _model;
		
		private SInputEvent _event;

		public SInputEvent InputEvent
		{
			get { return _event; }
			set { _event = value; }
		}
		private Ray _ray;

		private SeatBar _bar;
		
		protected override void opening()
		{
			// Code Here
			_model = GetModel<MainSceneModel>();
			_branchs = getExportObject<Transform>("Branchs");
			_itemsContainer = getExportObject<Transform>("ItemsContainer");

			_event = getExportObject<SInputEvent>("ItemEvent");
			_event.MouseEventHandle = mouseHandle;
			DealWithBranch().Forget();
		}
		
		public async UniTask DealWithBranch()
		{
			Levels_Base lb = GetModel<MainSceneModel>().CurLevelBase;
			float[] pos = posY1;
			if (lb.Floor == 1)
				pos = posY2;
			int index = 0;
			for (int i = 0; i < _branchs.childCount; i++)
			{
				var bh = _branchs.GetChild(i);
				bh.gameObject.SetActive(false);
				SeatBar bar = bh.GetComponent<SeatBar>();
				Vector3 vec = bar.transform.localPosition;
				if (i % 2 != 0 )
				{
					vec.y = pos[index];	
					index += 1;
					bar.transform.localPosition = vec;
				}
				
				bar.Recycle(); //回收之前的数据
				bar.SetEntityData((i + 1).ToString(), this);
				bar.SetIndex(i + 1);
				bar.Show();
				for (int j = 0; j < _model.CurLevelDetails.Count; j++)
				{
					var detail = _model.CurLevelDetails[j];
					if (bh.name == "Branch" + detail.Index)
					{
						await bar.SetData(detail);
						break;
					}
				}
			}

			// SendMessage(MainSceneControl.GUIDESTART);
		}

		public async UniTask<Item> CreateItem(int itemId, Vector3 pos, bool isleft)
		{
			Items_Base config = _model.GetItemBaseById(itemId);
			Debug.Log("create item:" + itemId);
			Item it = await CreateEntityAsync<Item>(SFResAssets.App_mainscene_sfp_Pig_prefab, _itemsContainer);
			it.transform.position = pos;
			await it.LoadSpine(config, isleft);
			return it;
		}

		public bool IsAllComplete()
		{
			int count = 0;
			int allCount = _branchs.childCount;
			for (int i = 0; i < allCount; i++)
			{
				var child = _branchs.GetChild(i);
				SeatBar bar = child.GetComponent<SeatBar>();
				if (bar.IsSeatBarComplete())
				{
					count += 1;
				}
			}

			if (count == allCount)
				return true;
			else
				return false;
		}

		public Transform HasVisibleSeatBar()
		{
			for (int i = 0; i < _branchs.childCount; i++)
			{
				var bh = _branchs.GetChild(i);
				if (bh.gameObject.activeSelf == false)
				{
					return bh;
				}
			}

			return null;
		}

		public void RandomSeatBarItem()
		{
			//获得所有剩余的items
			List<Item> allItems = new List<Item>();
			for (int i = 0; i < _branchs.childCount; i++)
			{
				var bh = _branchs.GetChild(i);
				SeatBar bar = bh.GetComponent<SeatBar>();
				allItems.AddRange(bar.GetItems());
			}

			refreshCard(allItems).Forget();
		}

		public SeatBar GetSeatBarByIndex(int index)
		{
			for (int i = 0; i < _branchs.childCount; i++)
			{
				var bh = _branchs.GetChild(i);
				SeatBar bar = bh.GetComponent<SeatBar>();
				if (bar.EntityId == index.ToString())
				{
					return bar;
				}
			}

			return null;
		}

		public void GameSuccess()
		{
			Control.OpenControl(SFStaticsControl.App_GameSuccess_GameSuccessControl, GameSuccessControl.GAMESUCCESS);
		}

		public void GameOver()
		{
			Control.OpenControl(SFStaticsControl.App_GameSuccess_GameSuccessControl, GameSuccessControl.GAMEOVER);
		}

		public List<Item> GetAllBombItem()
		{
			List<Item> result = new List<Item>();
			int allCount = _itemsContainer.childCount;
			for (int i = 0; i < allCount; i++)
			{
				var child = _itemsContainer.GetChild(i);
				Item it = child.GetComponent<Item>();
				if (it.HasBomb())
				{
					result.Add(it);
				}
			}

			return result;
		}

		public void UnLockItem()
		{
			List<Item> result = new List<Item>();
			int allCount = _itemsContainer.childCount;
			for (int i = 0; i < allCount; i++)
			{
				var child = _itemsContainer.GetChild(i);
				Item it = child.GetComponent<Item>();
				it.UnLock();
			}
		}

		private async UniTask refreshCard(List<Item> allItems)
		{
			//锁定屏幕都移动完了之后在解开屏幕
			_event.IsLocked = true;
			//洗牌
			allItems.ShuffleList();
			int idx = 0;
			for (int i = 0; i < _branchs.childCount; i++)
			{
				var bh = _branchs.GetChild(i);
				SeatBar bar = bh.GetComponent<SeatBar>();
				List<Item> its = bar.GetItems();
				List<Item> subs = allItems.GetRange(idx, its.Count);
				idx += its.Count;
				bar.SetItems(subs);
			}

			await UniTask.Delay(1500);
			_event.IsLocked = false;

		}
		
		protected override void closing()
		{
			// Code Here
		}

		private void nextRound(SeatBar oldbar, SeatBar newbar)
		{
			// 触发下一轮逻辑
			for (int i = 0; i < _branchs.childCount; i++)
			{
				var bh = _branchs.GetChild(i);
				SeatBar bar = bh.GetComponent<SeatBar>();
				bar.NextRound(oldbar, newbar);
			}
		}

		private void fixPos()
		{
			// 触发下一轮逻辑
			for (int i = 0; i < _branchs.childCount; i++)
			{
				var bh = _branchs.GetChild(i);
				SeatBar bar = bh.GetComponent<SeatBar>();
				bar.FixPos();
			}
		}

		private void mouseHandle(bool isTouchUI, SInputEventType enumInputEventType, Vector3 mousePosition,
			int clickCount, int keyCode)
		{
			if (enumInputEventType == SInputEventType.Click)
			{
				if (isTouchUI == false)
				{
					//发送射线做碰撞检测, 需要确认当前的摄像机是哪一个才能判断射线是否正确
					_ray = UIRoot.MainCamera.ScreenPointToRay(Input.mousePosition);
					
					int mask = LayerMask.GetMask("Default");
					RaycastHit2D[] hits = Physics2D.RaycastAll(new Vector2(_ray.origin.x, _ray.origin.y), Vector2.zero, Mathf.Infinity, mask);
					foreach (RaycastHit2D hit in hits)
					{
						if (hit.collider.isTrigger == false)
							continue;
						if (hit.collider != null)
						{
							SeatBar entity = hit.collider.GetComponent<SeatBar>();
							if (entity != null)
							{
								if (_bar == null)
								{
									if (entity.IsLock)
										return;
									_bar = entity;
									_bar.Select();
									GuideEvent gevt = new GuideEvent();
									gevt.Index = _bar.Index;
									gevt.State = 0;
									gevt.Something = this;
									// 广播新手引导
									SFEventManager.TriggerEvent(gevt);
								}
								else
								{
									bool rt = entity.Merge(_bar); //不需要判断是否可以合成都可以改变状态
									if(rt)
									{
										nextRound(_bar, entity);
									}
									_bar.Idle();
									// 广播新手引导
									GuideEvent gevt = new GuideEvent();
									gevt.Index = _bar.Index;
									gevt.State = 1;
									gevt.Something = this;
									SFEventManager.TriggerEvent(gevt);
									_bar = null;
								}
							}
						}
						else
						{
							if (_bar != null)
							{
								_bar.Idle();
								_bar = null;
							}

						}
					}
					
				}
				
			}
		}
	}
}
