using System;

namespace HHG.Messages.Runtime
{
    public struct Subscription : IDisposable
    {
        internal readonly SubscriptionId subscriptionId;

        internal Subscription(SubscriptionId subscriptionId)
        {
            this.subscriptionId = subscriptionId;
        }

        public void Dispose()
        {
            Message.Unsubscribe(this);
        }
    }
}