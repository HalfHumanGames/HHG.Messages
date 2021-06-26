using System;

namespace HHG.Messages
{
    public interface IMessage
    {
        #region Action Publishing

        void Publish<T>() where T : class;
        void Publish<T>(object id) where T : class;
        void Publish<T>(T message) where T : class;
        void Publish<T>(object id, T message) where T : class;

        #endregion

        #region Func Publishing

        R[] Publish<T, R>() where T : class;
        R[] Publish<T, R>(object id) where T : class;
        R[] Publish<T, R>(T message) where T : class;
        R[] Publish<T, R>(object id, T message) where T : class;

        #endregion

        #region Action Subscriptions

        void Subscribe<T>(Action<T> callback) where T : class;
        void Subscribe<T>(object id, Action<T> callback) where T : class;
        void Unsubscribe<T>(Action<T> handler) where T : class;
        void Unsubscribe<T>(object id, Action<T> callback) where T : class;

        #endregion

        #region Func Subscriptions

        void Subscribe<T, R>(Func<T, R> callback) where T : class;
        void Subscribe<T, R>(object id, Func<T, R> callback) where T : class;
        void Unsubscribe<T, R>(Func<T, R> handler) where T : class;
        void Unsubscribe<T, R>(object id, Func<T, R> callback) where T : class;

        #endregion
    }
}