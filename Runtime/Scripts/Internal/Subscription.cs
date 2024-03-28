using System;

namespace HHG.Messages.Runtime
{
    internal class Subscription : IComparable<Subscription>
    {
        public SubscriptionId SubscriptionId { get; private set; }
        private Delegate callback;
        private int sortOrder;

        public Subscription(SubscriptionId subscriptionId, Delegate wrappedCallback, int order)
        {
            SubscriptionId = subscriptionId;
            callback = wrappedCallback;
            sortOrder = order;
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

        public int CompareTo(Subscription other)
        {
            return sortOrder.CompareTo(other.sortOrder);
        }
    }
}