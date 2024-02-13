using System;

namespace HHG.Messages
{
    public class CancellationToken
    {
        internal bool IsCancelled { get; private set; }

        internal event Action Cancelled;
        internal event Action Completed;

        internal void Cancel()
        {
            Cancelled?.Invoke();
            IsCancelled = true;
            Completed = null;
            Cancelled = null;
        }

        internal void Complete()
        {
            Completed?.Invoke();
            Reset();
        }

        internal void Reset()
        {
            IsCancelled = false;
            Completed = null;
            Cancelled = null;
        }
    }
}