using System;
using System.Collections.Generic;

namespace Framework
{
    /// <summary>
    /// Generic Message broadcaster with support for filtering based on specific handlers
    /// </summary>
    /// <remarks>
    /// E.G. Send Message to Game Object
    /// </remarks>
    /// <typeparam name="TRoute">The type of route key, generally string or game object</typeparam>
    /// <typeparam name="TMessage">the type of message being raised</typeparam>
    public static class EventService<TRoute, TMessage>
    {
        class Route
        {
            public event Action<TMessage> Handlers = delegate { };

            public void Raise(TMessage m)
            {
                Handlers(m);
            }
            public void Clear()
            {
                Handlers = delegate { };
            }
        }

        /// <summary>
        /// All Listeners / Observers
        /// </summary>
        static readonly Dictionary<TRoute, Route> _listeners = new Dictionary<TRoute, Route>();

        /// <summary>
        /// sends a message to subscriptions
        /// </summary>
        public static bool Publish(TRoute route, TMessage message)
        {
            Route domain;
            bool result = false;
            if (_listeners.TryGetValue(route, out domain))
            {
                domain.Raise(message);
                result = true;
            }
            return result;
        }

        /// <summary>
        /// Adds a route
        /// </summary>
        public static void Subscribe(TRoute route, Action<TMessage> handler)
        {
            Route domain;
            if (!_listeners.TryGetValue(route, out domain))
            {
                domain = new Route();
                _listeners.Add(route, domain);
            }

            domain.Handlers += handler;
        }

        /// <summary>
        /// removes a handler
        /// </summary>
        public static void Unsubscribe(TRoute route, Action<TMessage> handler)
        {
            Route domain;
            if (_listeners.TryGetValue(route, out domain))
            {
                domain.Handlers -= handler;
            }
        }

        /// <summary>
        /// removes all handlers
        /// </summary>
        public static void Unsubscribe(TRoute route)
        {
            Route domain;
            if (_listeners.TryGetValue(route, out domain))
            {
                domain.Clear();
            }

        }

        /// <summary>
        /// removes all handlers
        /// </summary>
        public static void Clear()
        {
            _listeners.Clear();
        }
    }
}