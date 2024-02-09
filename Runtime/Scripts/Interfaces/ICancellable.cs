using System;

namespace HHG.Messages
{
    public interface ICancellable
    {
        CancellationToken CancellationToken { get; }

        void Cancel() => CancellationToken.Cancel();

        event Action Completed
        {
            add => CancellationToken.Completed += value;
            remove => CancellationToken.Completed -= value;
        }

        event Action Cancelled
        {
            add => CancellationToken.Cancelled += value;
            remove => CancellationToken.Cancelled -= value;
        }
    }

    // Extension method required since Unity does not
    // fully support default implementations just yet
    public static class CancellableExtensions
    {
        public static void Cancel(this ICancellable cancellable) => cancellable.Cancel();
        public static void OnCancelled(this ICancellable cancellable, Action action) => cancellable.Completed += action;
        public static void OnCompleted(this ICancellable cancellable, Action action) => cancellable.Completed += action;
    }
}