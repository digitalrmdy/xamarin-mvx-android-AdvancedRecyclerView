namespace MvvmCross.AdvancedRecyclerView.Data
{
	public abstract class MvxExpandableDataConverter
	{
		public abstract MvxGroupedData ConvertToMvxGroupedData(object item);

		public long GetItemUniqueId(object item)
		{
			return (item as MvxGroupedData)?.UniqueId
			       ?? GetChildItemUniqueId(item);
		}

		protected abstract long GetChildItemUniqueId(object item);
	}
}