using Config.LanguageBase;
using SFramework.Game;
using Cysharp.Threading.Tasks;

namespace App.Language
{
	public class LanguageModel : RootModel
	{
		private Language_Base_datas _language;
		private string _currentLanguageSymbol;
		protected override void opening()
		{
			//通过系统获得语言或者是通过设置来获得
			ReadUserData().Forget();
		}

		public void SetLanguage(string languageSymbol)
		{
			_currentLanguageSymbol = languageSymbol;
		}

		public string GetLanguage(string languageKey)
		{
			Language_Base lb;
			_language.Datamap.TryGetValue(languageKey, out lb);
			if (lb == null)
				return "";
			if (_currentLanguageSymbol == "" || _currentLanguageSymbol == null)
				_currentLanguageSymbol = "zh-Hans";
			if (_currentLanguageSymbol == "zh-Hans")
			{
				return lb.ZhHans;
			}else if (_currentLanguageSymbol == "zh-Hant")
			{
				return lb.ZhHant;
			}else if (_currentLanguageSymbol == "en")
			{
				return lb.En;
			}else if (_currentLanguageSymbol == "ja")
			{
				return lb.Ja;
			}

			return "";
		}
		
		public async UniTask ReadUserData()
		{
			//读取配置文件
			_language = await configManager.GetConfigAsync<Language_Base_datas>();
			await GetData();
		}
		protected override void closing()
		{
			// Code Here
		}
		
		
	}
}
