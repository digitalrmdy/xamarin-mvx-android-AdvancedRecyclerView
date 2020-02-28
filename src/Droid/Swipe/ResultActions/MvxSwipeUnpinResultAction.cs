using System;
using Android.Runtime;
using Com.H6ah4i.Android.Widget.Advrecyclerview.Swipeable.Action;
using MvvmCross.AdvancedRecyclerView.Adapters.NonExpandable;

namespace MvvmCross.AdvancedRecyclerView.Swipe.ResultActions
{
	public class MvxSwipeUnpinResultAction : SwipeResultActionDefault
	{
		private MvxNonExpandableAdapter _adapter;
		private readonly int _position;

		public MvxSwipeUnpinResultAction(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
		{
		}

		public MvxSwipeUnpinResultAction(MvxNonExpandableAdapter adapter, int position)
		{
			_adapter = adapter;
			_position = position;
		}

		protected override void OnPerformAction()
		{
			base.OnPerformAction();

			var item = _adapter.GetItem(_position);
			if (!_adapter.SwipeItemPinnedStateController.IsPinnedForAnyState(item))
			{
				return;
			}

			_adapter.SwipeItemPinnedStateController.SetPinnedForAllStates(item, false);
			_adapter.NotifyItemChanged(_position);
		}

		protected override void OnCleanUp()
		{
			base.OnCleanUp();
			_adapter = null;
		}
	}
}