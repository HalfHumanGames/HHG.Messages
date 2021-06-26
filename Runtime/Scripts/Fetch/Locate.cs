//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace HHG.Messages
//{
//    public class Locate<T>
//    {

//    }

//    // Make static methods wrap instance that implements ILocator (like Message)
//    public static class Locator
//    {
//        // TODO: Make work with id. Make with with method (so can do factory)
//        private static readonly Dictionary<object, Delegate> fetches = new Dictionary<object, Delegate>();

//        public static void Add<T>(T item) where T : class
//        {
//            Func<Locate<T>, T> callback = CreateCallback(item);
//            Message.Subscribe(callback);
//            fetches.Add(item, callback);
//        }

//        public static void Remove<T>(T item) where T : class
//        {
//            Message.Unsubscribe((Func<Locate<T>, T>)fetches[item]);
//            fetches.Remove(item);
//        }

//        public static T Get<T>() where T : class
//        {
//            return Message.Publish<Locate<T>, T>().FirstOrDefault();
//        }

//        public static T[] GetAll<T>() where T : class
//        {
//            return Message.Publish<Locate<T>, T>();
//        }

//        private static Func<Locate<T>, T> CreateCallback<T>(T item)
//        {
//            return fetch => item;
//        }
//    }
//}
