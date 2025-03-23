using App.Inventory;
using SFramework;
using SFramework.Game;

namespace App.SelectArea
{
	public class SelectAreaControl : RootControl
	{
		public static string SAVEAREA = "SAVEAREA";
		public override ViewOpenType GetViewOpenType()
		{
			return ViewOpenType.Single;
		}
		protected override void opening()
		{
			// Code Here
		}
		protected override void alreadyOpened()
		{
			base.alreadyOpened();
			// Code Here
		}
		protected override void closing()
		{
			// Code Here
		}

		public override void HandleMessage(SBundleParams value)
		{
			if (value.MessageId == SAVEAREA)
			{
				InventoryControl inv = GetControl<InventoryControl>();
				inv.SetArea((int)value.MessageData);
			}
		}
		
	}
}
