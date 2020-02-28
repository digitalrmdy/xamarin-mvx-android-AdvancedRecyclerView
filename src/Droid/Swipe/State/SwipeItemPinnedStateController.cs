using System.Collections.Generic;
using MvvmCross.AdvancedRecyclerView.Data;

namespace MvvmCross.AdvancedRecyclerView.Swipe.State
{
	public class SwipeItemPinnedStateController
	{
		private readonly Dictionary<long, bool> _pinnedStateDictionary = new Dictionary<long, bool>();

		public IMvxItemUniqueIdProvider UniqueIdProvider { get; set; }

		public void SetPinnedState(object forItem, bool isPinned)
		{
			var itemId = UniqueIdProvider.GetUniqueId(forItem);
			if (!_pinnedStateDictionary.ContainsKey(itemId))
			{
				_pinnedStateDictionary.Add(itemId, false);
			}

			_pinnedStateDictionary[itemId] = isPinned;
		}

		public bool IsPinned(object item)
		{
			var itemId = UniqueIdProvider.GetUniqueId(item);
			return _pinnedStateDictionary.ContainsKey(itemId) && _pinnedStateDictionary[itemId];
		}

		public void ResetState() => _pinnedStateDictionary.Clear();
	}
}