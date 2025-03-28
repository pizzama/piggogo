using System.Collections.Generic;
using App.Language;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Fungus;
using UnityEngine;
using SFramework;
using SFramework.Event;
using SFramework.Game;
using SFramework.Pool;
using SFramework.Statics;
using SFramework.Tools;
using Spine.Unity;
using UnityEngine.UI;

namespace App.Guide
{
	public class GuideView : SUIView
	{
		private SkeletonGraphic _hand;
		private GuideCircle _maskAniHole;
		private Sequence _handSeq;
		
		private Flowchart _curChart;
		private int _count = -1;
		private Text _content;
		private Button _contentBtn;

		private const string _guidePrefabPath = "app_guide.sfp/GuideLevel";
		private Transform _guideBack;
		private Transform _guideMiddle;
		private Transform _guideFront;
		public SkeletonGraphic Hand
		{
			get { return _hand; }
		}
		protected override UILayer GetViewLayer()
		{
			return UILayer.Toast;
		}
		
		protected override void SetViewPrefabPath(out string prefabPath, out string prefabName, out Vector2 offsetMin, out Vector2 offsetMax, out Quaternion rotation)
		{
			prefabPath = "Guide/GuideView";
			prefabName = "GuideView";
			offsetMin = new Vector3(0, 0, 0);
			offsetMax = new Vector3(0, 0, 0);
			rotation = Quaternion.Euler(0, 0, 0);
		}
		
		protected override async UniTaskVoid SetViewTransformAsync(Vector2 offsetMin, Vector2 offsetMax, Quaternion rotation)
		{
			if (!string.IsNullOrEmpty(mAbName) && !string.IsNullOrEmpty(mResName))
			{
				GameObject prefab = await assetManager.LoadFromResourcesAsync<GameObject>(mAbName);
				if (prefab == null)
					throw new NotFoundException("not found uiView from resources:" + mAbName + ";" + mResName);
				string fullPath = assetManager.FullPath(mAbName, mResName);
				GameObject ob = poolManager.Request<ListGameObjectPool>(fullPath, prefab, -1, 5);
				mViewTransform = ob.transform;
			}
			openUI(mViewTransform, offsetMin, offsetMax, rotation);
		}
		protected override void opening()
		{
			// Code Here
			_hand = getExportObject<SkeletonGraphic>("Handle");
			_maskAniHole = getExportObject<GuideCircle>("MaskAniHole");
			_content = getExportObject<Text>("Content");
			_contentBtn = getExportObject<Button>("ContentBtn");
			_contentBtn.onClick.AddListener(ContentClick);
			_guideFront = getExportObject<Transform>("GuideFront");
			_guideMiddle = getExportObject<Transform>("GuideMiddle");
			_guideBack = getExportObject<Transform>("GuideBack");
			HideGuideAll();
		}
		protected override void closing()
		{
			// Code Here
			_contentBtn.onClick.RemoveListener(ContentClick);
		}

		public void RefreshContent(string value, bool callbackEnable)
		{
			contentBtnCallbackEnable(callbackEnable);
			if (_content != null)
				_content.text = LanguageControl.Instance.GetLanguage(value);
		}

		public async UniTask Play(int level)
		{
			CloseGuildeElement();
			List<int> guideLevels =  GetModel<GuideModel>().GuideLevel;
			if(guideLevels.Contains(level))
			{
				DisplayGuide();
				_curChart = await CreateComponentAsync<Flowchart>(_guidePrefabPath + level, mViewTransform);
			}

			if (_curChart != null)
			{
				PlayNext();
			}
		}

		public void PlayNext()
		{
			HideHand();
			if (_curChart != null)
			{
				_count += 1;
				_curChart.ExecuteBlock("Guide" + _count);
			}
		}

		public void DisplayGuide()
		{
			mViewTransform.gameObject.SetActive(true);
		}

		public void HideGuide()
		{
			mViewTransform.gameObject.SetActive(false);
		}

