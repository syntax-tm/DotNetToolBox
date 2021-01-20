using System;
using System.Collections.Generic;

namespace DotNetToolBox.Events
{
    /// <summary>
    /// Weak event handlers to avoid keeping strong reference to subscriber of an event.
    /// </summary>
    public static class WeakEventHandlerExtensions
    {
        public static List<IWeakEventHandler> subscriptions = new List<IWeakEventHandler>();

        public static DateTime LastPurge { get; private set; } = DateTime.Now;

        public static readonly TimeSpan PurgeInterval = TimeSpan.FromSeconds(10);

        /// <summary>
        /// Adds the weak event handler.
        /// </summary>
        public static void AddWeakHandler<TEventArgs>(this object source, string eventName, EventHandler<TEventArgs> handler)
            where TEventArgs : EventArgs
        {
            var wHandler = new WeakEventHandler<TEventArgs>(source, eventName, handler);
            subscriptions.Add(wHandler);

            if (ShouldPurge())
            {
                Purge();
            }
        }

        /// <summary>
        /// Purges all event subscriptions in which the target has been garbage collected.
        /// </summary>
        public static void Purge()
        {
            for (var i = 0; i < subscriptions.Count; )
            {
                var subscription = subscriptions[i];
                if (subscription.CheckUnsubscribe())
                {
                    subscriptions.Remove(subscription);
                }
                else
                {
                    i++;
                }
            }

            LastPurge = DateTime.Now;
        }

        private static bool ShouldPurge()
        {
            var elapsed = DateTime.Now - LastPurge;
            return elapsed >= PurgeInterval;
        }

    }
}