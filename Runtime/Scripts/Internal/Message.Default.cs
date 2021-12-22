using HHG.Common;
using System;
using System.Collections.Generic;

namespace HHG.Messages
{
    public partial class Message
    {
        internal class Default : IMessage
        {
            private static readonly Dictionary<SubjectId, List<Subscription>> actionSubscriptions = new Dictionary<SubjectId, List<Subscription>>();
            private static readonly Dictionary<SubjectId, List<Subscription>> funcSubscriptions = new Dictionary<SubjectId, List<Subscription>>();

            #region Action Publishing

            public void Publish(object id)
            {
                Publish<object>(id, null);
            }

            public void Publish<T>()
            {
                T message = Activator.CreateInstance<T>();
                Publish(message);
            }

            public void Publish<T>(object id)
            {
                T message = Activator.CreateInstance<T>();
                Publish(id, message);
            }

            public void Publish<T>(T message)
            {
                if (typeof(T).IsPrimitive || typeof(T) == typeof(string))
                {
                    Publish((object) message);
                } 
                else
                {
                    Publish(null, message);
                }
            }

            public void Publish<T>(object id, T message)
            {
                SubjectId subjectId = new SubjectId(typeof(T), id);

                if (!actionSubscriptions.ContainsKey(subjectId))
                {
                    return;
                }

                foreach (Subscription subscription in actionSubscriptions[subjectId])
                {
                    subscription.InvokeAction(message);
                }

                if (id != null)
                {
                    Publish(null, message);
                }
            }

            #endregion

            #region Func Publishing

            public R[] Publish<T, R>()
            {
                T message = Activator.CreateInstance<T>();
                return Publish<T, R>(message);
            }

            public R[] Publish<T, R>(object id)
            {
                T message = Activator.CreateInstance<T>();
                return Publish<T, R>(id, message);
            }

            public R[] Publish<T, R>(T message)
            {
                return Publish<T, R>(null, message);
            }

            public R[] Publish<T, R>(object id, T message)
            {
                SubjectId subjectId = new SubjectId(typeof(T), id);

                if (!funcSubscriptions.ContainsKey(subjectId))
                {
                    return default;
                }

                int global = 0;
                int size = funcSubscriptions[subjectId].Count;
                if (id != null)
                {
                    SubjectId nullSubjectId = new SubjectId(typeof(T), null);
                    global = funcSubscriptions[nullSubjectId].Count;
                    size += global;
                }

                R[] retval = new R[size];

                int i = 0;
                foreach (Subscription subscription in funcSubscriptions[subjectId])
                {
                    retval[i++] = (R)subscription.InvokeFunc(message);
                }

                if (id != null && global > 0)
                {
                    Array.Copy(Publish<T, R>(null, message), 0, retval, i, global);
                }

                return retval;
            }

            #endregion

            #region Action Subscriptions

            public void Subscribe(object id, Action callback)
            {
                SubscribeInternal<object>(id, callback);
            }

            public void Subscribe<T>(Action<T> callback)
            {
                SubscribeInternal<T>(null, callback);
            }

            public void Subscribe<T>(object id, Action<T> callback)
            {
                SubscribeInternal<T>(id, callback);
            }

            protected void SubscribeInternal<T>(object id, Delegate callback)
            {
                Action<object> wrappedCallback = default;// = arg => callback((T)arg);
                if (callback is Action action)
                {
                    wrappedCallback = arg => action();
                }
                else if (callback is Action<T> actionWithParam)
                {
                    wrappedCallback = arg => actionWithParam((T)arg);
                }
                SubjectId subjectId = new SubjectId(typeof(T), id);
                SubscriptionId subscriptionId = new SubscriptionId(subjectId, callback);
                Subscription subscription = new Subscription(subscriptionId, wrappedCallback);

                if (!actionSubscriptions.ContainsKey(subjectId))
                {
                    actionSubscriptions.Add(subjectId, new List<Subscription>());
                }

                if (!actionSubscriptions[subjectId].Contains(subscription))
                {
                    actionSubscriptions[subjectId].Add(subscription);
                }
            }

            public void Unsubscribe(object id, Action callback)
            {
                UnsubscribeInternal<object>(id, callback);
            }

            public void Unsubscribe<T>(Action<T> callback)
            {
                UnsubscribeInternal<T>(null, callback);
            }

            public void Unsubscribe<T>(object id, Action<T> callback)
            {
                UnsubscribeInternal<T>(id, callback);
            }

            public void UnsubscribeInternal<T>(object id, Delegate callback)
            {
                //Action<object> wrappedCallback = arg => callback((T)arg);
                SubjectId subjectId = new SubjectId(typeof(T), id);
                SubscriptionId subscriptionId = new SubscriptionId(subjectId, callback);

                if (!actionSubscriptions.ContainsKey(subjectId))
                {
                    return;
                }

                for (int i = 0; i < actionSubscriptions[subjectId].Count; i++)
                {
                    if (actionSubscriptions[subjectId][i].SubscriptionId == subscriptionId)
                    {
                        actionSubscriptions[subjectId].RemoveAt(i);
                        break;
                    }
                }
            }

            #endregion

            #region Func Subscriptions

            public void Subscribe<T, R>(Func<T, R> callback)
            {
                Subscribe(null, callback);
            }

            public void Subscribe<T, R>(object id, Func<T, R> callback)
            {
                Func<object, object> wrappedCallback = args => callback((T)args);
                SubjectId subjectId = new SubjectId(typeof(T), id);
                SubscriptionId subscriptionId = new SubscriptionId(subjectId, callback);
                Subscription subscription = new Subscription(subscriptionId, wrappedCallback);

                if (!funcSubscriptions.ContainsKey(subjectId))
                {
                    funcSubscriptions.Add(subjectId, new List<Subscription>());
                }

                if (!funcSubscriptions[subjectId].Contains(subscription))
                {
                    funcSubscriptions[subjectId].Add(subscription);
                }
            }

            public void Unsubscribe<T, R>(Func<T, R> callback)
            {
                Unsubscribe(null, callback);
            }

            public void Unsubscribe<T, R>(object id, Func<T, R> callback)
            {
                Func<object, object> wrappedCallback = args => callback((T)args);
                SubjectId subjectId = new SubjectId(typeof(T), id);
                SubscriptionId subscriptionId = new SubscriptionId(subjectId, callback);

                if (!funcSubscriptions.ContainsKey(subjectId))
                {
                    return;
                }

                for (int i = 0; i < funcSubscriptions[subjectId].Count; i++)
                {
                    if (funcSubscriptions[subjectId][i].SubscriptionId == subscriptionId)
                    {
                        funcSubscriptions[subjectId].RemoveAt(i);
                        break;
                    }
                }
            }

            #endregion
        }
    }
}