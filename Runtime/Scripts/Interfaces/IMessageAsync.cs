using System;
using System.Threading.Tasks;

namespace HHG.Messages.Runtime
{
    public interface IMessageAsync
    {
        #region Publish

        Task PublishAsync(object id);
        Task PublishAsync(object id, object message, PublishMode mode = PublishMode.Broadcast);
        Task PublishToAsync(object id, object message);

        #endregion

        #region Publish

        Task<R[]> PublishAsync<R>(object message);
        Task<R[]> PublishAsync<R>(object id, object message, PublishMode mode = PublishMode.Broadcast);
        Task<R[]> PublishToAsync<R>(object id, object message);

        #endregion

        #region Request

        Task<R> PublishAsync<R>(IRequest<R> request);
        Task<R> PublishAsync<R>(object id, IRequest<R> request, PublishMode mode = PublishMode.Broadcast);
        Task<R> PublishToAsync<R>(object id, IRequest<R> request);

        #endregion

        #region Aggregate

        Task<S> PublishAsync<S>(IAggregate<S> aggregate);
        Task<S> PublishAsync<S>(object id, IAggregate<S> aggregate, PublishMode mode = PublishMode.Broadcast);
        Task<S> PublishToAsync<S>(object id, IAggregate<S> aggregate);
        Task<A> PublishAsync<S, A>(IAggregate<S, A> aggregate);
        Task<A> PublishAsync<S, A>(object id, IAggregate<S, A> aggregate, PublishMode mode = PublishMode.Broadcast);
        Task<A> PublishToAsync<S, A>(object id, IAggregate<S, A> aggregate);
        Task<R> PublishAsync<S, A, R>(IAggregate<S, A, R> aggregate);
        Task<R> PublishAsync<S, A, R>(object id, IAggregate<S, A, R> aggregate, PublishMode mode = PublishMode.Broadcast);
        Task<R> PublishToAsync<S, A, R>(object id, IAggregate<S, A, R> aggregate);

        #endregion

        #region Subscribe (Publish)

        Task<Subscription> SubscribeAsync<T>(Action<T> callback, int order = 0);
        Task<Subscription> SubscribeAsync<T>(object id, Action<T> callback, int order = 0);
        Task UnsubscribeAsync<T>(Action<T> callback);
        Task UnsubscribeAsync<T>(object id, Action<T> callback);
        Task UnsubscribeAsync(Subscription subscription);

        #endregion

        #region Subscribe (Publish)

        Task<Subscription> SubscribeAsync<T, R>(Func<T, R> callback, int order = 0);
        Task<Subscription> SubscribeAsync<T, R>(object id, Func<T, R> callback, int order = 0);
        Task UnsubscribeAsync<T, R>(Func<T, R> callback);
        Task UnsubscribeAsync<T, R>(object id, Func<T, R> callback);

        #endregion
    }
}