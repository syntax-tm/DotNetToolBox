using System;
using System.Reflection;

namespace DotNetToolBox.Events
{

    /// <summary>
    /// A light event handler, only this object will be kept in memory (and will keep a reference to source).
    /// This could be considered as acceptable.
    /// </summary>
    public class WeakEventHandler<TEventArgs> : IWeakEventHandler where TEventArgs : EventArgs
    {

        private readonly Delegate eventHandler;

        private readonly EventInfo eventInfo;
        private readonly WeakReference targetReference;
        private readonly MethodInfo targetMethod;

        public WeakEventHandler(object source, string eventName, EventHandler<TEventArgs> target)
        {
            eventInfo = source.GetType().GetEvent(eventName);
            targetReference = new WeakReference(source);
            targetMethod = target.Method;

            var methodInfo = GetType().GetMethod(nameof(OnEvent), BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            if (methodInfo == null) throw new ArgumentNullException(nameof(methodInfo));

            eventHandler = Delegate.CreateDelegate(eventInfo.EventHandlerType, this, methodInfo);
            eventInfo.AddEventHandler(source, eventHandler);
        }

        private void OnEvent(object sender, TEventArgs args)
        {
            if (!targetReference.IsAlive) return;
            targetMethod.Invoke(targetReference.Target, new[] { sender, args });
        }

        public bool CheckUnsubscribe()
        {
            if (targetReference.IsAlive) return false;
            eventInfo.RemoveEventHandler(targetReference.Target, eventHandler);
            return true;
        }
    }
}
