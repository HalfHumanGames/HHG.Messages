using System;
using System.Threading.Tasks;

namespace HHG.Messages
{
    public partial class Message
    {
        internal partial class DefaultAsync : IMessageAsync
        {
            private Default message = new Default();

            #region Publish

            public Task PublishAsync(object message)
            {
                this.message.Publish(message);
                return Task.CompletedTask;
            }

            public Task PublishAsync(object id, object message)
            {
                this.message.Publish(id, message);
                return Task.CompletedTask;
            }

            #endregion

            #region Request

            public Task<R[]> RequestAsync<R>(object message)
            {
                return Task.FromResult(this.message.Request<R>(message));
            }

            public Task<R[]> RequestAsync<R>(object id, object message)
            {
                return Task.FromResult(this.message.Request<R>(id, message));
            }

            #endregion

            #region Subscribe (Publishes)

            public Task SubscribeAsync<T>(Action<T> callback)
            {
                message.Subscribe(callback);
                return Task.CompletedTask;
            }

            public Task SubscribeAsync<T>(object id, Action<T> callback)
            {
                message.Subscribe(id, callback);
                return Task.CompletedTask;
            }

            public Task UnsubscribeAsync<T>(Action<T> callback)
            {
                message.Unsubscribe(callback);
                return Task.CompletedTask;
            }

            public Task UnsubscribeAsync<T>(object id, Action<T> callback)
            {
                message.Unsubscribe(id, callback);
                return Task.CompletedTask;
            }

            #endregion

            #region Subscribe (Requests)

            public Task SubscribeAsync<T, R>(Func<T, R> callback)
            {
                message.Subscribe(callback);
                return Task.CompletedTask;
            }

            public Task SubscribeAsync<T, R>(object id, Func<T, R> callback)
            {
                message.Subscribe(id, callback);
                return Task.CompletedTask;
            }

            public Task UnsubscribeAsync<T, R>(Func<T, R> callback)
            {
                message.Unsubscribe(callback);
                return Task.CompletedTask;
            }

            public Task UnsubscribeAsync<T, R>(object id, Func<T, R> callback)
            {
                message.Unsubscribe(id, callback);
                return Task.CompletedTask;
            }

            #endregion
        }
    }
}