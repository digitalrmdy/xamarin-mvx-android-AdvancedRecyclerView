using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Com.H6ah4i.Android.Widget.Advrecyclerview.Utils;
using MvvmCross.AdvancedRecyclerView.Data;
using MvvmCross.AdvancedRecyclerView.Data.EventArguments;
using MvvmCross.AdvancedRecyclerView.Utils;
using MvvmCross.AdvancedRecyclerView.ViewHolders;
using MvvmCross.Binding.Extensions;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Droid.Support.V7.RecyclerView.ItemTemplates;
using MvvmCross.Exceptions;
using MvvmCross.Logging;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using Object = Java.Lang.Object;

namespace MvvmCross.AdvancedRecyclerView.Adapters.Expandable
{
	public class MvxExpandableItemAdapter : AbstractExpandableItemAdapter, IMvxAdvancedRecyclerViewAdapter
	{
		private readonly MvxGroupedItemsSourceProvider _expandableGroupedItemsSourceProvider;
		private IEnumerable _itemsSource;

		public MvxExpandableItemAdapter() : this(MvxAndroidBindingContextHelpers.Current())
		{
		}

		public MvxExpandableItemAdapter(IMvxAndroidBindingContext androidBindingContext)
		{
			BindingContext = androidBindingContext;
			HasStableIds = true;
			_expandableGroupedItemsSourceProvider = new MvxGroupedItemsSourceProvider();
			_expandableGroupedItemsSourceProvider.Source.CollectionChanged += SourceOnCollectionChanged;
			_expandableGroupedItemsSourceProvider.ChildItemsAdded += SourceItemChildChanged;
			_expandableGroupedItemsSourceProvider.ChildItemsRemoved += SourceItemChildChanged;
			_expandableGroupedItemsSourceProvider.ChildItemsCollectionCleared += (group) => base.NotifyDataSetChanged();
			_expandableGroupedItemsSourceProvider.ItemsMovedOrReplaced += () => base.NotifyDataSetChanged();
		}

		protected MvxExpandableItemAdapter(IntPtr javaReference, JniHandleOwnership transfer)
			: base(javaReference, transfer)
		{
		}

		protected IMvxAndroidBindingContext BindingContext { get; }

		public IMvxTemplateSelector TemplateSelector { get; set; }

		private ObservableCollection<object> GroupedItems => _expandableGroupedItemsSourceProvider.Source;

		public IEnumerable ItemsSource
		{
			get => _itemsSource;
			set
			{
				if (ReferenceEquals(_itemsSource, value))
				{
					return;
				}

				_itemsSource = value;
				_expandableGroupedItemsSourceProvider.Initialize(_itemsSource, ExpandableDataConverter);
				NotifyDataSetChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			}
		}

		public MvxExpandableDataConverter ExpandableDataConverter { get; set; }

		public MvxGroupExpandController GroupExpandController { get; internal set; } = new DefaultMvxGroupExpandController();

		public override int GroupCount => GroupedItems.Count;

		public ICommand GroupItemClickCommand { get; set; }

		public ICommand GroupItemLongClickCommand { get; set; }

		public ICommand ChildItemClickCommand { get; set; }

		public ICommand ChildItemLongClickCommand { get; set; }

		private void SourceItemChildChanged(MvxGroupedData gropedData, IEnumerable newItems)
		{
			base.NotifyDataSetChanged();
		}

