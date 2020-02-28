using System;
using System.Collections.Specialized;

namespace MvvmCross.AdvancedRecyclerView.Data
{
	internal class EventHandlerWeakSubscriptionHolder : IDisposable
	{
		private readonly EventHandler<NotifyCollectionChangedEventArgs> _handler;
		private readonly IDisposable _disposable;

		public EventHandlerWeakSubscriptionHolder(EventHandler<NotifyCollectionChangedEventArgs> handler, IDisposable disposable)
		{
			_handler = handler;
			_disposable = disposable;
		}

		public void Dispose() => _disposable.Dispose();
	}
}