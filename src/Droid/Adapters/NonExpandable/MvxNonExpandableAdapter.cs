using System;
using System.Collections.Specialized;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Com.H6ah4i.Android.Widget.Advrecyclerview.Swipeable;
using Com.H6ah4i.Android.Widget.Advrecyclerview.Swipeable.Action;
using MvvmCross.AdvancedRecyclerView.Data;
using MvvmCross.AdvancedRecyclerView.Data.EventArguments;
using MvvmCross.AdvancedRecyclerView.Swipe.ResultActions;
using MvvmCross.AdvancedRecyclerView.Swipe.State;
using MvvmCross.AdvancedRecyclerView.TemplateSelectors;
using MvvmCross.AdvancedRecyclerView.ViewHolders;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using Object = Java.Lang.Object;

namespace MvvmCross.AdvancedRecyclerView.Adapters.NonExpandable
{
	public class MvxNonExpandableAdapter : MvxRecyclerAdapter, ISwipeableItemAdapter, IMvxAdvancedRecyclerViewAdapter
	{
		private readonly Lazy<DefaultSwipeableTemplate> _lazyDefaultSwipeableTemplate = new Lazy<DefaultSwipeableTemplate>();

		public MvxNonExpandableAdapter(IMvxAndroidBindingContext bindingContext) : base(bindingContext)
		{
			HasStableIds = true;
		}

		public MvxNonExpandableAdapter(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
		{
		}

		private IMvxSwipeableTemplate _swipeableTemplate;

		public IMvxSwipeableTemplate SwipeableTemplate
		{
			get => _swipeableTemplate ?? _lazyDefaultSwipeableTemplate.Value;
			set => _swipeableTemplate = value;
		}

		public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
		{
			var itemBindingContext = new MvxAndroidBindingContext(parent.Context, BindingContext.LayoutInflaterHolder);

			var viewForHolder = InflateViewForHolder(parent, viewType, itemBindingContext);

			var vh = new MvxAdvancedRecyclerViewHolder(viewForHolder,
				itemBindingContext)
			{
				Click = ItemClick,
				LongClick = ItemLongClick
			};

			return vh;
		}

		public int OnGetSwipeReactionType(Object p0, int p1, int x, int y)
		{
			return 0;
		}

		public void OnSetSwipeBackground(Object viewHolder, int position, int type)
		{
			var advancedViewHolder = viewHolder as MvxAdvancedRecyclerViewHolder;
			SwipeBackgroundSet?.Invoke(new MvxSwipeBackgroundSetEventArgs()
			{
				ViewHolder = advancedViewHolder,
				Position = position,
				Type = type
			});
		}

		public SwipeResultAction OnSwipeItem(Object p0, int position, int result)
		{
			switch (result)
			{
				case SwipeableItemConstants.ResultSwipedDown:
					return SwipeResultActionFactory.GetSwipeDownResultAction(this, position);
				case SwipeableItemConstants.ResultSwipedLeft:
					return SwipeResultActionFactory.GetSwipeLeftResultAction(this, position);
				case SwipeableItemConstants.ResultSwipedRight:
					return SwipeResultActionFactory.GetSwipeRightResultAction(this, position);
				case SwipeableItemConstants.ResultSwipedUp:
					return SwipeResultActionFactory.GetSwipeUpResultAction(this, position);
				default:
					return position != RecyclerView.NoPosition ? SwipeResultActionFactory.GetUnpinSwipeResultAction(this, position) : new SwipeResultActionDoNothing();
			}
		}

		public override long GetItemId(int position)
		{
			if (UniqueIdProvider == null)
			{
				throw new InvalidOperationException($"You have to assign {nameof(UniqueIdProvider)} property to use AdvancedRecyclerView Adapter");
			}

			var item = GetItem(position);
			return UniqueIdProvider.GetUniqueId(item);
		}

		/// <summary>
		/// Use SwipeableItemConstants.ReactionCan/CanNotSwipe to determine available swipe type.
		/// </summary>
		public int SwipeReactionType => SwipeableTemplate.SwipeReactionType;

		public MvxSwipeResultActionFactory SwipeResultActionFactory { get; set; } = new MvxSwipeResultActionFactory();

		private IMvxItemUniqueIdProvider _uniqueIdProvider;

		public IMvxItemUniqueIdProvider UniqueIdProvider
		{
			get => _uniqueIdProvider;
			set
			{
				_uniqueIdProvider = value;
				SwipeItemPinnedStateController.UniqueIdProvider = value;
			}
		}

		public SwipeItemPinnedStateControllerProvider SwipeItemPinnedStateController { get; } = new SwipeItemPinnedStateControllerProvider();

		public override void NotifyDataSetChanged(NotifyCollectionChangedEventArgs e)
		{
			base.NotifyDataSetChanged(e);

			if (e.Action == NotifyCollectionChangedAction.Reset)
			{
				SwipeItemPinnedStateController.ResetState();
			}
			else if (e.Action == NotifyCollectionChangedAction.Remove && e.OldItems != null)
			{
				foreach (var itemsToRemove in e.OldItems)
				{
					SwipeItemPinnedStateController.SetPinnedForAllStates(itemsToRemove, false);
				}
			}
		}

		public void OnSwipeItemStarted(Object p0, int p1)
		{
			NotifyDataSetChanged();
		}

		public event Action<MvxSwipeBackgroundSetEventArgs> SwipeBackgroundSet;
	}
}