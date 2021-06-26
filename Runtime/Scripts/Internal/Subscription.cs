using System;

namespace HHG.Messages
{
    internal class ActionSubscription
    {
        public SubscriptionId SubscriptionId { get; private set; }
        private Action<object> callback;

        public ActionSubscription(SubscriptionId subscriptionId, Action<object> wrappedCallback)
        {
            SubscriptionId = subscriptionId;
            callback = wrappedCallback;
        }

        public void Invoke(object message)
        {
            callback(message);
        }
    }

    internal class FuncSubscription
    {
        public SubscriptionId SubscriptionId { get; private set; }
        private Func<object, object> callback;

        public FuncSubscription(SubscriptionId subscriptionId, Func<object, object> wrappedCallback)
        {
            SubscriptionId = subscriptionId;
            callback = wrappedCallback;
        }

        public object Invoke(object message)
        {
            return callback(message);
        }
    }
}