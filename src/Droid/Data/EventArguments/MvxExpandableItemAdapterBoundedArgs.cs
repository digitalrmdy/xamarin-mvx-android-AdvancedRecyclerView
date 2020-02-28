using MvvmCross.AdvancedRecyclerView.ViewHolders;

namespace MvvmCross.AdvancedRecyclerView.Data.EventArguments
{
    public class MvxExpandableItemAdapterBoundedArgs
    {
        public MvxExpandableRecyclerViewHolder ViewHolder { get; internal set; }
 
        public object DataContext { get; internal set; }
    }
}