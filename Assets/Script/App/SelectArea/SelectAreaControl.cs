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
			RefreshAreaName();
			// Code Here
		}
		protected override void closing()
		{
			// Code Here
		}

		public void RefreshAreaName()
		{
			InventoryControl inv = GetControl<InventoryControl>();
			string area = inv.GetAreaName();
			SelectAreaView view = GetView<SelectAreaView>();
			view.RefreshArea(area);
		}

		public override void HandleMessage(SBundleParams value)
		{
			if (value.MessageId == SAVEAREA)
			{
				RefreshAreaName();
			}
		}
		
	}
}
