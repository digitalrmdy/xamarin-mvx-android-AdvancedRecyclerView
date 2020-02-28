﻿﻿using System;
using System.Windows.Input;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Com.H6ah4i.Android.Widget.Advrecyclerview.Animator;
using MvvmCross.AdvancedRecyclerView.Adapters;
using MvvmCross.AdvancedRecyclerView.Adapters.NonExpandable;
using MvvmCross.AdvancedRecyclerView.Extensions;
using MvvmCross.Platforms.Android.Binding.BindingContext;

namespace MvvmCross.AdvancedRecyclerView
{
	[Register("mvvmcross.advancedrecyclerview.MvxAdvancedNonExpandableRecyclerView")]
	public sealed class MvxAdvancedNonExpandableRecyclerView : MvxAdvancedRecyclerView
	{
		public MvxAdvancedNonExpandableRecyclerView(IntPtr javaReference, JniHandleOwnership transfer) : base(
			javaReference, transfer)
		{
		}

		public MvxAdvancedNonExpandableRecyclerView(Context context) : this(context, null)
		{
		}

		public MvxAdvancedNonExpandableRecyclerView(Context context, IAttributeSet attrs) : this(context, attrs, 0)
		{
		}

		public MvxAdvancedNonExpandableRecyclerView(Context context, IAttributeSet attrs,
			int defStyle) : base(context, attrs, defStyle)
		{
			AdapterController =
				new MvxAdvancedRecyclerViewNonExpandableAdapterController(context, attrs, this,
					MvxAndroidBindingContextHelpers.Current());
			SetupDefaultItemAnimator(MvxAdvancedRecyclerViewAttributeExtensions.IsSwipeSupported(context, attrs));
		}

		private void SetupDefaultItemAnimator(bool isSwipeSupported)
		{
			// Change animations are enabled by default since support-v7-recyclerview v22.
			// Need to disable them when using animation indicator.
			var itemAnimator = isSwipeSupported 
				? new SwipeDismissItemAnimator {SupportsChangeAnimations = false} 
				: new RefactoredDefaultItemAnimator {SupportsChangeAnimations = false};

			SetItemAnimator(itemAnimator);
		}

		public ICommand ItemClick
		{
			get => NonExpandableAdapter?.ItemClick;
			set => NonExpandableAdapter.ItemClick = value;
		}


		public ICommand ItemLongClick
		{
			get => NonExpandableAdapter?.ItemLongClick;
			set => NonExpandableAdapter.ItemLongClick = value;
		}

		public MvxNonExpandableAdapter NonExpandableAdapter => AdapterController.AdvancedRecyclerViewAdapter as MvxNonExpandableAdapter;
		
		protected override MvxAdvancedRecyclerViewAdapterController AdapterController { get; }
	}
}
