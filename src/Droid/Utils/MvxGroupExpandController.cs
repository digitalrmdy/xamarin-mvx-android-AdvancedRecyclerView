using Com.H6ah4i.Android.Widget.Advrecyclerview.Expandable;
using MvvmCross.AdvancedRecyclerView.Data;
using MvvmCross.AdvancedRecyclerView.Data.EventArguments;

namespace MvvmCross.AdvancedRecyclerView.Utils
{
	public abstract class MvxGroupExpandController
	{
		public RecyclerViewExpandableItemManager ExpandableItemManager { get; internal set; }

		public virtual bool AreGroupsExpandedByDefault => true;

		public abstract bool CanExpandGroup(MvxGroupDetails groupDetails);

		public abstract bool CanCollapseGroup(MvxGroupDetails groupDetails);

		public abstract bool OnHookGroupExpand(MvxHookGroupExpandCollapseArgs groupItemDetails);

		public abstract bool OnHookGroupCollapse(MvxHookGroupExpandCollapseArgs groupItemDetails);
	}
}