using System;

namespace HHG.Messages
{
    public interface IMessage
    {
        #region Publish

        void Publish(object message);
        void Publish(object id, object message, PublishMode mode = PublishMode.Broadcast);
        void PublishTo(object id, object message);

        #endregion

        #region Publish

        R[] Publish<R>(object message);
        R[] Publish<R>(object id, object message, PublishMode mode = PublishMode.Broadcast);
        R[] PublishTo<R>(object id, object message);

        #endregion

        #region Request

        R Publish<R>(IRequest<R> message);
        R Publish<R>(object id, IRequest<R> message, PublishMode mode = PublishMode.Broadcast);
        R PublishTo<R>(object id, IRequest<R> message);

        #endregion

        #region Subscribe (Publish)

        void Subscribe<T>(Action<T> callback);
        void Subscribe<T>(object id, Action<T> callback);
        void Unsubscribe<T>(Action<T> callback);
        void Unsubscribe<T>(object id, Action<T> callback);

        #endregion

        #region Subscribe (Publish)

        void Subscribe<T, R>(Func<T, R> callback);
        void Subscribe<T, R>(object id, Func<T, R> callback);
        void Unsubscribe<T, R>(Func<T, R> callbacl);
        void Unsubscribe<T, R>(object id, Func<T, R> callback);

        #endregion
    }
}