		private void SourceOnCollectionChanged(object sender,
			NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
		{
			NotifyDataSetChanged(notifyCollectionChangedEventArgs);
		}

		public virtual void NotifyDataSetChanged(NotifyCollectionChangedEventArgs e)
		{
			try
			{
				NotifyDataSetChanged();
			}
			catch (Exception exception)
			{
				Mvx.IoCProvider.Resolve<IMvxLog>().Log(MvxLogLevel.Warn, () =>
					$"Exception masked during Adapter RealNotifyDataSetChanged {exception.ToLongString()}. Are you trying to update your collection from a background task? See http://goo.gl/0nW0L6"
				);
			}
		}

		public override bool OnCheckCanExpandOrCollapseGroup(Object holder, int groupPosition, int x, int y, bool expand)
		{
			var groupItemDetails = new MvxGroupDetails
			{
				Holder = holder as RecyclerView.ViewHolder,
				Item = GroupedItems.ElementAt(groupPosition) as MvxGroupedData,
				GroupIndex = groupPosition
			};

			if (expand)
			{
				return GroupExpandController.CanExpandGroup(groupItemDetails);
			}

			return GroupExpandController.CanCollapseGroup(groupItemDetails);
		}

		public override Object OnCreateChildViewHolder(ViewGroup parent, int viewType)
		{
			var itemBindingContext = new MvxAndroidBindingContext(parent.Context, BindingContext.LayoutInflaterHolder);

			var viewHolder =
				new MvxExpandableRecyclerViewHolder(
					InflateViewForHolder(TemplateSelector.GetItemLayoutId(viewType), parent, viewType, itemBindingContext), itemBindingContext)
				{
					Click = ChildItemClickCommand,
					LongClick = ChildItemLongClickCommand
				};

			return viewHolder;
		}

		public override Object OnCreateGroupViewHolder(ViewGroup parent, int viewType)
		{
			var itemBindingContext = new MvxAndroidBindingContext(parent.Context, BindingContext.LayoutInflaterHolder);

			var viewHolder =
				new MvxExpandableRecyclerViewHolder(
					InflateViewForHolder(TemplateSelector.GetItemLayoutId(viewType), parent, viewType, itemBindingContext), itemBindingContext)
				{
					Click = GroupItemClickCommand,
					LongClick = GroupItemLongClickCommand
				};

			return viewHolder;
		}

		public override bool OnHookGroupCollapse(int p0, bool p1)
			=> GroupExpandController.OnHookGroupCollapse(new MvxHookGroupExpandCollapseArgs
			{
				DataContext = GetItemAt(p0),
				GroupPosition = p0,
				RequestedByUser = p1
			});

		public override bool OnHookGroupExpand(int p0, bool p1)
			=> GroupExpandController.OnHookGroupExpand(new MvxHookGroupExpandCollapseArgs
			{
				DataContext = GetItemAt(p0),
				GroupPosition = p0,
				RequestedByUser = p1
			});

		protected virtual View InflateViewForHolder(int layoutId, ViewGroup parent,
			int viewType,
			IMvxAndroidBindingContext bindingContext)
		{
			return bindingContext.BindingInflate(layoutId, parent, false);
		}

		public override void OnBindChildViewHolder(Object viewHolder, int groupPosition, int childPosition, int viewType)
		{
			var dataContext = GetItemAt(groupPosition, childPosition);
			((IMvxRecyclerViewHolder) viewHolder).DataContext = dataContext;
			OnChildItemBound(new MvxExpandableItemAdapterBoundedArgs
			{
				ViewHolder = viewHolder as MvxExpandableRecyclerViewHolder,
				DataContext = dataContext
			});
		}

		public override void OnBindGroupViewHolder(Object viewHolder, int groupPosition, int viewType)
		{
			var dataContext = GetItemAt(groupPosition);
			((IMvxRecyclerViewHolder) viewHolder).DataContext = dataContext;
			OnGroupItemBound(new MvxExpandableItemAdapterBoundedArgs
			{
				ViewHolder = viewHolder as MvxExpandableRecyclerViewHolder,
				DataContext = dataContext
			});
		}

		public override int GetChildCount(int p0)
		{
			if (GroupedItems.Count <= p0)
			{
				return 0;
			}

			return ((MvxGroupedData) GroupedItems.ElementAt(p0)).GroupItems.Count();
		}

		public override long GetChildId(int p0, int p1)
		{
			var childItem = GetItemAt(p0, p1);

			return ExpandableDataConverter.GetItemUniqueId(childItem);
		}

		public override long GetGroupId(int p0)
		{
			var mvxGroupedData = GetItemAt(p0);

			return ExpandableDataConverter.GetItemUniqueId(mvxGroupedData);
		}

		private MvxGroupedData GetItemAt(int groupIndex)
			=> GroupedItems.ElementAt(groupIndex) as MvxGroupedData;

		private object GetItemAt(int groupIndex, int childIndex)
			=> GetItemAt(groupIndex).GroupItems.ElementAt(childIndex);

		public event Action<MvxExpandableItemAdapterBoundedArgs> GroupItemBound;

		public event Action<MvxExpandableItemAdapterBoundedArgs> ChildItemBound;

		public override int GetGroupItemViewType(int p0) => TemplateSelector.GetItemViewType(GetItemAt(p0));

		public override int GetChildItemViewType(int p0, int p1) => TemplateSelector.GetItemViewType(GetItemAt(p0, p1));

		protected virtual void OnGroupItemBound(MvxExpandableItemAdapterBoundedArgs obj)
		{
			GroupItemBound?.Invoke(obj);
		}

		protected virtual void OnChildItemBound(MvxExpandableItemAdapterBoundedArgs obj)
		{
			ChildItemBound?.Invoke(obj);
		}
	}
}