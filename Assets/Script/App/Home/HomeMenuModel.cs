using SFramework.Game;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System;

namespace App.Home
{
	public class HomeMenuModel : RootModel
	{
		protected override void opening()
		{
			requestBaidu().Forget();
		}

		private async UniTask<bool> requestBaidu()
		{
			// 创建进度报告器，添加回调函数打印进度值
			IProgress<float> progress = new Progress<float>(progressValue => {
				// 将进度值转换为百分比并打印
				Debug.Log($"下载进度: {progressValue * 100:F2}%");
			});
			
			// 调用带有进度参数的GetData方法
			string url = "https://www.baidu.com";
			byte[] data = await GetData(url, progress);
			
			if (data != null)
			{
				Debug.Log($"下载完成，数据大小: {data.Length} 字节");
			}
			else
			{
				Debug.Log("下载失败");
			}
			await GetData();
			return true;
		}
		protected override void closing()
		{
			// Code Here
		}
	}
}
