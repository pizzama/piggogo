using App.Inventory;
using SFramework;
using SFramework.Game;

namespace App.Home
{
	public class RenameControl : RootControl
	{
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

		public string GetUserName()
		{
			InventoryControl inv = GetControl<InventoryControl>();
			return inv.GetUserName();
		}
	}
}
