using Com.H6ah4i.Android.Widget.Advrecyclerview.Swipeable.Action;
using MvvmCross.AdvancedRecyclerView.Adapters.NonExpandable;

namespace MvvmCross.AdvancedRecyclerView.Swipe.ResultActions
{
	public class MvxSwipeResultActionFactory
	{
		public virtual SwipeResultAction GetSwipeUpResultAction(MvxNonExpandableAdapter adapter, int position)
		{
			return new SwipeResultActionDoNothing();
		}

		public virtual SwipeResultAction GetSwipeDownResultAction(MvxNonExpandableAdapter adapter, int position)
		{
			return new SwipeResultActionDoNothing();
		}

		public virtual SwipeResultAction GetSwipeLeftResultAction(MvxNonExpandableAdapter adapter, int position)
		{
			return new SwipeResultActionDoNothing();
		}

		public virtual SwipeResultAction GetSwipeRightResultAction(MvxNonExpandableAdapter adapter, int position)
		{
			return new SwipeResultActionDoNothing();
		}

		public virtual SwipeResultAction GetUnpinSwipeResultAction(MvxNonExpandableAdapter adapter, int position)
		{
			return new SwipeResultActionDoNothing();
		}
	}
}