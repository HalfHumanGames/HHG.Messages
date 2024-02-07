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

        public static R Publish<R>(IRequest<R> message) => Provider.Publish(message);
        public static R Publish<R>(object id, IRequest<R> message, PublishMode mode = PublishMode.Broadcast) => Provider.Publish(id, message, mode);
        public static R PublishTo<R>(object id, IRequest<R> message) => Provider.PublishTo(id, message);

        #endregion

        #region Aggregate

        public static R Publish<R>(IAggregate<R> message) => Provider.Publish(message);
        public static R Publish<R>(object id, IAggregate<R> message, PublishMode mode = PublishMode.Broadcast) => Provider.Publish(id, message, mode);
        public static R PublishTo<R>(object id, IAggregate<R> message) => Provider.PublishTo(id, message);

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