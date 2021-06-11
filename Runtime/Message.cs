using System;
using System.Collections.Generic;

namespace HHG.Messages
{
    public static class Message
    {
        private static readonly Dictionary<SubjectId, List<Subscription>> subscriptions = new Dictionary<SubjectId, List<Subscription>>();

        public static void Publish<T>(T message)
        {
            Publish(null, message);
        }

        public static void Publish<T>(object id, T message)
        {
            SubjectId subjectId = new SubjectId(typeof(T), id);

            if (!subscriptions.ContainsKey(subjectId))
            {
                return;
            }

            foreach (Subscription subscription in subscriptions[subjectId])
            {
                subscription.Invoke(message);
            }

            if (id != null)
            {
                Publish(null, message);
            }
        }

        public static void Subscribe<T>(Action<T> callback)
        {
            Subscribe(null, callback);
        }

        public static void Subscribe<T>(object id, Action<T> callback)
        {
            Action<object> wrappedCallback = args => callback((T) args);
            SubjectId subjectId = new SubjectId(typeof(T), id);
            SubscriptionId subscriptionId = new SubscriptionId(subjectId, callback);
            Subscription subscription = new Subscription(subscriptionId, wrappedCallback);
            
            if (!subscriptions.ContainsKey(subjectId))
            {
                subscriptions.Add(subjectId, new List<Subscription>());
            }

            if (!subscriptions[subjectId].Contains(subscription))
            {
                subscriptions[subjectId].Add(subscription);
            }
        }

        public static void Unsubscribe<T>(Action<T> handler)
        {
            Unsubscribe(null, handler);
        }

        public static void Unsubscribe<T>(object id, Action<T> callback)
        {
            Action<object> wrappedCallback = args => callback((T)args);
            SubjectId subjectId = new SubjectId(typeof(T), id);
            SubscriptionId subscriptionId = new SubscriptionId(subjectId, wrappedCallback);

            if (!subscriptions.ContainsKey(subjectId))
            {
                return;
            }

            for (int i = 0; i < subscriptions[subjectId].Count; i++)
            {
                if (subscriptions[subjectId][i].SubscriptionId == subscriptionId)
                {
                    subscriptions[subjectId].RemoveAt(i);
                    break;
                }
            }
        }
    }
}