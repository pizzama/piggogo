using UnityEngine;
using SFramework;
using SFramework.Game;
using UnityEngine.UI;

namespace App.Home
{
	public class RenameView : SUIView
	{
		private InputField _inputText;
		private Button _bgBtn;
		private Button _confirmBtn;
		protected override UILayer GetViewLayer()
		{
			return UILayer.Popup;
		}
		protected override void opening()
		{
			// Code Here
			_inputText = getExportObject<InputField>("InputField");
			
			// 设置InputField的初始文本
			_inputText.text = GetControl<RenameControl>().GetUserName();
			
			// 添加输入变化事件监听
			_inputText.onValueChanged.AddListener(OnInputValueChanged);
			
			// 添加结束编辑事件监听
			_inputText.onEndEdit.AddListener(OnInputEndEdit);
			
			// 获取确认按钮并添加点击事件
			_confirmBtn = getExportObject<Button>("OkBtn");
			_confirmBtn.onClick.AddListener(OnConfirmButtonClick);
			
			_bgBtn = getExportObject<Button>("BgBtn");
			_bgBtn.onClick.AddListener(bgBtnClick);
		}
		
		private void OnInputValueChanged(string value)
		{
			// 处理输入变化
			Debug.Log("输入内容变化: " + value);
		}
		
		private void OnInputEndEdit(string value)
		{
			// 处理输入结束
			Debug.Log("输入结束: " + value);
			
			// 可以在这里进行输入验证
			if (string.IsNullOrEmpty(value))
			{
				_inputText.text = "名称不能为空";
			}
		}
		
		private void OnConfirmButtonClick()
		{
			// 处理确认按钮点击
			string inputValue = _inputText.text;
			Debug.Log("确认名称: " + inputValue);
			// 发送请求
			
			// 关闭重命名面板
			Control.Close();
		}
		
		protected override void closing()
		{
			// 移除事件监听
			if (_inputText != null)
			{
				_inputText.onValueChanged.RemoveAllListeners();
				_inputText.onEndEdit.RemoveAllListeners();
			}
			
			_confirmBtn.onClick.RemoveAllListeners();
			_bgBtn.onClick.AddListener(bgBtnClick);
		}

		private void bgBtnClick()
		{
			Control.Close();
		}
	}
}
