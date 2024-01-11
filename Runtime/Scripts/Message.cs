using System;

namespace HHG.Messages
{
    public partial class Message
    {
        public static IMessage Provider { get; set; } = new Default();

        #region Publish

        public static void Publish(object message) => Provider.Publish(message);
        public static void Publish(object id, object message) => Provider.Publish(id, message);

        #endregion

        #region Request

        public static R[] Request<R>(object message) => Provider.Request<R>(message);
        public static R[] Request<R>(object id, object message) => Provider.Request<R>(id, message);

        #endregion

        #region Subscribe (Publish)

        public static void Subscribe<T>(Action<T> callback) => Provider.Subscribe(callback);
        public static void Subscribe<T>(object id, Action<T> callback) => Provider.Subscribe(id, callback);
        public static void Unsubscribe<T>(Action<T> callback) => Provider.Unsubscribe(callback);
        public static void Unsubscribe<T>(object id, Action<T> callback) => Provider.Unsubscribe(id, callback);

        #endregion

        #region Subscribe (Request)

        public static void Subscribe<T, R>(Func<T, R> callback) => Provider.Subscribe(callback);
        public static void Subscribe<T, R>(object id, Func<T, R> callback) => Provider.Subscribe(id, callback);
        public static void Unsubscribe<T, R>(Func<T, R> callback) => Provider.Unsubscribe(callback);
        public static void Unsubscribe<T, R>(object id, Func<T, R> callback) => Provider.Unsubscribe(id, callback);

        #endregion
    }
}