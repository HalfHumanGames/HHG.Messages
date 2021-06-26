using System;

namespace HHG.Messages
{
    public partial class Message
    {
        public static IMessage Provider { get; set; } = new Default();

        #region Action Publishing

        public static void Publish<T>() where T : class => Provider.Publish<T>();
        public static void Publish<T>(object id) where T : class => Provider.Publish<T>(id);
        public static void Publish<T>(T message) where T : class => Provider.Publish(message);
        public static void Publish<T>(object id, T message) where T : class => Provider.Publish(id, message);

        #endregion

        #region Func Publishing

        public static R[] Publish<T, R>() where T : class => Provider.Publish<T, R>();
        public static R[] Publish<T, R>(object id) where T : class => Provider.Publish<T, R>(id);
        public static R[] Publish<T, R>(T message) where T : class => Provider.Publish<T, R>(message);
        public static R[] Publish<T, R>(object id, T message) where T : class => Provider.Publish<T, R>(id, message);

        #endregion

        #region Action Subscriptions

        public static void Subscribe<T>(Action<T> callback) where T : class => Provider.Subscribe(callback);
        public static void Subscribe<T>(object id, Action<T> callback) where T : class => Provider.Subscribe(id, callback);
        public static void Unsubscribe<T>(Action<T> callback) where T : class => Provider.Unsubscribe(callback);
        public static void Unsubscribe<T>(object id, Action<T> callback) where T : class => Provider.Unsubscribe(id, callback);

        #endregion

        #region Func Subscriptions

        public static void Subscribe<T, R>(Func<T, R> callback) where T : class => Provider.Subscribe(callback);
        public static void Subscribe<T, R>(object id, Func<T, R> callback) where T : class => Provider.Subscribe(id, callback);
        public static void Unsubscribe<T, R>(Func<T, R> callback) where T : class => Provider.Unsubscribe(callback);
        public static void Unsubscribe<T, R>(object id, Func<T, R> callback) where T : class => Provider.Unsubscribe(id, callback);

        #endregion
    }
}