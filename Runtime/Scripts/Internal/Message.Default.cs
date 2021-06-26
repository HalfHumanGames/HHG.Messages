using HHG.Common;
using System;
using System.Collections.Generic;

namespace HHG.Messages
{
    public partial class Message
    {
        internal class Default : IMessage
        {
            private static readonly Dictionary<SubjectId, List<ActionSubscription>> actionSubscriptions = new Dictionary<SubjectId, List<ActionSubscription>>();
            private static readonly Dictionary<SubjectId, List<FuncSubscription>> funcSubscriptions = new Dictionary<SubjectId, List<FuncSubscription>>();

            #region Action Publishing

            public void Publish<T>() where T : class
            {
                T message = Activator.CreateInstance<T>();
                Publish(message);
            }

            public void Publish<T>(object id) where T : class
            {
                T message = Activator.CreateInstance<T>();
                Publish(id, message);
            }

            public void Publish<T>(T message) where T : class
            {
                Publish(null, message);
            }

            public void Publish<T>(object id, T message) where T : class
            {
                SubjectId subjectId = new SubjectId(typeof(T), id);

                if (!actionSubscriptions.ContainsKey(subjectId))
                {
                    return;
                }

                foreach (ActionSubscription subscription in actionSubscriptions[subjectId])
                {
                    subscription.Invoke(message);
                }

                if (id != null)
                {
                    Publish(null, message);
                }
            }

            #endregion

            #region Func Publishing

            public R[] Publish<T, R>() where T : class
            {
                T message = Activator.CreateInstance<T>();
                return Publish<T, R>(message);
            }

            public R[] Publish<T, R>(object id) where T : class
            {
                T message = Activator.CreateInstance<T>();
                return Publish<T, R>(id, message);
            }

            public R[] Publish<T, R>(T message) where T : class
            {
                return Publish<T, R>(null, message);
            }

            public R[] Publish<T, R>(object id, T message) where T : class
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
                foreach (FuncSubscription subscription in funcSubscriptions[subjectId])
                {
                    retval[i++] = (R)subscription.Invoke(message);
                }

                if (id != null && global > 0)
                {
                    Array.Copy(Publish<T, R>(null, message), 0, retval, i, global);
                }

                return retval;
            }

            #endregion

            #region Action Subscriptions

            public void Subscribe<T>(Action<T> callback) where T : class
            {
                Subscribe(null, callback);
            }

            public void Subscribe<T>(object id, Action<T> callback) where T : class
            {
                Action<object> wrappedCallback = args => callback((T)args);
                SubjectId subjectId = new SubjectId(typeof(T), id);
                SubscriptionId subscriptionId = new SubscriptionId(subjectId, callback);
                ActionSubscription subscription = new ActionSubscription(subscriptionId, wrappedCallback);

                if (!actionSubscriptions.ContainsKey(subjectId))
                {
                    actionSubscriptions.Add(subjectId, new List<ActionSubscription>());
                }

                if (!actionSubscriptions[subjectId].Contains(subscription))
                {
                    actionSubscriptions[subjectId].Add(subscription);
                }
            }

            public void Unsubscribe<T>(Action<T> callback) where T : class
            {
                Unsubscribe(null, callback);
            }

            public void Unsubscribe<T>(object id, Action<T> callback) where T : class
            {
                Action<object> wrappedCallback = args => callback((T)args);
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

            public void Subscribe<T, R>(Func<T, R> callback) where T : class
            {
                Subscribe(null, callback);
            }

            public void Subscribe<T, R>(object id, Func<T, R> callback) where T : class
            {
                Func<object, object> wrappedCallback = args => callback((T)args);
                SubjectId subjectId = new SubjectId(typeof(T), id);
                SubscriptionId subscriptionId = new SubscriptionId(subjectId, callback);
                FuncSubscription subscription = new FuncSubscription(subscriptionId, wrappedCallback);

                if (!funcSubscriptions.ContainsKey(subjectId))
                {
                    funcSubscriptions.Add(subjectId, new List<FuncSubscription>());
                }

                if (!funcSubscriptions[subjectId].Contains(subscription))
                {
                    funcSubscriptions[subjectId].Add(subscription);
                }
            }

            public void Unsubscribe<T, R>(Func<T, R> callback) where T : class
            {
                Unsubscribe(null, callback);
            }

            public void Unsubscribe<T, R>(object id, Func<T, R> callback) where T : class
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