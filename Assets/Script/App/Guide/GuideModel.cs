using System.Collections.Generic;
using SFramework.Game;
using Cysharp.Threading.Tasks;

namespace App.Guide
{
	public class GuideModel : RootModel
	{
		private List<int> _guideLevel = new List<int>()
		{
			1,2,3
		};
		
		public List<int> GuideLevel
		{
			get {return _guideLevel;}
		}
		protected override void opening()
		{
			 GetData().Forget();
		}
		protected override void closing()
		{
			// Code Here
		}
	}
}
