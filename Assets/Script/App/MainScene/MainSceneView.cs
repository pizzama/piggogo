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

namespace App.MainScene
{
	public class MainSceneView : SSCENEView
	{
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
			DealWithBranch();
		}
		
		public void DealWithBranch()
		{
			for (int i = 0; i < _branchs.childCount; i++)
			{
				var bh = _branchs.GetChild(i);
				bh.gameObject.SetActive(false);
				SeatBar bar = bh.GetComponent<SeatBar>();
				bar.Recycle(); //回收之前的数据
				bar.SetEntityData((i + 1).ToString(), this);
				bar.SetIndex(i + 1);
				bar.Show();
				for (int j = 0; j < _model.CurLevelDetails.Count; j++)
				{
					var detail = _model.CurLevelDetails[j];
					if (bh.name == "Branch" + detail.Index)
					{
						bar.SetData(detail);
						break;
					}
				}
			}
		}

		public async UniTask<Item> CreateItem(int itemId, Vector3 pos, bool isleft)
		{
			Items_Base config = _model.GetItemBaseById(itemId);
			Item it = await CreateEntityAsync<Item>(SFResAssets.App_mainscene_sfp_Pig_prefab, _itemsContainer);
			it.transform.position = pos;
			it.LoadSpine(config, isleft);
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

		private void mouseHandle(bool isTouchUI, SInputEventType enumInputEventType, Vector3 mousePosition,
			int clickCount, int keyCode)
		{
			if (enumInputEventType == SInputEventType.Click)
			{
				if (isTouchUI == false)
				{
					//发送射线做碰撞检测, 需要确认当前的摄像机是哪一个才能判断射线是否正确
					_ray = UIRoot.MainCamera.ScreenPointToRay(Input.mousePosition);
					RaycastHit2D hit;
					int mask = LayerMask.GetMask("Default");
					hit = Physics2D.Raycast(new Vector2(_ray.origin.x, _ray.origin.y), Vector2.zero, Mathf.Infinity, mask);
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
								
								// 广播新手引导
								SFEventManager.TriggerEvent(new GuideEvent());
							}
							else
							{
								bool rt = entity.Merge(_bar);
								if (!rt)
								{
									_bar.Idle();
								}
								_bar = null;
								
								// 广播新手引导
								SFEventManager.TriggerEvent(new GuideEvent());
							}
						}
						Debug.Log("hit collider:" + hit.collider.tag + ";" + hit.collider.name);
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
