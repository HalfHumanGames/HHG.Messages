using System;

namespace HHG.Messages.Runtime
{
    internal class Handler : IComparable<Handler>
    {
        public HandlerId HandlerId { get; private set; }

        private Delegate handler;
        private int sortOrder;

        public Handler(HandlerId handlerId, Delegate wrappedHandler, int order)
        {
            HandlerId = handlerId;
            handler = wrappedHandler;
            sortOrder = order;
        }

        public void InvokeAction(object message)
        {
            if (handler is Action action)
            {
                action();
            }
            else if (handler is Action<object> actionWithParam)
            {
                actionWithParam(message);
            }
        }

        public object InvokeFunc(object message)
        {
            if (handler is Func<object> func)
            {
                return func();
            }
            else if (handler is Func<object, object> funcWithParam)
            {
                return funcWithParam(message);
            }
            return default;
        }

        public int CompareTo(Handler other)
        {
            return sortOrder.CompareTo(other.sortOrder);
        }
    }
}