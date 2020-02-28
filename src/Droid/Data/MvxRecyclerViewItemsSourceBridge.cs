using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using MvvmCross.WeakSubscription;

namespace MvvmCross.AdvancedRecyclerView.Data
{
	internal class MvxExpandableGroupedItemsSourceProvider
	{
		private readonly ObservableCollection<object> _observableItemsSource = new ObservableCollection<object>();
		private readonly IList<IDisposable> _collectionChangedDisposables = new List<IDisposable>();

		public ObservableCollection<object> Source => _observableItemsSource;

		public void Initialize(IEnumerable groupedItems, MvxExpandableDataConverter expandableDataConverter)
		{
			_observableItemsSource.Clear();
			foreach (var disposables in _collectionChangedDisposables)
			{
				disposables.Dispose();
			}

			_collectionChangedDisposables.Clear();

			foreach (var mvxGroupable in groupedItems.Cast<object>().Select(expandableDataConverter.ConvertToMvxGroupedData))
			{
				_observableItemsSource.Add(mvxGroupable);
			}

			if (!(groupedItems is INotifyCollectionChanged observableGroups))
			{
				return;
			}

			var observableGroupsDisposeSubscription = observableGroups.WeakSubscribe(
				(sender, args) =>
				{
					switch (args.Action)
					{
						case NotifyCollectionChangedAction.Reset:
							_observableItemsSource.Clear();
							break;
						case NotifyCollectionChangedAction.Add:
							foreach (var item in args.NewItems.Cast<object>())
							{
								_observableItemsSource.Add(expandableDataConverter.ConvertToMvxGroupedData(item));
							}

							break;
						case NotifyCollectionChangedAction.Remove:
							foreach (var item in args.OldItems.Cast<object>())
							{
								var mvxGroupedData = expandableDataConverter.ConvertToMvxGroupedData(item);
								_observableItemsSource.Remove(mvxGroupedData);
								foreach (var childItem in mvxGroupedData.GroupItems)
								{
									_observableItemsSource.Remove(childItem);
								}
							}

							break;
						default:
							throw new InvalidOperationException("No move/replace in Grouped Items yet...");
					}
				});
			_collectionChangedDisposables.Add(observableGroupsDisposeSubscription);
		}
	}
}