using System;
using System.Threading.Tasks;

namespace HHG.Messages
{
    public interface IMessageAsync
    {
        #region Publish

        Task PublishAsync(object id);
        Task PublishAsync(object id, object message);

        #endregion

        #region Request

        Task<R[]> RequestAsync<R>(object message);
        Task<R[]> RequestAsync<R>(object id, object message);

        #endregion

        #region Subscribe (Publish)

        Task SubscribeAsync<T>(Action<T> callback);
        Task SubscribeAsync<T>(object id, Action<T> callback);
        Task UnsubscribeAsync<T>(Action<T> callback);
        Task UnsubscribeAsync<T>(object id, Action<T> callback);

        #endregion

        #region Subscribe (Request)

        Task SubscribeAsync<T, R>(Func<T, R> callback);
        Task SubscribeAsync<T, R>(object id, Func<T, R> callback);
        Task UnsubscribeAsync<T, R>(Func<T, R> callback);
        Task UnsubscribeAsync<T, R>(object id, Func<T, R> callback);

        #endregion
    }
}