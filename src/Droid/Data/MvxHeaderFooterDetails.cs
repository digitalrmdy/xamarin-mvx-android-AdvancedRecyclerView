namespace MvvmCross.AdvancedRecyclerView.Data
{
	public class MvxHeaderFooterDetails
	{
		public int HeaderLayoutId { get; set; }

		public int FooterLayoutId { get; set; }

		public bool HasHeader => HeaderLayoutId != 0;

		public bool HasFooter => FooterLayoutId != 0;
	}
}