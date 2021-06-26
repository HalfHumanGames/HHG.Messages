using System;
using System.Threading.Tasks;

namespace HHG.Messages
{
    public partial class Message
    {
        public static IMessageAsync AsyncProvider { get; set; } = new DefaultAsync();

        #region Action Publishing

        public static Task PublishAsync<T>() where T : class => AsyncProvider.PublishAsync<T>();
        public static Task PublishAsync<T>(object id) where T : class => AsyncProvider.PublishAsync<T>(id);
        public static Task PublishAsync<T>(T message) where T : class => AsyncProvider.PublishAsync(message);
        public static Task PublishAsync<T>(object id, T message) where T : class => AsyncProvider.PublishAsync(id, message);

        #endregion

        #region Func Publishing

        public static Task<R[]> PublishAsync<T, R>() where T : class => AsyncProvider.PublishAsync<T, R>();
        public static Task<R[]> PublishAsync<T, R>(object id) where T : class => AsyncProvider.PublishAsync<T, R>(id);
        public static Task<R[]> PublishAsync<T, R>(T message) where T : class => AsyncProvider.PublishAsync<T, R>(message);
        public static Task<R[]> PublishAsync<T, R>(object id, T message) where T : class => AsyncProvider.PublishAsync<T, R>(id, message);

        #endregion

        #region Action Subscriptions

        public static Task SubscribeAsync<T>(Action<T> callback) where T : class => AsyncProvider.SubscribeAsync(callback);
        public static Task SubscribeAsync<T>(object id, Action<T> callback) where T : class => AsyncProvider.SubscribeAsync(id, callback);
        public static Task UnsubscribeAsync<T>(Action<T> callback) where T : class => AsyncProvider.UnsubscribeAsync(callback);
        public static Task UnsubscribeAsync<T>(object id, Action<T> callback) where T : class => AsyncProvider.UnsubscribeAsync(id, callback);

        #endregion

        #region Func Subscriptions

        public static Task SubscribeAsync<T, R>(Func<T, R> callback) where T : class => AsyncProvider.SubscribeAsync(callback);
        public static Task SubscribeAsync<T, R>(object id, Func<T, R> callback) where T : class => AsyncProvider.SubscribeAsync(id, callback);
        public static Task UnsubscribeAsync<T, R>(Func<T, R> callback) where T : class => AsyncProvider.UnsubscribeAsync(callback);
        public static Task UnsubscribeAsync<T, R>(object id, Func<T, R> callback) where T : class => AsyncProvider.UnsubscribeAsync(id, callback);

        #endregion
    }
}