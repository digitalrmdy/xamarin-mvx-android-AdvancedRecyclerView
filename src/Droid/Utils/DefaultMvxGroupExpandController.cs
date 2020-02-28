using MvvmCross.AdvancedRecyclerView.Data;
using MvvmCross.AdvancedRecyclerView.Data.EventArguments;

namespace MvvmCross.AdvancedRecyclerView.Utils
{
	internal class DefaultMvxGroupExpandController : MvxGroupExpandController
	{
		public override bool CanExpandGroup(MvxGroupDetails groupDetails) => true;
		public override bool CanCollapseGroup(MvxGroupDetails groupDetails) => true;
		public override bool OnHookGroupExpand(MvxHookGroupExpandCollapseArgs groupItemDetails) => true;

		public override bool OnHookGroupCollapse(MvxHookGroupExpandCollapseArgs groupItemDetails) => true;
	}
}