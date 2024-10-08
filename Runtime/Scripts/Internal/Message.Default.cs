using HHG.Common.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HHG.Messages.Runtime
{
    public partial class Message
    {
        internal class Default : IMessage
        {
            private static readonly Dictionary<SubjectId, List<Subscription>> actionSubscriptions = new Dictionary<SubjectId, List<Subscription>>();
            private static readonly Dictionary<SubjectId, List<Subscription>> funcSubscriptions = new Dictionary<SubjectId, List<Subscription>>();

            #region Publish

            public void Publish(object message)
            {
                Publish(null, message);
            }

            public void Publish(object id, object message, PublishMode mode = PublishMode.Broadcast)
            {
                SubjectId subjectId = new SubjectId(message.GetType(), id);
                PublishInternal(subjectId, message, mode);
            }

            private void PublishInternal(SubjectId subjectId, object message, PublishMode mode = PublishMode.Broadcast)
            {
                if (!actionSubscriptions.TryGetValue(subjectId, out List<Subscription> subscriptions))
                {
                    return;
                }

                for (int i = 0; i < subscriptions.Count; i++)
                {
                    Subscription subscription = subscriptions[i];
                    subscription.InvokeAction(message);

                    if (message is ICancellable cancellable && cancellable.CancellationToken.IsCancelled)
                    {
                        cancellable.CancellationToken.Reset();
                        return;
                    }
                }

                if (subjectId.Id != null && mode == PublishMode.Broadcast)
                {
                    Publish(null, message);
                }

                if (message is ICancellable cancellable2)
                {
                    cancellable2.CancellationToken.Complete();
                }
            }

            public void PublishTo(object id, object message)
            {
                Publish(id, message, PublishMode.Narrowcast);
            }

            #endregion

            #region Publish

            public R[] Publish<R>(object message)
            {
                return Publish<R>(null, message);
            }

            public R[] Publish<R>(object id, object message, PublishMode mode = PublishMode.Broadcast)
            {
                SubjectId subjectId = new SubjectId(message.GetType(), id);
                return PublishInternal<R>(subjectId, message, mode);
            }

            private R[] PublishInternal<R>(SubjectId subjectId, object message, PublishMode mode = PublishMode.Broadcast)
            {
                if (!funcSubscriptions.TryGetValue(subjectId, out List<Subscription> subscriptions))
                {
                    return new R[0];
                }

                int global = 0;
                int size = subscriptions.Count;
                if (subjectId.Id != null && mode == PublishMode.Broadcast)
                {
                    SubjectId nullSubjectId = new SubjectId(subjectId.Type, null);
                    global = funcSubscriptions[nullSubjectId].Count;
                    size += global;
                }

                R[] retval = new R[size];

                int index = 0;
                for (int i = 0; i < subscriptions.Count; i++)
                {
                    Subscription subscription = subscriptions[i];
                    retval[index++] = (R)subscription.InvokeFunc(message);

                    if (message is ICancellable cancellable && cancellable.CancellationToken.IsCancelled)
                    {
                        cancellable.CancellationToken.Reset();
                        Array.Resize(ref retval, index);
                        return retval;
                    }
                }

                if (subjectId.Id != null && global > 0)
                {
                    Array.Copy(Publish<R>(null, message), 0, retval, index, global);
                }

                if (message is ICancellable cancellable2)
                {
                    cancellable2.CancellationToken.Complete();
                }

                return retval;
            }

            public R[] PublishTo<R>(object id, object message)
            {
                return Publish<R>(id, message, PublishMode.Narrowcast);
            }

            #endregion

            #region Request

            public R Publish<R>(IRequest<R> request)
            {
                Publish((object)request);
                return request.Response;
            }

            public R Publish<R>(object id, IRequest<R> request, PublishMode mode = PublishMode.Broadcast)
            {
                Publish(id, (object)request, mode);
                return request.Response;
            }

            public R PublishTo<R>(object id, IRequest<R> request)
            {
                PublishTo(id, (object)request);
                return request.Response;
            }

            #endregion

            #region Aggregate

            public S Publish<S>(IAggregate<S> aggregate)
            {
                S[] results = Publish<S>((object)aggregate);
                return results.Aggregate(aggregate.Aggregate);
            }

            public S Publish<S>(object id, IAggregate<S> aggregate, PublishMode mode = PublishMode.Broadcast)
            {
                S[] results = Publish<S>(id, (object)aggregate, mode);
                return results.Aggregate(aggregate.Aggregate);
            }

            public S PublishTo<S>(object id, IAggregate<S> aggregate)
            {
                S[] results = PublishTo<S>(id, (object)aggregate);
                return results.Aggregate(aggregate.Aggregate);
            }

            public A Publish<S, A>(IAggregate<S, A> aggregate)
            {
                S[] results = Publish<S>(aggregate);
                return results.Aggregate(aggregate.GetSeed(), aggregate.Aggregate);
            }

            public A Publish<S, A>(object id, IAggregate<S, A> aggregate, PublishMode mode = PublishMode.Broadcast)
            {
                S[] results = Publish<S>(id, aggregate, mode);
                return results.Aggregate(aggregate.GetSeed(), aggregate.Aggregate);
            }

            public A PublishTo<S, A>(object id, IAggregate<S, A> aggregate)
            {
                S[] results = PublishTo<S>(id, aggregate);
                return results.Aggregate(aggregate.GetSeed(), aggregate.Aggregate);
            }

            public R Publish<S, A, R>(IAggregate<S, A, R> aggregate)
            {
                S[] results = Publish<S>(aggregate);
                return results.Aggregate(aggregate.GetSeed(), aggregate.Aggregate, aggregate.GetResult);
            }

            public R Publish<S, A, R>(object id, IAggregate<S, A, R> aggregate, PublishMode mode = PublishMode.Broadcast)
            {
                S[] results = Publish<S>(id, aggregate, mode);
                return results.Aggregate(aggregate.GetSeed(), aggregate.Aggregate, aggregate.GetResult);
            }

            public R PublishTo<S, A, R>(object id, IAggregate<S, A, R> aggregate)
            {
                S[] results = PublishTo<S>(id, aggregate);
                return results.Aggregate(aggregate.GetSeed(), aggregate.Aggregate, aggregate.GetResult);
            }

            #endregion

            #region Subscribe (Publishes)


            public void Subscribe<T>(Action<T> callback, int order = 0)
            {
                SubscribeInternal<T>(null, callback, order);
            }

            public void Subscribe<T>(object id, Action<T> callback, int order = 0)
            {
                SubscribeInternal<T>(id, callback, order);
            }

            protected void SubscribeInternal<T>(object id, Delegate callback, int order = 0)
            {
                Action<object> wrappedCallback = default;
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
                Subscription subscription = new Subscription(subscriptionId, wrappedCallback, order);

                if (!actionSubscriptions.TryGetValue(subjectId, out List<Subscription> subscriptions))
                {
                    actionSubscriptions.Add(subjectId, new List<Subscription>());
                }

                if (!subscriptions.Contains(subscription))
                {
                    subscriptions.Add(subscription);
                    subscriptions.Sort();
                }
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
                SubjectId subjectId = new SubjectId(typeof(T), id);
                UnsubscribeInternal<T>(subjectId, callback);
            }

            public void UnsubscribeInternal<T>(SubjectId subjectId, Delegate callback)
            {
                SubscriptionId subscriptionId = new SubscriptionId(subjectId, callback);

                if (!actionSubscriptions.TryGetValue(subjectId, out List<Subscription> subscriptions))
                {
                    return;
                }

                for (int i = 0; i < subscriptions.Count; i++)
                {
                    if (subscriptions[i].SubscriptionId == subscriptionId)
                    {
                        subscriptions.RemoveAt(i);
                        break;
                    }
                }
            }

            #endregion

            #region Subscribe (Publishs)

            public void Subscribe<T, R>(Func<T, R> callback, int order = 0)
            {
                Subscribe(null, callback, order);
            }

            public void Subscribe<T, R>(object id, Func<T, R> callback, int order = 0)
            {
                Func<object, object> wrappedCallback = args => callback((T)args);
                SubjectId subjectId = new SubjectId(typeof(T), id);
                SubscriptionId subscriptionId = new SubscriptionId(subjectId, callback);
                Subscription subscription = new Subscription(subscriptionId, wrappedCallback, order);

                if (!funcSubscriptions.TryGetValue(subjectId, out List<Subscription> subscriptions))
                {
                    funcSubscriptions.Add(subjectId, new List<Subscription>());
                }

                if (!subscriptions.Contains(subscription))
                {
                    subscriptions.Add(subscription);
                    subscriptions.Sort();
                }
            }

            public void Unsubscribe<T, R>(Func<T, R> callback)
            {
                Unsubscribe(null, callback);
            }

            public void Unsubscribe<T, R>(object id, Func<T, R> callback)
            {
                SubjectId subjectId = new SubjectId(typeof(T), id);
                UnsubscribeInternal(subjectId, callback);
            }

            public void UnsubscribeInternal<T, R>(SubjectId subjectId, Func<T, R> callback)
            {
                Func<object, object> wrappedCallback = args => callback((T)args);
                SubscriptionId subscriptionId = new SubscriptionId(subjectId, callback);

                if (!funcSubscriptions.TryGetValue(subjectId, out List<Subscription> subscriptions))
                {
                    return;
                }

                for (int i = 0; i < subscriptions.Count; i++)
                {
                    if (subscriptions[i].SubscriptionId == subscriptionId)
                    {
                        subscriptions.RemoveAt(i);
                        break;
                    }
                }
            }

            #endregion
        }
    }
}