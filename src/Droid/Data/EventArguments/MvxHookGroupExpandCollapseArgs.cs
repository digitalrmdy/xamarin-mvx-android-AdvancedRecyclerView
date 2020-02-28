namespace MvvmCross.AdvancedRecyclerView.Data.EventArguments
{
    public class MvxHookGroupExpandCollapseArgs
    {
        public int GroupPosition { get; internal set; }

        public MvxGroupedData DataContext { get; internal set; }
        public bool RequestedByUser { get; internal set; }
    }
}