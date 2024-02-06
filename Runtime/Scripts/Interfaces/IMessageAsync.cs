using System;
using System.Threading.Tasks;

namespace HHG.Messages
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

        Task<R> PublishAsync<R>(IRequest<R> message);
        Task<R> PublishAsync<R>(object id, IRequest<R> message, PublishMode mode = PublishMode.Broadcast);
        Task<R> PublishToAsync<R>(object id, IRequest<R> message);

        #endregion

        #region Subscribe (Publish)

        Task SubscribeAsync<T>(Action<T> callback);
        Task SubscribeAsync<T>(object id, Action<T> callback);
        Task UnsubscribeAsync<T>(Action<T> callback);
        Task UnsubscribeAsync<T>(object id, Action<T> callback);

        #endregion

        #region Subscribe (Publish)

        Task SubscribeAsync<T, R>(Func<T, R> callback);
        Task SubscribeAsync<T, R>(object id, Func<T, R> callback);
        Task UnsubscribeAsync<T, R>(Func<T, R> callback);
        Task UnsubscribeAsync<T, R>(object id, Func<T, R> callback);

        #endregion
    }
}