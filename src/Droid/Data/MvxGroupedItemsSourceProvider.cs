using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using MvvmCross.WeakSubscription;

namespace MvvmCross.AdvancedRecyclerView.Data
{
	internal class MvxGroupedItemsSourceProvider
	{
		private readonly ObservableCollection<object> _observableItemsSource = new ObservableCollection<object>();
		private readonly Dictionary<MvxGroupedData, IDisposable> _groupedDataDisposables = new Dictionary<MvxGroupedData, IDisposable>();

		public ObservableCollection<object> Source => _observableItemsSource;

		private MvxExpandableDataConverter _groupedDataConverter;
		private INotifyCollectionChanged _oldGroupedDataSource;

		private void ObservableGroups_CollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
		{
			switch (args.Action)
			{
				case NotifyCollectionChangedAction.Reset:
					var enumerableGroupedItems = sender as IEnumerable ?? Enumerable.Empty<object>();
					_observableItemsSource.Clear();
					foreach (var disposables in _groupedDataDisposables.Values)
					{
						disposables.Dispose();
					}

					_groupedDataDisposables.Clear();
					AddItems(enumerableGroupedItems, _groupedDataConverter);
					break;
				case NotifyCollectionChangedAction.Add:
					AddItems(args.NewItems, _groupedDataConverter);
					break;
				case NotifyCollectionChangedAction.Remove:
					foreach (var item in args.OldItems.Cast<object>())
					{
						var mvxGroupedData = _groupedDataConverter.ConvertToMvxGroupedData(item);

						_observableItemsSource.Remove(mvxGroupedData);

						if (!_groupedDataDisposables.ContainsKey(mvxGroupedData))
						{
							continue;
						}

						_groupedDataDisposables[mvxGroupedData].Dispose();
						_groupedDataDisposables.Remove(mvxGroupedData);
					}

					break;
				default:
					ItemsMovedOrReplaced?.Invoke();
					break;
			}
		}

		public void Initialize(IEnumerable groupedItems, MvxExpandableDataConverter groupedDataConverter)
		{
			DisposeOldSource();
			this._groupedDataConverter = groupedDataConverter;
			var observableGroups = groupedItems as INotifyCollectionChanged;
			_oldGroupedDataSource = observableGroups;

			if (observableGroups != null)
			{
				observableGroups.CollectionChanged += ObservableGroups_CollectionChanged;
			}

			AddItems(groupedItems, groupedDataConverter);
		}

		private void DisposeOldSource()
		{
			if (_oldGroupedDataSource != null)
			{
				_oldGroupedDataSource.CollectionChanged -= ObservableGroups_CollectionChanged;
			}

			_oldGroupedDataSource = null;

			foreach (var disposable in _groupedDataDisposables.Values)
			{
				disposable.Dispose();
			}

			_groupedDataDisposables.Clear();
			_observableItemsSource.Clear();
		}

		private void AddItems(IEnumerable groupedItems, MvxExpandableDataConverter groupedDataConverter)
		{
			foreach (var mvxGroupable in groupedItems.Cast<object>().Select(groupedDataConverter.ConvertToMvxGroupedData))
			{
				if (!_observableItemsSource.Contains(mvxGroupable))
				{
					_observableItemsSource.Add(mvxGroupable);
				}

				var childNotifyCollectionChanged = mvxGroupable.GroupItems as INotifyCollectionChanged;

				if (childNotifyCollectionChanged == null)
				{
					continue;
				}

				if (_groupedDataDisposables.ContainsKey(mvxGroupable))
				{
					continue;
				}

				void CollectionChangedHandler(object sender, NotifyCollectionChangedEventArgs args)
				{
					switch (args.Action)
					{
						case NotifyCollectionChangedAction.Add:
							AddChildItems(mvxGroupable, args.NewItems);
							break;
						case NotifyCollectionChangedAction.Remove:
							RemoveChildItems(mvxGroupable, args.OldItems);
							break;
						case NotifyCollectionChangedAction.Reset:
							ResetChildCollection(mvxGroupable);
							break;
						case NotifyCollectionChangedAction.Replace:
							break;
						case NotifyCollectionChangedAction.Move:
							break;
						default:
							ItemsMovedOrReplaced?.Invoke();
							break;
					}
				}

				_groupedDataDisposables.Add(mvxGroupable,
					new EventHandlerWeakSubscriptionHolder(
						CollectionChangedHandler,
						childNotifyCollectionChanged.WeakSubscribe(CollectionChangedHandler)
					)
				);
			}
		}

		private void AddChildItems(MvxGroupedData toGroup, IEnumerable items)
		{
			ChildItemsAdded?.Invoke(toGroup, items);
		}

		private void RemoveChildItems(MvxGroupedData toGroup, IEnumerable itemsToRemove)
		{
			ChildItemsRemoved?.Invoke(toGroup, itemsToRemove);
		}

		private void ResetChildCollection(MvxGroupedData ofGroupedData)
		{
			ChildItemsCollectionCleared?.Invoke(ofGroupedData);
		}

		public event Action<MvxGroupedData, IEnumerable> ChildItemsAdded;
		public event Action<MvxGroupedData, IEnumerable> ChildItemsRemoved;
		public event Action<MvxGroupedData> ChildItemsCollectionCleared;
		public event Action ItemsMovedOrReplaced;
	}
}