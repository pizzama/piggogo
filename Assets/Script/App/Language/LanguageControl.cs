using SFramework;
using UnityEngine;
using SFramework.Game;

namespace App.Language
{
	public class LanguageControl : RootControl
	{
		public static LanguageControl Instance;
		public override ViewOpenType GetViewOpenType()
		{
			return ViewOpenType.SingleNeverClose;
		}
		protected override void opening()
		{
			// Code Here
			Instance = this;
		}
		protected override void alreadyOpened()
		{
			// Code Here
		}
		protected override void closing()
		{
			// Code Here
		}

		public void SetLanguage(string languageSymbol)
		{
			LanguageModel model = GetModel<LanguageModel>();
			model.SetLanguage(languageSymbol);
		}

		public string GetLanguage(string languageKey)
		{
			LanguageModel model = GetModel<LanguageModel>();
			return model.GetLanguage(languageKey);
		}
	}
}
