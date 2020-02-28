﻿using System;
using Android.Content;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Util;
using Com.H6ah4i.Android.Widget.Advrecyclerview.Utils;
using MvvmCross.AdvancedRecyclerView.Extensions;
using MvvmCross.AdvancedRecyclerView.TemplateSelectors;
using MvvmCross.Droid.Support.V7.RecyclerView.ItemTemplates;
using MvvmCross.Platforms.Android.Binding.BindingContext;

namespace MvvmCross.AdvancedRecyclerView.Adapters
{
    public abstract class MvxAdvancedRecyclerViewAdapterController : IDisposable
    {
        protected readonly Context Context;
        protected readonly IAttributeSet Attrs;
        protected readonly RecyclerView RecyclerView;
        protected readonly IMvxAndroidBindingContext BindingContext;
        private RecyclerView.Adapter _wrappedAdapter;
        
        protected MvxAdvancedRecyclerViewAdapterController(Context context, IAttributeSet attrs, RecyclerView recyclerView, IMvxAndroidBindingContext bindingContext)
        {
            Context = context;
            Attrs = attrs;
            RecyclerView = recyclerView;
            BindingContext = bindingContext;
        }

        public RecyclerView.Adapter BuildAdapter()
        {
            var templateSelector = BuildTemplateSelector();
            _wrappedAdapter = BuildWrappedAdapter(templateSelector);

            return HeaderFooterWrapperAdapter = new MvxHeaderFooterWrapperAdapter(_wrappedAdapter, BindingContext)
            {
                HeaderFooterDetails = BuildHeaderFooterDetails(templateSelector)
            };
        }
        
        private Data.MvxHeaderFooterDetails BuildHeaderFooterDetails(IMvxTemplateSelector templateSelector)
        {
            var headerFooterDetails = new Data.MvxHeaderFooterDetails();
            var headerTemplate = templateSelector as IMvxHeaderTemplate;
            var footerTemplate = templateSelector as IMvxFooterTemplate;

            if (headerTemplate != null)
            {
                headerFooterDetails.HeaderLayoutId = headerTemplate.HeaderLayoutId;
            }

            if (footerTemplate != null)
            {
                headerFooterDetails.FooterLayoutId = footerTemplate.FooterLayoutId;
            }

            return headerFooterDetails;
        }

        protected virtual IMvxTemplateSelector BuildTemplateSelector() => MvxAdvancedRecyclerViewAttributeExtensions.BuildItemTemplateSelector(Context, Attrs);

        protected abstract RecyclerView.Adapter BuildWrappedAdapter(IMvxTemplateSelector templateSelector);

        public abstract void AttachRecyclerView();
        
        public IMvxAdvancedRecyclerViewAdapter AdvancedRecyclerViewAdapter { get; protected set; }
        
        public MvxHeaderFooterWrapperAdapter HeaderFooterWrapperAdapter { get; private set; }

        public virtual void RestoreFromBundle(Bundle bundle)
        {
        }

        public virtual void SaveToBundle(Bundle bundle)
        {
        }
        
        public virtual void Dispose()
        {
            if (_wrappedAdapter != null)
            {
                WrapperAdapterUtils.ReleaseAll(_wrappedAdapter);
            }

            _wrappedAdapter = null;
        }
    }
}
