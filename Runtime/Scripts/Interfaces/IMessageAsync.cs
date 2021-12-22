using System;
using System.Threading.Tasks;

namespace HHG.Messages
{
    public interface IMessageAsync
    {
        #region Action Publishing

        Task PublishAsync(object id);
        Task PublishAsync<T>();
        Task PublishAsync<T>(object id);
        Task PublishAsync<T>(T message);
        Task PublishAsync<T>(object id, T message);

        #endregion

        #region Func Publishing

        Task<R[]> PublishAsync<T, R>();
        Task<R[]> PublishAsync<T, R>(object id);
        Task<R[]> PublishAsync<T, R>(T message);
        Task<R[]> PublishAsync<T, R>(object id, T message);

        #endregion

        #region Action Subscriptions

        Task SubscribeAsync(object id, Action callback);
        Task SubscribeAsync<T>(Action<T> callback);
        Task SubscribeAsync<T>(object id, Action<T> callback);
        Task UnsubscribeAsync(object id, Action callback);
        Task UnsubscribeAsync<T>(Action<T> callback);
        Task UnsubscribeAsync<T>(object id, Action<T> callback);

        #endregion

        #region Func Subscriptions

        Task SubscribeAsync<T, R>(Func<T, R> callback);
        Task SubscribeAsync<T, R>(object id, Func<T, R> callback);
        Task UnsubscribeAsync<T, R>(Func<T, R> callback);
        Task UnsubscribeAsync<T, R>(object id, Func<T, R> callback);

        #endregion
    }
}