using System;

namespace HHG.Messages
{
    internal class Subscription
    {
        public SubscriptionId SubscriptionId { get; private set; }
        private Action<object> callback;

        public Subscription(SubscriptionId subscriptionId, Action<object> wrappedCallback)
        {
            SubscriptionId = subscriptionId;
            callback = wrappedCallback;
        }

        public void Invoke(object message)
        {
            callback(message);
        }
    }
}