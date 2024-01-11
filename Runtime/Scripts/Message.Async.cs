using System;
using System.Threading.Tasks;

namespace HHG.Messages
{
    public partial class Message
    {
        public static IMessageAsync AsyncProvider { get; set; } = new DefaultAsync();

        #region Publish

        public static Task PublishAsync(object message) => AsyncProvider.PublishAsync(message);
        public static Task PublishAsync(object id, object message) => AsyncProvider.PublishAsync(id, message);

        #endregion

        #region Request

        public static Task<R[]> RequestAsync<R>(object message) => AsyncProvider.RequestAsync<R>(message);
        public static Task<R[]> RequestAsync<R>(object id, object message) => AsyncProvider.RequestAsync<R>(id, message);

        #endregion

        #region Subscribe (Publish)

        public static Task SubscribeAsync<T>(Action<T> callback) => AsyncProvider.SubscribeAsync(callback);
        public static Task SubscribeAsync<T>(object id, Action<T> callback) => AsyncProvider.SubscribeAsync(id, callback);
        public static Task UnsubscribeAsync<T>(Action<T> callback) => AsyncProvider.UnsubscribeAsync(callback);
        public static Task UnsubscribeAsync<T>(object id, Action<T> callback) => AsyncProvider.UnsubscribeAsync(id, callback);

        #endregion

        #region Subscribe (Request)

        public static Task SubscribeAsync<T, R>(Func<T, R> callback) => AsyncProvider.SubscribeAsync(callback);
        public static Task SubscribeAsync<T, R>(object id, Func<T, R> callback) => AsyncProvider.SubscribeAsync(id, callback);
        public static Task UnsubscribeAsync<T, R>(Func<T, R> callback) => AsyncProvider.UnsubscribeAsync(callback);
        public static Task UnsubscribeAsync<T, R>(object id, Func<T, R> callback) => AsyncProvider.UnsubscribeAsync(id, callback);

        #endregion
    }
}