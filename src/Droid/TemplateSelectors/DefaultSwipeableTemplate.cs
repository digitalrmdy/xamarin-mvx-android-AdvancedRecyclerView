using Com.H6ah4i.Android.Widget.Advrecyclerview.Swipeable;

namespace MvvmCross.AdvancedRecyclerView.TemplateSelectors
{
	public class DefaultSwipeableTemplate : IMvxSwipeableTemplate
	{
		public int SwipeContainerViewGroupId => -1;

		public int UnderSwipeContainerViewGroupId => -1;

		public int SwipeReactionType => SwipeableItemConstants.ReactionCanNotSwipeAny;
	}
}