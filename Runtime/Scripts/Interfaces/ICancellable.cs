namespace HHG.Messages
{
    public interface ICancellable
    {
        bool IsCancelled { get; set; }
        void Cancel() => IsCancelled = true;
        void Reset() => IsCancelled = false;
    }

    // Extension method required since Unity does not
    // fully support default implementations just yet
    public static class CancellableExtensions
    {
        public static void Cancel(this ICancellable cancellable) => cancellable.Cancel();
    }
}