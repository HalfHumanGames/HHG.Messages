using System;

namespace HHG.Messages
{
    public interface IMessage
    {
        #region Action Publishing

        void Publish(object id);
        void Publish<T>();
        void Publish<T>(object id);
        void Publish<T>(T message);
        void Publish<T>(object id, T message);

        #endregion

        #region Func Publishing

        R[] Publish<T, R>();
        R[] Publish<T, R>(object id);
        R[] Publish<T, R>(T message);
        R[] Publish<T, R>(object id, T message);

        #endregion

        #region Action Subscriptions

        void Subscribe(object id, Action callback);
        void Subscribe<T>(Action<T> callback);
        void Subscribe<T>(object id, Action<T> callback);
        void Unsubscribe(object id, Action callback);
        void Unsubscribe<T>(Action<T> handler);
        void Unsubscribe<T>(object id, Action<T> callback);

        #endregion

        #region Func Subscriptions

        void Subscribe<T, R>(Func<T, R> callback);
        void Subscribe<T, R>(object id, Func<T, R> callback);
        void Unsubscribe<T, R>(Func<T, R> handler);
        void Unsubscribe<T, R>(object id, Func<T, R> callback);

        #endregion
    }
}