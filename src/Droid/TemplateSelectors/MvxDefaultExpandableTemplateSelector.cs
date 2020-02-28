namespace MvvmCross.AdvancedRecyclerView.TemplateSelectors
{
	public class MvxDefaultExpandableTemplateSelector : MvxExpandableTemplateSelector
	{
		private readonly int _childLayoutId;

		public MvxDefaultExpandableTemplateSelector(int groupLayoutId, int childLayoutId) : base(groupLayoutId)
		{
			_childLayoutId = childLayoutId;
		}

		protected override int GetChildItemViewType(object forItemObject) => 0;

		protected override int GetChildItemLayoutId(int fromViewType) => _childLayoutId;
	}
}