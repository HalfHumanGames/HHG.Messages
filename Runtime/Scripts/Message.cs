using System;

namespace HHG.Messages
{
    public partial class Message
    {
        public static IMessage Provider { get; set; } = new Default();

        #region Publish

        public static void Publish(object message) => Provider.Publish(message);
        public static void Publish(object id, object message, PublishMode mode = PublishMode.Broadcast) => Provider.Publish(id, message, mode);
        public static void PublishTo(object id, object message) => Provider.PublishTo(id, message);

        #endregion

        #region Publish

        public static R[] Publish<R>(object message) => Provider.Publish<R>(message);
        public static R[] Publish<R>(object id, object message, PublishMode mode = PublishMode.Broadcast) => Provider.Publish<R>(id, message, mode);
        public static R[] PublishTo<R>(object id, object message) => Provider.PublishTo<R>(id, message);

        #endregion

        #region Request

        public static R Publish<R>(IRequest<R> request) => Provider.Publish(request);
        public static R Publish<R>(object id, IRequest<R> request, PublishMode mode = PublishMode.Broadcast) => Provider.Publish(id, request, mode);
        public static R PublishTo<R>(object id, IRequest<R> request) => Provider.PublishTo(id, request);

        #endregion

        #region Aggregate

        public static R Publish<R>(IAggregate<R> aggregate) => Provider.Publish(aggregate);
        public static R Publish<R>(object id, IAggregate<R> aggregate, PublishMode mode = PublishMode.Broadcast) => Provider.Publish(id, aggregate, mode);
        public static R PublishTo<R>(object id, IAggregate<R> aggregate) => Provider.PublishTo(id, aggregate);
        public static A Publish<S, A>(IAggregate<S, A> aggregate) => Provider.Publish(aggregate);
        public static A Publish<S, A>(object id, IAggregate<S, A> aggregate, PublishMode mode = PublishMode.Broadcast) => Provider.Publish(id, aggregate, mode);
        public static A PublishTo<S, A>(object id, IAggregate<S, A> aggregate) => Provider.PublishTo(id, aggregate);
        public static R Publish<S, A, R>(IAggregate<S, A, R> aggregate) => Provider.Publish(aggregate);
        public static R Publish<S, A, R>(object id, IAggregate<S, A, R> aggregate, PublishMode mode = PublishMode.Broadcast) => Provider.Publish(id, aggregate, mode);
        public static R PublishTo<S, A, R>(object id, IAggregate<S, A, R> aggregate) => Provider.PublishTo(id, aggregate);

        #endregion

        #region Select

        public static R Publish<S, R>(Func<S, R> selector) => Provider.Publish(selector);
        public static R Publish<S, R>(object id, Func<S, R> selector, PublishMode mode = PublishMode.Broadcast) => Provider.Publish(id, selector, mode);
        public static R PublishTo<S, R>(object id, Func<S, R> selector) => Provider.Publish(id, selector);

        #endregion

        #region Subscribe (Select)

        public static void Subscribe<T>(T source, int order = 0) => Provider.Subscribe(source, order);
        public static void Subscribe<T>(object id, T source, int order = 0) => Provider.Subscribe(id, source, order);

        #endregion

        #region Subscribe (Action)

        public static void Subscribe<T>(Action<T> callback, int order = 0) => Provider.Subscribe(callback, order);
        public static void Subscribe<T>(object id, Action<T> callback, int order = 0) => Provider.Subscribe(id, callback, order);
        public static void Unsubscribe<T>(Action<T> callback) => Provider.Unsubscribe(callback);
        public static void Unsubscribe<T>(object id, Action<T> callback) => Provider.Unsubscribe(id, callback);

        #endregion

        #region Subscribe (Func)

        public static void Subscribe<T, R>(Func<T, R> callback, int order = 0) => Provider.Subscribe(callback, order);
        public static void Subscribe<T, R>(object id, Func<T, R> callback, int order = 0) => Provider.Subscribe(id, callback, order);
        public static void Unsubscribe<T, R>(Func<T, R> callback) => Provider.Unsubscribe(callback);
        public static void Unsubscribe<T, R>(object id, Func<T, R> callback) => Provider.Unsubscribe(id, callback);

        #endregion
    }
}