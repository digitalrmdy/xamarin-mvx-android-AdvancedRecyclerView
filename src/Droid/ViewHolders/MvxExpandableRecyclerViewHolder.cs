using System;
using Android.Runtime;
using Android.Views;
using Com.H6ah4i.Android.Widget.Advrecyclerview.Expandable;
using MvvmCross.Platforms.Android.Binding.BindingContext;

namespace MvvmCross.AdvancedRecyclerView.ViewHolders
{
	public class MvxExpandableRecyclerViewHolder : MvxAdvancedRecyclerViewHolder, IExpandableItemViewHolder
	{
		public MvxExpandableRecyclerViewHolder(View itemView, IMvxAndroidBindingContext context) : base(itemView, context)
		{
		}

		public MvxExpandableRecyclerViewHolder(IntPtr handle, JniHandleOwnership ownership) : base(handle, ownership)
		{
		}

		public int ExpandStateFlags { get; set; }
	}
}