using System;

namespace HHG.Messages
{
    internal class Subscription
    {
        public SubscriptionId SubscriptionId { get; private set; }
        private Delegate callback;

        public Subscription(SubscriptionId subscriptionId, Delegate wrappedCallback)
        {
            SubscriptionId = subscriptionId;
            callback = wrappedCallback;
        }

        public void InvokeAction(object message)
        {
            if (callback is Action action)
            {
                action();
            }
            else if (callback is Action<object> actionWithParam)
            {
                actionWithParam(message);
            }
        }

        public object InvokeFunc(object message)
        {
            if (callback is Func<object> func)
            {
                return func();
            }
            else if (callback is Func<object, object> funcWithParam)
            {
                return funcWithParam(message);
            }
            return default;
        }
    }
}