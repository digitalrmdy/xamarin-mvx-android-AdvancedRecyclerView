using System.Collections;

namespace MvvmCross.AdvancedRecyclerView.Data
{
	public class MvxGroupedData
	{
		public object Key { get; set; }

		public IEnumerable GroupItems { get; set; }

		public long UniqueId { get; set; }

		public override bool Equals(object obj)
		{
			return obj is MvxGroupedData other && other.UniqueId == UniqueId;
		}

		public override int GetHashCode()
		{
			return UniqueId.GetHashCode();
		}
	}
}