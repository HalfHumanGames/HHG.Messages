using System;
using System.Threading.Tasks;

namespace HHG.Messages
{
    public partial class Message
    {
        public static IMessageAsync AsyncProvider { get; set; } = new DefaultAsync();

        #region Action Publishing

        public static Task PublishAsync(object id) => AsyncProvider.PublishAsync(id);
        public static Task PublishAsync<T>() => AsyncProvider.PublishAsync<T>();
        public static Task PublishAsync<T>(object id) => AsyncProvider.PublishAsync<T>(id);
        public static Task PublishAsync<T>(T message) => AsyncProvider.PublishAsync(message);
        public static Task PublishAsync<T>(object id, T message) => AsyncProvider.PublishAsync(id, message);

        #endregion

        #region Func Publishing

        public static Task<R[]> PublishAsync<T, R>() => AsyncProvider.PublishAsync<T, R>();
        public static Task<R[]> PublishAsync<T, R>(object id) => AsyncProvider.PublishAsync<T, R>(id);
        public static Task<R[]> PublishAsync<T, R>(T message) => AsyncProvider.PublishAsync<T, R>(message);
        public static Task<R[]> PublishAsync<T, R>(object id, T message) => AsyncProvider.PublishAsync<T, R>(id, message);

        #endregion

        #region Action Subscriptions

        public static Task SubscribeAsync<T>(object id, Action callback) => AsyncProvider.SubscribeAsync(id, callback);
        public static Task SubscribeAsync<T>(Action<T> callback) => AsyncProvider.SubscribeAsync(callback);
        public static Task SubscribeAsync<T>(object id, Action<T> callback) => AsyncProvider.SubscribeAsync(id, callback);
        public static Task UnsubscribeAsync<T>(object id, Action callback) => AsyncProvider.UnsubscribeAsync(id, callback);
        public static Task UnsubscribeAsync<T>(Action<T> callback) => AsyncProvider.UnsubscribeAsync(callback);
        public static Task UnsubscribeAsync<T>(object id, Action<T> callback) => AsyncProvider.UnsubscribeAsync(id, callback);

        #endregion

        #region Func Subscriptions

        public static Task SubscribeAsync<T, R>(Func<T, R> callback) => AsyncProvider.SubscribeAsync(callback);
        public static Task SubscribeAsync<T, R>(object id, Func<T, R> callback) => AsyncProvider.SubscribeAsync(id, callback);
        public static Task UnsubscribeAsync<T, R>(Func<T, R> callback) => AsyncProvider.UnsubscribeAsync(callback);
        public static Task UnsubscribeAsync<T, R>(object id, Func<T, R> callback) => AsyncProvider.UnsubscribeAsync(id, callback);

        #endregion
    }
}