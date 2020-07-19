using System;
using System.Collections.Generic;

namespace Framework
{
    public class DependencyService
    {
        public static Dictionary<Type, object> Container = new Dictionary<Type, object>();

        public static T Get<T>()
        {
            return (T)Container[typeof(T)];
        }

        public static void Add<TInterface>(TInterface i) 
        {
            Container.Remove(typeof(TInterface));
            Container.Add(typeof(TInterface), i);
        }

        public static void Remove<TInterface>()
        {
            Container.Remove(typeof(TInterface));
        }
    }
}
