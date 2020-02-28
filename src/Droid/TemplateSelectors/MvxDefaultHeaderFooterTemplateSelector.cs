using MvvmCross.Droid.Support.V7.RecyclerView.ItemTemplates;

namespace MvvmCross.AdvancedRecyclerView.TemplateSelectors
{
	public class MvxDefaultHeaderFooterTemplateSelector : MvxDefaultTemplateSelector, IMvxFooterTemplate, IMvxHeaderTemplate
	{
		public MvxDefaultHeaderFooterTemplateSelector(int itemTemplateId) : base(itemTemplateId)
		{
		}

		public MvxDefaultHeaderFooterTemplateSelector()
		{
		}

		public int FooterLayoutId { get; set; }
		public int HeaderLayoutId { get; set; }
	}
}