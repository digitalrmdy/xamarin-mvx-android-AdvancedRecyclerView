using Android.Support.V7.Widget;

namespace MvvmCross.AdvancedRecyclerView.Data
{
	public class MvxGroupDetails
	{
		public object Item { get; internal set; }

		public int GroupIndex { get; internal set; }

		public RecyclerView.ViewHolder Holder { get; internal set; }
	}
}