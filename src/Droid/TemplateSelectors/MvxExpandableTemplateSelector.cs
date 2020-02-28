using MvvmCross.AdvancedRecyclerView.Data;
using MvvmCross.Droid.Support.V7.RecyclerView.ItemTemplates;

namespace MvvmCross.AdvancedRecyclerView.TemplateSelectors
{
	public abstract class MvxExpandableTemplateSelector : IMvxTemplateSelector
	{
		private const int GroupViewType = 12345276;
		private readonly int _groupLayoutId;

		protected MvxExpandableTemplateSelector(int groupLayoutId)
		{
			this._groupLayoutId = groupLayoutId;
		}

		public int GetItemViewType(object forItemObject)
		{
			return forItemObject is MvxGroupedData
				? GroupViewType
				: GetChildItemViewType(forItemObject);
		}

		public int GetItemLayoutId(int fromViewType)
		{
			return fromViewType == GroupViewType
				? _groupLayoutId
				: GetChildItemLayoutId(fromViewType);
		}

		public int ItemTemplateId { get; set; }

		protected abstract int GetChildItemViewType(object forItemObject);

		protected abstract int GetChildItemLayoutId(int fromViewType);
	}
}