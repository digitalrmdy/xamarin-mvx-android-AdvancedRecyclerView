namespace MvvmCross.AdvancedRecyclerView.TemplateSelectors
{
	public interface IMvxSwipeableTemplate
	{
		int SwipeContainerViewGroupId { get; }

		int UnderSwipeContainerViewGroupId { get; }

		int SwipeReactionType { get; }
	}
}