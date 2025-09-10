using HHG.Common.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace HHG.Messages.Runtime
{
    public partial class Message
    {
        internal class Default : IMessage
        {
            private static readonly Dictionary<SubjectId, List<Handler>> actionHandlers = new Dictionary<SubjectId, List<Handler>>();
            private static readonly Dictionary<SubjectId, List<Handler>> funcHandlers = new Dictionary<SubjectId, List<Handler>>();

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

            public void PublishTo(object id, object message)
            {
                Publish(id, message, PublishMode.Narrowcast);
            }

            private void PublishInternal(SubjectId subjectId, object message, PublishMode mode = PublishMode.Broadcast)
            {
                if (!actionHandlers.TryGetValue(subjectId, out List<Handler> handlers))
                {
                    return;
                }

                for (int i = handlers.Count - 1; i >= 0; i--)
                {
                    Handler handler = handlers[i];
                    handler.InvokeAction(message);

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

            public R[] PublishTo<R>(object id, object message)
            {
                return Publish<R>(id, message, PublishMode.Narrowcast);
            }

            private R[] PublishInternal<R>(SubjectId subjectId, object message, PublishMode mode = PublishMode.Broadcast)
            {
                if (!funcHandlers.TryGetValue(subjectId, out List<Handler> handlers))
                {
                    return new R[0];
                }

                int global = 0;
                int size = handlers.Count;
                if (subjectId.Id != null && mode == PublishMode.Broadcast)
                {
                    SubjectId nullSubjectId = new SubjectId(subjectId.Type, null);
                    global = funcHandlers[nullSubjectId].Count;
                    size += global;
                }

                R[] retval = new R[size];

                int index = 0;
                for (int i = handlers.Count - 1; i >= 0; i--)
                {
                    Handler handler = handlers[i];
                    retval[index++] = (R)handler.InvokeFunc(message);

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

            #region Subscribe (Actions)

            public Subscription Subscribe<T>(Action<T> callback, int order = 0)
            {
                return SubscribeInternal<T>(null, callback, order);
            }

            public Subscription Subscribe<T>(object id, Action<T> callback, int order = 0)
            {
                return SubscribeInternal<T>(id, callback, order);
            }

            private Subscription SubscribeInternal<T>(object id, Delegate callback, int order = 0)
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
                Handler handler = new Handler(subscriptionId, wrappedCallback, order);

                if (!actionHandlers.TryGetValue(subjectId, out List<Handler> handlers))
                {
                    handlers = new List<Handler>();
                    actionHandlers.Add(subjectId, handlers);
                }

                if (!handlers.Contains(handler))
                {
                    handlers.Add(handler);
                    handlers.Sort();
                    handlers.Reverse(); // Since iterate backwards
                }

                return new Subscription(subscriptionId);
            }

            public void Unsubscribe<T>(Action<T> callback)
            {
                SubjectId subjectId = new SubjectId(typeof(T), null);
                SubscriptionId subscriptionId = new SubscriptionId(subjectId, callback);
                UnsubscribeInternal(subscriptionId);
            }

            public void Unsubscribe<T>(object id, Action<T> callback)
            {
                SubjectId subjectId = new SubjectId(typeof(T), id);
                SubscriptionId subscriptionId = new SubscriptionId(subjectId, callback);
                UnsubscribeInternal(subscriptionId);
            }

            #endregion

            #region Subscribe (Funcs)

            public Subscription Subscribe<T, R>(Func<T, R> callback, int order = 0)
            {
                return Subscribe(null, callback, order);
            }

            public Subscription Subscribe<T, R>(object id, Func<T, R> callback, int order = 0)
            {
                Func<object, object> wrappedCallback = args => callback((T)args);
                SubjectId subjectId = new SubjectId(typeof(T), id);
                SubscriptionId subscriptionId = new SubscriptionId(subjectId, callback);
                Handler handler = new Handler(subscriptionId, wrappedCallback, order);

                if (!funcHandlers.TryGetValue(subjectId, out List<Handler> handlers))
                {
                    handlers = new List<Handler>();
                    funcHandlers.Add(subjectId, handlers);
                }

                if (!handlers.Contains(handler))
                {
                    handlers.Add(handler);
                    handlers.Sort();
                    handlers.Reverse(); // Since iterate backwards
                }

                return new Subscription(subscriptionId);
            }

            public void Unsubscribe<T, R>(Func<T, R> callback)
            {
                SubjectId subjectId = new SubjectId(typeof(T), null);
                SubscriptionId subscriptionId = new SubscriptionId(subjectId, callback);
                UnsubscribeInternal(subscriptionId);
            }

            public void Unsubscribe<T, R>(object id, Func<T, R> callback)
            {
                SubjectId subjectId = new SubjectId(typeof(T), id);
                SubscriptionId subscriptionId = new SubscriptionId(subjectId, callback);
                UnsubscribeInternal(subscriptionId);
            }

            public void Unsubscribe(Subscription subscription)
            {
                UnsubscribeInternal(subscription.subscriptionId);
            }

            private void UnsubscribeInternal(SubscriptionId subscriptionId)
            {
                if (actionHandlers.TryGetValue(subscriptionId.SubjectId, out List<Handler> handlers))
                {
                    UnsubscribeInternalHelper(subscriptionId, handlers);
                }

                if (funcHandlers.TryGetValue(subscriptionId.SubjectId, out handlers))
                {
                    UnsubscribeInternalHelper(subscriptionId, handlers);
                }
            }

            private void UnsubscribeInternalHelper(SubscriptionId subscriptionId, List<Handler> handlers)
            {
                for (int i = handlers.Count - 1; i >= 0; i--)
                {
                    if (handlers[i].SubscriptionId == subscriptionId)
                    {
                        handlers.RemoveAt(i);
                        break;
                    }
                }
            }

            #endregion
        }
    }
}