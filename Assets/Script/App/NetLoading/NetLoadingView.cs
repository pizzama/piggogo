using Cysharp.Threading.Tasks;
using UnityEngine;
using SFramework;
using SFramework.Game;
using SFramework.Pool;
using UnityEngine.UI;

namespace App.NetLoading
{
	public class NetLoadingView : SUIView
	{
		protected override UILayer GetViewLayer()
		{
			return UILayer.Popup;
		}
		
		protected override void SetViewPrefabPath(out string prefabPath, out string prefabName, out Vector2 offsetMin, out Vector2 offsetMax, out Quaternion rotation)
		{
			prefabPath = "NetLoading/NetLoadingView";
			prefabName = "NetLoadingView";
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
		}
		protected override void closing()
		{
			// Code Here
		}
	}
}
