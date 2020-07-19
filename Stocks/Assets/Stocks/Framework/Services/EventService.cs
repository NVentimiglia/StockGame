using System;

namespace Framework
{
    /// <summary>
    /// Generic Message broadcaster
    /// </summary>
    /// <remarks>
    /// Global Message Broker. Routed by Type
    /// </remarks>
    /// <typeparam name="TMessage">the type of message being raised</typeparam>
    public static class EventService<TMessage>
    {
        /// <summary>
        /// Event
        /// </summary>
        public static event Action<TMessage> OnMessage = delegate { };
        private static TMessage _instance;

        /// <summary>
        /// sends a message to subscriptions
        /// </summary>
        public static void Publish(TMessage message)
        {
            OnMessage(message);
        }

        /// <summary>
        /// sends a static message to subscriptions
        /// </summary>
        public static void Publish()
        {
            if (_instance == null)
            {
                _instance = Activator.CreateInstance<TMessage>();
            }

            OnMessage(_instance);
        }

        /// <summary>
        /// Adds a route
        /// </summary>
        public static void Subscribe(Action<TMessage> handler)
        {
            OnMessage += handler;
        }

        /// <summary>
        /// removes a route
        /// </summary>
        public static void Unsubscribe(Action<TMessage> handler)
        {
            OnMessage -= handler;
        }
    }
}