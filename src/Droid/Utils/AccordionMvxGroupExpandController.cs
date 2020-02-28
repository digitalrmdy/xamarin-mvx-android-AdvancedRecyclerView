using MvvmCross.AdvancedRecyclerView.Data;
using MvvmCross.AdvancedRecyclerView.Data.EventArguments;

namespace MvvmCross.AdvancedRecyclerView.Utils
{
	public class AccordionMvxGroupExpandController : MvxGroupExpandController
	{
		private int _currentlyExpandedIndex = -1;

		public override bool AreGroupsExpandedByDefault => false;

		public override bool CanCollapseGroup(MvxGroupDetails groupDetails)
		{
			return true;
		}

		public override bool CanExpandGroup(MvxGroupDetails groupDetails)
		{
			return true;
		}

		public override bool OnHookGroupCollapse(MvxHookGroupExpandCollapseArgs groupItemDetails)
		{
			_currentlyExpandedIndex = -1;
			return true;
		}

		public override bool OnHookGroupExpand(MvxHookGroupExpandCollapseArgs groupItemDetails)
		{
			if (_currentlyExpandedIndex != -1)
			{
				ExpandableItemManager.CollapseGroup(_currentlyExpandedIndex);
			}

			_currentlyExpandedIndex = groupItemDetails.GroupPosition;

			if (ItemHeight.HasValue)
			{
				ExpandableItemManager.ScrollToGroup(_currentlyExpandedIndex, ItemHeight.Value, 20, 20);
			}

			return true;
		}

		public int? ItemHeight { get; set; }
	}
}