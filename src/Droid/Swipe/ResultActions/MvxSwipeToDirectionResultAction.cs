using System;
using Android.Runtime;
using Com.H6ah4i.Android.Widget.Advrecyclerview.Swipeable.Action;
using MvvmCross.AdvancedRecyclerView.Adapters.NonExpandable;

namespace MvvmCross.AdvancedRecyclerView.Swipe.ResultActions
{
	public class MvxSwipeToDirectionResultAction : SwipeResultActionMoveToSwipedDirection
	{
		private MvxNonExpandableAdapter _adapter;
		private readonly SwipeDirection _swipeDirection;
		private readonly int _position;

		public MvxSwipeToDirectionResultAction(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
		{
		}

		public MvxSwipeToDirectionResultAction(MvxNonExpandableAdapter adapter, SwipeDirection swipeDirection, int position)
		{
			_adapter = adapter;
			_swipeDirection = swipeDirection;
			_position = position;
		}

		protected override void OnPerformAction()
		{
			base.OnPerformAction();

			var stateController = _adapter.SwipeItemPinnedStateController.FromSwipeDirection(_swipeDirection);

			var item = _adapter.GetItem(_position);
			if (stateController.IsPinned(item))
			{
				return;
			}

			stateController.SetPinnedState(item, true);
			_adapter.NotifyItemChanged(_position);
		}

		protected override void OnCleanUp()
		{
			base.OnCleanUp();
			_adapter = null;
		}
	}
}