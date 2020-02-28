﻿using System.Collections;

namespace MvvmCross.AdvancedRecyclerView.Adapters
{
    public interface IMvxAdvancedRecyclerViewAdapter
    {
        IEnumerable ItemsSource { get; set; }
    }
}
