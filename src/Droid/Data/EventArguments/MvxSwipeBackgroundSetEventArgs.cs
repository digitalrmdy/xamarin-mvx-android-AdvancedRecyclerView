﻿using MvvmCross.AdvancedRecyclerView.ViewHolders;

namespace MvvmCross.AdvancedRecyclerView.Data.EventArguments
{
    public class MvxSwipeBackgroundSetEventArgs
    {
        public MvxAdvancedRecyclerViewHolder ViewHolder { get; internal set; }

        public int Position { get; internal set; }

        public int Type { get; internal set; }
    }
}
