using System;
using System.Threading.Tasks;

namespace HHG.Messages
{
    public partial class Message
    {
        internal partial class DefaultAsync : IMessageAsync
        {
            private Default message = new Default();

            #region Action Publishing

            public Task PublishAsync<T>() where T : class
            {
                message.Publish<T>();
                return Task.CompletedTask;
            }

            public Task PublishAsync<T>(object id) where T : class
            {
                message.Publish<T>(id);
                return Task.CompletedTask;
            }

            public Task PublishAsync<T>(T message) where T : class
            {
                this.message.Publish(message);
                return Task.CompletedTask;
            }

            public Task PublishAsync<T>(object id, T message) where T : class
            {
                this.message.Publish(id, message);
                return Task.CompletedTask;
            }

            #endregion

            #region Func Publishing

            public Task<R[]> PublishAsync<T, R>() where T : class
            {
                return Task.FromResult(message.Publish<T, R>());
            }

            public Task<R[]> PublishAsync<T, R>(object id) where T : class
            {
                return Task.FromResult(message.Publish<T, R>(id));
            }

            public Task<R[]> PublishAsync<T, R>(T message) where T : class
            {
                return Task.FromResult(this.message.Publish<T, R>(message));
            }

            public Task<R[]> PublishAsync<T, R>(object id, T message) where T : class
            {
                return Task.FromResult(this.message.Publish<T, R>(id, message));
            }

            #endregion

            #region Action Subscriptions

            public Task SubscribeAsync<T>(Action<T> callback) where T : class
            {
                message.Subscribe(callback);
                return Task.CompletedTask;
            }

            public Task SubscribeAsync<T>(object id, Action<T> callback) where T : class
            {
                message.Subscribe(id, callback);
                return Task.CompletedTask;
            }

            public Task UnsubscribeAsync<T>(Action<T> callback) where T : class
            {
                message.Unsubscribe(callback);
                return Task.CompletedTask;
            }

            public Task UnsubscribeAsync<T>(object id, Action<T> callback) where T : class
            {
                message.Unsubscribe(id, callback);
                return Task.CompletedTask;
            }

            #endregion

            #region Func Subscriptions

            public Task SubscribeAsync<T, R>(Func<T, R> callback) where T : class
            {
                message.Subscribe(callback);
                return Task.CompletedTask;
            }

            public Task SubscribeAsync<T, R>(object id, Func<T, R> callback) where T : class
            {
                message.Subscribe(id, callback);
                return Task.CompletedTask;
            }

            public Task UnsubscribeAsync<T, R>(Func<T, R> callback) where T : class
            {
                message.Unsubscribe(callback);
                return Task.CompletedTask;
            }

            public Task UnsubscribeAsync<T, R>(object id, Func<T, R> callback) where T : class
            {
                message.Unsubscribe(id, callback);
                return Task.CompletedTask;
            }

            #endregion
        }
    }
}