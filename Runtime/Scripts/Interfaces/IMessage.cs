using System;

namespace HHG.Messages.Runtime
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

        R Publish<R>(IRequest<R> request);
        R Publish<R>(object id, IRequest<R> request, PublishMode mode = PublishMode.Broadcast);
        R PublishTo<R>(object id, IRequest<R> request);

        #endregion

        #region Aggregate

        R Publish<R>(IAggregate<R> aggregate);
        R Publish<R>(object id, IAggregate<R> aggregate, PublishMode mode = PublishMode.Broadcast);
        R PublishTo<R>(object id, IAggregate<R> aggregate);
        A Publish<S, A>(IAggregate<S, A> aggregate);
        A Publish<S, A>(object id, IAggregate<S, A> aggregate, PublishMode mode = PublishMode.Broadcast);
        A PublishTo<S, A>(object id, IAggregate<S, A> aggregate);
        R Publish<S, A, R>(IAggregate<S, A, R> aggregate);
        R Publish<S, A, R>(object id, IAggregate<S, A, R> aggregate, PublishMode mode = PublishMode.Broadcast);
        R PublishTo<S, A, R>(object id, IAggregate<S, A, R> aggregate);

        #endregion

        #region Subscribe (Publish)

        void Subscribe<T>(Action<T> callback, int order = 0);
        void Subscribe<T>(object id, Action<T> callback, int order = 0);
        void Unsubscribe<T>(Action<T> callback);
        void Unsubscribe<T>(object id, Action<T> callback);

        #endregion

        #region Subscribe (Publish)

        void Subscribe<T, R>(Func<T, R> callback, int order = 0);
        void Subscribe<T, R>(object id, Func<T, R> callback, int order = 0);
        void Unsubscribe<T, R>(Func<T, R> callbacl);
        void Unsubscribe<T, R>(object id, Func<T, R> callback);

        #endregion
    }
}