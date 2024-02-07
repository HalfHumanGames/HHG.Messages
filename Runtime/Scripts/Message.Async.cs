using System;
using System.Threading.Tasks;

namespace HHG.Messages
{
    public partial class Message
    {
        public static IMessageAsync AsyncProvider { get; set; } = new DefaultAsync();

        #region Publish

        public static Task PublishAsync(object message) => AsyncProvider.PublishAsync(message);
        public static Task PublishAsync(object id, object message, PublishMode mode = PublishMode.Broadcast) => AsyncProvider.PublishAsync(id, message, mode);
        public static Task PublishToAsync(object id, object message) => AsyncProvider.PublishToAsync(id, message);

        #endregion

        #region Publish

        public static Task<R[]> PublishAsync<R>(object message) => AsyncProvider.PublishAsync<R>(message);
        public static Task<R[]> PublishAsync<R>(object id, object message, PublishMode mode = PublishMode.Broadcast) => AsyncProvider.PublishAsync<R>(id, message, mode);
        public static Task<R[]> PublishToAsync<R>(object id, object message) => AsyncProvider.PublishToAsync<R>(id, message);

        #endregion

        #region Request

        public static Task<R> PublishAsync<R>(IRequest<R> request) => AsyncProvider.PublishAsync(request);
        public static Task<R> PublishAsync<R>(object id, IRequest<R> request, PublishMode mode = PublishMode.Broadcast) => AsyncProvider.PublishAsync(id, request, mode);
        public static Task<R> PublishToAsync<R>(object id, IRequest<R> request) => AsyncProvider.PublishToAsync(id, request);

        #endregion

        #region Aggregate

        public static Task<S> PublishAsync<S>(IAggregate<S> aggregate) => AsyncProvider.PublishAsync(aggregate);
        public static Task<S> PublishAsync<S>(object id, IAggregate<S> aggregate, PublishMode mode = PublishMode.Broadcast) => AsyncProvider.PublishAsync(id, aggregate, mode);
        public static Task<S> PublishToAsync<S>(object id, IAggregate<S> aggregate) => AsyncProvider.PublishToAsync(id, aggregate);
        public static Task<A> PublishAsync<S, A>(IAggregate<S, A> aggregate) => AsyncProvider.PublishAsync(aggregate);
        public static Task<A> PublishAsync<S, A>(object id, IAggregate<S, A> aggregate, PublishMode mode = PublishMode.Broadcast) => AsyncProvider.PublishAsync(id, aggregate, mode);
        public static Task<A> PublishToAsync<S, A>(object id, IAggregate<S, A> aggregate) => AsyncProvider.PublishToAsync(id, aggregate);
        public static Task<R> PublishAsync<S, A, R>(IAggregate<S, A, R> aggregate) => AsyncProvider.PublishAsync(aggregate);
        public static Task<R> PublishAsync<S, A, R>(object id, IAggregate<S, A, R> aggregate, PublishMode mode = PublishMode.Broadcast) => AsyncProvider.PublishAsync(id, aggregate, mode);
        public static Task<R> PublishToAsync<S, A, R>(object id, IAggregate<S, A, R> aggregate) => AsyncProvider.PublishToAsync(id, aggregate);

        #endregion

        #region Subscribe (Action)

        public static Task SubscribeAsync<T>(Action<T> callback, int order = 0) => AsyncProvider.SubscribeAsync(callback, order);
        public static Task SubscribeAsync<T>(object id, Action<T> callback, int order = 0) => AsyncProvider.SubscribeAsync(id, callback, order);
        public static Task UnsubscribeAsync<T>(Action<T> callback) => AsyncProvider.UnsubscribeAsync(callback);
        public static Task UnsubscribeAsync<T>(object id, Action<T> callback) => AsyncProvider.UnsubscribeAsync(id, callback);

        #endregion

        #region Subscribe (Func)

        public static Task SubscribeAsync<T, R>(Func<T, R> callback, int order = 0) => AsyncProvider.SubscribeAsync(callback, order);
        public static Task SubscribeAsync<T, R>(object id, Func<T, R> callback, int order = 0) => AsyncProvider.SubscribeAsync(id, callback, order);
        public static Task UnsubscribeAsync<T, R>(Func<T, R> callback) => AsyncProvider.UnsubscribeAsync(callback);
        public static Task UnsubscribeAsync<T, R>(object id, Func<T, R> callback) => AsyncProvider.UnsubscribeAsync(id, callback);

        #endregion
    }
}