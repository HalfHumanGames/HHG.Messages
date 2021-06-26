using System;
using System.Threading.Tasks;

namespace HHG.Messages
{
    public interface IMessageAsync
    {
        #region Action Publishing

        Task PublishAsync<T>() where T : class;
        Task PublishAsync<T>(object id) where T : class;
        Task PublishAsync<T>(T message) where T : class;
        Task PublishAsync<T>(object id, T message) where T : class;

        #endregion

        #region Func Publishing

        Task<R[]> PublishAsync<T, R>() where T : class;
        Task<R[]> PublishAsync<T, R>(object id) where T : class;
        Task<R[]> PublishAsync<T, R>(T message) where T : class;
        Task<R[]> PublishAsync<T, R>(object id, T message) where T : class;

        #endregion

        #region Action Subscriptions

        Task SubscribeAsync<T>(Action<T> callback) where T : class;
        Task SubscribeAsync<T>(object id, Action<T> callback) where T : class;
        Task UnsubscribeAsync<T>(Action<T> handler) where T : class;
        Task UnsubscribeAsync<T>(object id, Action<T> callback) where T : class;

        #endregion

        #region Func Subscriptions

        Task SubscribeAsync<T, R>(Func<T, R> callback) where T : class;
        Task SubscribeAsync<T, R>(object id, Func<T, R> callback) where T : class;
        Task UnsubscribeAsync<T, R>(Func<T, R> handler) where T : class;
        Task UnsubscribeAsync<T, R>(object id, Func<T, R> callback) where T : class;

        #endregion
    }
}