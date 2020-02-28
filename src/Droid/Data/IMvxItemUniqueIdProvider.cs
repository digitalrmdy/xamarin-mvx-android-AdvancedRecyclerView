namespace MvvmCross.AdvancedRecyclerView.Data
{
	public interface IMvxItemUniqueIdProvider
	{
		long GetUniqueId(object fromObject);
	}
}