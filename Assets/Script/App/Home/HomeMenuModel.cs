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
			GetData().Forget();
		}

		protected override void closing()
		{
			// Code Here
		}
	}
}