		public void HideGuideAll()
		{
			HideGuide();
			HideHand();
			HideMaskHole();
		}

		public void DisplayHand()
		{
			if (_hand != null)
			{
				_hand.gameObject.SetActive(true);
			}
		}

		public void DisplayMaskHole()
		{
			if (_maskAniHole)
				_maskAniHole.gameObject.SetActive(true);
		}

		public void HideMaskHole()
		{
			if (_maskAniHole)
				_maskAniHole.gameObject.SetActive(false);
		}

		public void HideHand()
		{
			if (_hand != null)
			{
				_hand.gameObject.SetActive(false);
			}
		}

		public void PointHand(Vector3 pos)
		{
			DisplayHand();
			_hand.transform.localPosition = pos;
			_maskAniHole.MoveTarget(pos);
		}

		public void PointHole(Vector3 pos)
		{
			_maskAniHole.MoveHole(pos);
		}

		private void ContentClick()
		{
			SFEventManager.TriggerEvent(new GuideEvent());
		}

		private void contentBtnCallbackEnable(bool value)
		{
			if (_contentBtn != null)
			{
				_contentBtn.GetComponent<Image>().enabled = value;
				_contentBtn.enabled = value;
			}
		}

		public RectTransform GetGuideBack()
		{
			return _guideBack.GetComponent<RectTransform>();
		}

		public void AddFrontImage(Image img, Vector3 pos, Vector3 scale)
		{
			addGuideImage(_guideFront, img, pos, scale);
		}

		public void AddMiddelImage(Image img, Vector3 pos, Vector3 scale)
		{
			addGuideImage(_guideMiddle, img, pos, scale);
		}

		public void AddBackImage(Image img, Vector3 pos, Vector3 scale)
		{
			addGuideImage(_guideBack, img, pos, scale);
		}

		public void RemoveFrontByName(string name)
		{
			destoryGuideByName(_guideFront, name);
		}

		public void RemoveMiddelByName(string name)
		{
			destoryGuideByName(_guideMiddle, name);
		}

		public void RemoveBackByName(string name)
		{
			destoryGuideByName(_guideBack, name);
		}

		public void CloseGuildeElement()
		{
			if (_curChart != null)
			{
				GameObject.Destroy(_curChart.gameObject);
				_curChart = null;
			}
			_count = -1;

			destoryGruide(_guideFront);
			destoryGruide(_guideMiddle);
			destoryGruide(_guideBack);
		}

		private void addGuideImage(Transform parent, Image img, Vector3 pos, Vector3 scale)
		{
			img.transform.SetParent(parent);
			img.transform.localPosition = pos;
			img.transform.localScale = scale;
		}

		private void destoryGuideByName(Transform parent, string name)
		{
			for (int i = parent.childCount - 1; i >= 0 ; i--)
			{
				var child = parent.GetChild(i);
				if (child != null && child.name == name)
				{
					ReleaseGameObjectDestroy(child.gameObject, false);
				}
			}
		}

		private void destoryGruide(Transform parent)
		{
			for (int i = parent.childCount - 1; i >= 0 ; i--)
			{
				var child = parent.GetChild(i);
				ReleaseGameObjectDestroy(child.gameObject, false);
			}
		}

		public void DisplayGuideFront()
		{
			_guideFront.gameObject.SetActive(true);
		}
		
		public void HideGuideFront()
		{
			_guideFront.gameObject.SetActive(false);
		}

		public void DisplayGuideMiddle()
		{
			_guideMiddle.gameObject.SetActive(true);
		}
		
		public void HideGuideMiddle()
		{
			_guideMiddle.gameObject.SetActive(false);
		}

		public void DisplayGuideBack()
		{
			_guideBack.gameObject.SetActive(true);
		}
		
		public void HideGuideBack()
		{
			_guideBack.gameObject.SetActive(false);
		}

	}
}
