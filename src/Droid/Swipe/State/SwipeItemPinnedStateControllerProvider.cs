using System;
using MvvmCross.AdvancedRecyclerView.Data;

namespace MvvmCross.AdvancedRecyclerView.Swipe.State
{
	public class SwipeItemPinnedStateControllerProvider
	{
		private IMvxItemUniqueIdProvider _uniqueIdProvider;
		private readonly SwipeItemPinnedStateController _bottomSwipeStateController;
		private readonly SwipeItemPinnedStateController _leftSwipeStateController;
		private readonly SwipeItemPinnedStateController _rightSwipeStateController;
		private readonly SwipeItemPinnedStateController _topSwipeStateController;

		public SwipeItemPinnedStateControllerProvider()
		{
			_leftSwipeStateController = new SwipeItemPinnedStateController();
			_rightSwipeStateController = new SwipeItemPinnedStateController();
			_topSwipeStateController = new SwipeItemPinnedStateController();
			_bottomSwipeStateController = new SwipeItemPinnedStateController();
		}

		public IMvxItemUniqueIdProvider UniqueIdProvider
		{
			get => _uniqueIdProvider;
			set
			{
				_uniqueIdProvider = value;
				_leftSwipeStateController.UniqueIdProvider = value;
				_rightSwipeStateController.UniqueIdProvider = value;
				_topSwipeStateController.UniqueIdProvider = value;
				_bottomSwipeStateController.UniqueIdProvider = value;
			}
		}

		public SwipeItemPinnedStateController ForLeftSwipe() => _leftSwipeStateController;

		public SwipeItemPinnedStateController ForRightSwipe() => _rightSwipeStateController;

		public SwipeItemPinnedStateController ForTopSwipe() => _topSwipeStateController;

		public SwipeItemPinnedStateController ForBottomSwipe() => _bottomSwipeStateController;

		public SwipeItemPinnedStateController FromSwipeDirection(SwipeDirection swipeDirection)
		{
			return swipeDirection switch
			{
				SwipeDirection.FromBottom => ForBottomSwipe(),
				SwipeDirection.FromTop => ForTopSwipe(),
				SwipeDirection.FromLeft => ForLeftSwipe(),
				SwipeDirection.FromRight => ForRightSwipe(),
				_ => throw new InvalidOperationException($"{swipeDirection} swipe direction is not implemented.")
			};
		}

		public bool IsPinnedForAnyState(object item)
		{
			return ForTopSwipe().IsPinned(item) ||
			       ForRightSwipe().IsPinned(item) ||
			       ForLeftSwipe().IsPinned(item) ||
			       ForBottomSwipe().IsPinned(item);
		}

		public void SetPinnedForAllStates(object item, bool isPinned)
		{
			ForTopSwipe().SetPinnedState(item, isPinned);
			ForBottomSwipe().SetPinnedState(item, isPinned);
			ForLeftSwipe().SetPinnedState(item, isPinned);
			ForRightSwipe().SetPinnedState(item, isPinned);
		}

		public void ResetState()
		{
			_leftSwipeStateController.ResetState();
			_rightSwipeStateController.ResetState();
			_bottomSwipeStateController.ResetState();
			_topSwipeStateController.ResetState();
		}
	}
}