using System;
using Android.OS;
using Android.Support.V7.Widget;
using Com.H6ah4i.Android.Widget.Advrecyclerview.Expandable;
using MvvmCross.AdvancedRecyclerView.Extensions;
using MvvmCross.Droid.Support.V7.RecyclerView.ItemTemplates;
using MvvmCross.Platforms.Android.Binding.BindingContext;

namespace MvvmCross.AdvancedRecyclerView.Adapters.Expandable
{
	public class MvxAdvancedRecyclerViewExpandableAdapterController : MvxAdvancedRecyclerViewAdapterController
	{
		private RecyclerViewExpandableItemManager _expandableItemManager;
		private RecyclerView.Adapter _wrappedAdapter;
		private IParcelable _expandCollapseSavedState;

		private const string ExpandManagerParcelableKey = "ExpandManagerStateParcelKey";

		public MvxAdvancedRecyclerViewExpandableAdapterController(Android.Content.Context context, Android.Util.IAttributeSet attrs, RecyclerView recyclerView,
			IMvxAndroidBindingContext bindingContext)
			: base(context, attrs, recyclerView, bindingContext)
		{
		}

		protected override RecyclerView.Adapter BuildWrappedAdapter(IMvxTemplateSelector templateSelector)
		{
			var isGroupingSupported = MvxAdvancedRecyclerViewAttributeExtensions.IsGroupingSupported(Context, Attrs);

			if (!isGroupingSupported)
			{
				throw new InvalidOperationException($"You are using {nameof(MvxAdvancedExpandableRecyclerView)} without using grouping attributes. Check documentation.");
			}

			_expandableItemManager = new RecyclerViewExpandableItemManager(_expandCollapseSavedState);
			var expandableAdapter = new MvxExpandableItemAdapter(BindingContext)
			{
				TemplateSelector = templateSelector, GroupExpandController = MvxAdvancedRecyclerViewAttributeExtensions.BuildGroupExpandController(Context, Attrs)
			};

			_expandableItemManager.DefaultGroupsExpandedState = expandableAdapter.GroupExpandController.AreGroupsExpandedByDefault;
			expandableAdapter.GroupExpandController.ExpandableItemManager = _expandableItemManager;

			AdvancedRecyclerViewAdapter = expandableAdapter;
			var groupedDataConverter = MvxAdvancedRecyclerViewAttributeExtensions.BuildMvxGroupedDataConverter(Context, Attrs);

			expandableAdapter.ExpandableDataConverter = groupedDataConverter;
			_wrappedAdapter = _expandableItemManager.CreateWrappedAdapter(expandableAdapter);

			return _wrappedAdapter;
		}

		public override void AttachRecyclerView()
		{
			_expandableItemManager?.AttachRecyclerView(RecyclerView);
		}

		public override void Dispose()
		{
			_expandableItemManager?.Release();
			_expandableItemManager = null;
		}

		public override void RestoreFromBundle(Bundle bundle)
		{
			base.RestoreFromBundle(bundle);

			if (bundle.ContainsKey(ExpandManagerParcelableKey))
			{
				_expandCollapseSavedState = (IParcelable) bundle.GetParcelable(ExpandManagerParcelableKey);
			}
		}

		public override void SaveToBundle(Bundle bundle)
		{
			base.SaveToBundle(bundle);

			if (_expandableItemManager != null)
			{
				bundle.PutParcelable(
					ExpandManagerParcelableKey,
					_expandableItemManager.GetSavedState());
			}
		}
	}
}