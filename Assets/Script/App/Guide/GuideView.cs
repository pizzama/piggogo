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
using UnityEngine.XR;

namespace App.Guide
{
	public class GuideView : SUIView
	{
		private SkeletonGraphic _hand;
		private Transform _maskAniHole;
		private Sequence _handSeq;
		
		private Flowchart _curChart;
		private int _count = -1;
		private Text _content;
		private Button _contentBtn;

		public SkeletonGraphic Hand
		{
			get { return _hand; }
		}
		protected override UILayer GetViewLayer()
		{
			return UILayer.Toast;
		}
		
		protected override void SetViewPrefabPath(out string prefabPath, out string prefabName, out Vector3 position, out Quaternion rotation)
		{
			prefabPath = "Guide/GuideView";
			prefabName = "GuideView";
			position = new Vector3(0, 0, 0);
			rotation = Quaternion.Euler(0, 0, 0);
		}
		
		protected override async UniTaskVoid SetViewTransformAsync(Vector3 position, Quaternion rotation)
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
			openUI(mViewTransform, position, rotation);
		}
		protected override void opening()
		{
			// Code Here
			_hand = getExportObject<SkeletonGraphic>("Handle");
			_maskAniHole = getExportObject<Transform>("MaskAniHole");
			_content = getExportObject<Text>("Content");
			_contentBtn = getExportObject<Button>("ContentBtn");
			mViewTransform.gameObject.SetActive(false);
			
			_contentBtn.onClick.AddListener(ContentClick);
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

		public void Play(int level)
		{
			if (_curChart != null)
			{
				GameObject.Destroy(_curChart.gameObject);
				_curChart = null;
			}

			string strparmas = string.Format("RES_GuideLevel{0}_prefab", level);
			bool rt = ReflectionTools.HasPublicConstString(typeof(App_guide_sfp), strparmas);
			if (rt)
			{
				mViewTransform.gameObject.SetActive(true);
				_curChart = CreateComponent<Flowchart>(SFResAssets.App_guide_sfp_GuideLevel1_prefab, mViewTransform);
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

		public void DisplayHand()
		{
			if (_hand != null)
			{
				_hand.gameObject.SetActive(true);
				_maskAniHole.gameObject.SetActive(true);
			}
		}

		public void HideHand()
		{
			if (_hand != null)
			{
				_hand.gameObject.SetActive(false);
				_maskAniHole.gameObject.SetActive(false);
			}
		}

		public void PointHand(Vector3 pos)
		{
			DisplayHand();
			_hand.transform.localPosition = pos;
			_maskAniHole.transform.localPosition = pos;
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
		
	}
}
