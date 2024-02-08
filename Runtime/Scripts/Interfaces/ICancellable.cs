namespace HHG.Messages
{
    public interface ICancellable
    {
        void Cancel() => IsCancelled = true;

        internal bool IsCancelled { get; set; }

        internal void Reset() => IsCancelled = false;
    }

    // Extension method required since Unity does not
    // fully support default implementations just yet
    public static class CancellableExtensions
    {
        public static void Cancel(this ICancellable cancellable)
        {
            cancellable.Cancel();
        }
    }
}