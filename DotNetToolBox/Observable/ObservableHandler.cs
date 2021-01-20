using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using JetBrains.Annotations;

//namespace DotNetToolBox.Observable
//{

//    /// <summary>
//    /// This class can be used to watch (observe) an object implementing the <see cref="INotifyPropertyChanged"/> <c>interface</c> and invoke 
//    /// specific <see cref="Action"/> when the source object's properties change notifications are raised. This class is based on the
//    /// Observer pattern and utilizes the .NET framework's <c>WeakReference</c> to ensure the object's references are 
//    /// not being referenced in memory.
//    /// </summary>
//    /// <typeparam name="T">The source object that implements the <see cref="INotifyPropertyChanged"/> interface.</typeparam>
//    /// <seealso cref="INotifyPropertyChanged" />
//    /// <seealso cref="WeakReference" />
//    public class ObservableHandler<T> : IWeakEventListener
//        where T : class, INotifyPropertyChanged
//    {

//        protected readonly Dictionary<string, Action> typelessHandlers = new Dictionary<string, Action>();
//        protected readonly Dictionary<string, Action<T>> typeHandlers = new Dictionary<string, Action<T>>();
//        protected readonly WeakReference<T> source;

//        /// <summary>
//        /// Creates a new <see cref="ObservableHandler&lt;T&gt;"/> to observe properties for change notifications and execute
//        /// <see cref="Action"/> responses to changes.
//        /// </summary>
//        /// <param name="source">The source object that implements the <see cref="INotifyPropertyChanged"/> interface.</param>
//        public ObservableHandler([NotNull] T source)
//        {
//            if (source == null) throw new ArgumentNullException(nameof(source));
//            this.source = new WeakReference<T>(source);
//        }

//        bool IWeakEventListener.ReceiveWeakEvent([NotNull] Type managerType, [CanBeNull] object sender, [CanBeNull] EventArgs e)
//        {
//            return OnReceiveWeakEvent(managerType, sender, e);
//        }

//        /// <summary>
//        /// Watches the source object's <see cref="INotifyPropertyChanged"/> events looking for a change of the property specified in
//        /// the <see cref="expression" /> parameter. An example of a call to this function should look something like this:
//        /// <example><code>
//        /// <para />private ObservableHandler&lt;T&gt; _objObserver;
//        /// <para />private void SetupObservables(&lt;T&gt; obj) {
//        ///   <para>_objObserver = new ObservableHandler&lt;T&gt;(obj);</para>
//        ///   <para>_objObserver.AddAndInvoke(o =&gt; o.SampleProperty, OnSamplePropertyChanged);</para>
//        /// }<para />private void OnSamplePropertyChanged() { }
//        /// </code></example>
//        /// </summary>
//        /// <param name="expression">This is the <see cref="LambdaExpression"/> of the <c>Property</c> to watch on the source object.<para> </para>
//        /// <para>Example:<para/><example><c>o => o.PropertyToWatch</c></example></para>
//        /// </param>
//        /// <param name="handler">The <see cref="handler"/> parameter is the <see cref="Action" /> that should be invoked when an <see cref="INotifyPropertyChanged"/> 
//        /// event is observed for the property name in the <see cref="expression"/> parameter. in the class observing the object.<para></para>
//        /// Since the <c>OnSamplePropertyChanged</c> function has no parameters, you can use <c>OnSamplePropertyChanged</c> as the <see cref="Action"/> 
//        /// parameter and when the <see cref="expression" /> property changes, the <c>OnSamplePropertyChanged</c> function will also be called. 
//        /// </param>
//        public ObservableHandler<T> Add([NotNull] Expression<Func<T, object>> expression, [NotNull] Action handler)
//        {
//            if (expression == null) throw new ArgumentNullException(nameof(expression));
//            if (typelessHandlers == null) throw new ArgumentNullException(nameof(typelessHandlers));

//            var sourceObj = GetSource();
//            if (sourceObj == null) throw new InvalidOperationException($"The {nameof(sourceObj)} has been garbage collected.");

//            var propertyName = ReflectionHelper.GetPropertyNameFromLambda(expression);
            
//            if (string.IsNullOrEmpty(propertyName)) throw new InvalidOperationException($"The {nameof(propertyName)} could not be determined from the expression.");

//            if (!typelessHandlers.ContainsKey(propertyName))
//            {
//                typelessHandlers.Add(propertyName, handler);
//            }
//            else
//            {
//                typelessHandlers[propertyName] = handler;
//            }
            
//            PropertyChangedEventManager.AddListener(sourceObj, this, propertyName);

//            return this;
//        }

//        /// <summary>
//        ///     Watches the source object's <see cref="INotifyPropertyChanged"/> events looking for a change of the property specified in
//        ///     the <see cref="expression" /> parameter. An example of a call to this function should look something like this:
//        ///     <example><code>
//        ///     <para />private ObservableHandler&lt;T&gt; _objObserver;
//        ///     <para />private void SetupObservables(&lt;T&gt; obj) {
//        ///       <para>_objObserver = new ObservableHandler&lt;T&gt;(obj);</para>
//        ///       <para>_objObserver.AddAndInvoke(o =&gt; o.SampleProperty, OnSamplePropertyChanged);</para>
//        ///     }<para />private void OnSamplePropertyChanged([NotNull] T sourceObj) { }
//        ///     </code></example>
//        /// </summary>
//        /// <param name="expression">This is the <see cref="LambdaExpression"/> of the <c>Property</c> to watch on the source object.<para> </para>
//        ///     <para>Example:<para/><example><c>o => o.PropertyToWatch</c></example></para>
//        /// </param>
//        /// <param name="handler">The <see cref="handler"/> parameter is the <see cref="Action&lt;T&gt;" /> that should be invoked when an <see cref="INotifyPropertyChanged"/> 
//        ///     event is observed for the property name in the <see cref="expression"/> parameter. in the class observing the object.<para></para>
//        ///     Since the <c>OnSamplePropertyChanged</c> function has one parameter of Type <see cref="T"/>, you can use <c>OnSamplePropertyChanged</c> as the <see cref="Action&lt;T&gt;"/> 
//        ///     parameter and when the <see cref="expression" /> property changes, the <c>OnSamplePropertyChanged</c> function will also be called. 
//        /// </param>
//        public ObservableHandler<T> Add([NotNull] Expression<Func<T, object>> expression, [NotNull] Action<T> handler)
//        {
//            if (expression == null) throw new ArgumentNullException(nameof(expression));
//            if (typeHandlers == null) throw new ArgumentNullException(nameof(typeHandlers));

//            var sourceObj = GetSource();
//            if (sourceObj == null) throw new InvalidOperationException($"The {nameof(sourceObj)} has been garbage collected.");

//            var propertyName = ReflectionHelper.GetPropertyNameFromLambda(expression);
            
//            if (string.IsNullOrEmpty(propertyName)) throw new InvalidOperationException($"The {nameof(propertyName)} could not be determined from the expression.");

//            if (!typeHandlers.ContainsKey(propertyName))
//            {
//                typeHandlers.Add(propertyName, handler);
//            }
//            else
//            {
//                typeHandlers[propertyName] = handler;
//            }

//            PropertyChangedEventManager.AddListener(sourceObj, this, propertyName);

//            return this;
//        }

//        /// <summary>
//        ///     Invokes the <param name="handler" /> now <c>and</c> when an <see cref="INotifyPropertyChanged"/> event is raised for 
//        ///     the property in the <see cref="expression" /> parameter. An example of a call to this function should look something like this:
//        ///     <example><code>
//        ///     <para />private ObservableHandler&lt;T&gt; _objObserver;
//        ///     <para />private void SetupObservables(&lt;T&gt; obj) {
//        ///       <para>_objObserver = new ObservableHandler&lt;T&gt;(obj);</para>
//        ///       <para>_objObserver.AddAndInvoke(o =&gt; o.SampleProperty, OnSamplePropertyChanged);</para>
//        ///     }<para />private void OnSamplePropertyChanged() { }
//        ///     </code></example>
//        /// </summary>
//        /// <param name="expression">This is the <see cref="LambdaExpression"/> of the <c>Property</c> to watch on the source object.<para> </para>
//        ///     <para>Example:<para/><example><c>o => o.PropertyToWatch</c></example></para>
//        /// </param>
//        /// <param name="handler">The <see cref="handler"/> parameter is the <see cref="Action" /> that should be invoked when an <see cref="INotifyPropertyChanged"/> 
//        ///     event is observed for the property name in the <see cref="expression"/> parameter. in the class observing the object.<para></para>
//        ///     Since the <c>OnSamplePropertyChanged</c> function has no parameters, you can use <c>OnSamplePropertyChanged</c> as the <see cref="Action"/> 
//        ///     parameter and when the <see cref="expression" /> property changes, the <c>OnSamplePropertyChanged</c> function will also be called. 
//        /// </param>
//        public ObservableHandler<T> AddAndInvoke([NotNull] Expression<Func<T, object>> expression, [NotNull] Action handler)
//        {
//            if (expression == null) throw new ArgumentNullException(nameof(expression));
//            if (handler == null) throw new ArgumentNullException(nameof(handler));

//            Add(expression, handler);
//            handler.Invoke();
//            return this;
//        }

//        /// <summary>
//        ///     Invokes the <param name="handler" /> now <c>and</c> when an <see cref="INotifyPropertyChanged"/> event is raised for 
//        ///     the property in the <see cref="expression" /> parameter. An example of a call to this function should look something like this:
//        ///     <example><code>
//        ///     <para />private ObservableHandler&lt;T&gt; _objObserver;
//        ///     <para />private void SetupObservables(&lt;T&gt; obj) {
//        ///       <para>_objObserver = new ObservableHandler&lt;T&gt;(obj);</para>
//        ///       <para>_objObserver.AddAndInvoke(o =&gt; o.SampleProperty, OnSamplePropertyChanged);</para>
//        ///     }<para />private void OnSamplePropertyChanged([NotNull] T sourceObj) { }
//        ///     </code></example>
//        /// </summary>
//        /// <param name="expression">This is the <see cref="LambdaExpression"/> of the <c>Property</c> to watch on the source object.<para> </para>
//        ///     <para>Example:<para/><example><c>o => o.PropertyToWatch</c></example></para>
//        /// </param>
//        /// <param name="handler">The <see cref="handler"/> parameter is the <see cref="Action&lt;T&gt;" /> that should be invoked when an <see cref="INotifyPropertyChanged"/> 
//        ///     event is observed for the property name in the <see cref="expression"/> parameter. in the class observing the object.<para></para>
//        ///     Since the <c>OnSamplePropertyChanged</c> function has one parameter of <c>Type</c> <see cref="T"/>, you can use <c>OnSamplePropertyChanged</c> as the <see cref="Action&lt;T&gt;"/> 
//        ///     parameter and when the <see cref="expression" /> property changes, the <c>OnSamplePropertyChanged</c> function will also be called. 
//        /// </param>
//        public ObservableHandler<T> AddAndInvoke([NotNull] Expression<Func<T, object>> expression, [NotNull] Action<T> handler)
//        {
//            if (expression == null) throw new ArgumentNullException(nameof(expression));
//            if (handler == null) throw new ArgumentNullException(nameof(handler));

//            Add(expression, handler);
//            handler.Invoke(GetSource());
//            return this;
//        }

//        private T GetSource()
//        {
//            var hasSource = this.source.TryGetTarget(out var sourceObj);
//            if (!hasSource) throw new InvalidOperationException($"Failed to get the instance of the {nameof(source)} object from the stored {nameof(WeakReference)}.");
//            return sourceObj;
//        }

//        public virtual bool OnReceiveWeakEvent([NotNull] Type managerType, [CanBeNull] object sender, [CanBeNull] EventArgs e)
//        {
//            if (managerType != typeof(PropertyChangedEventManager)) return false;
//            var propertyName = ((PropertyChangedEventArgs) e)?.PropertyName ?? string.Empty;
//            Notify(propertyName);
//            return true;
//        }

//        protected void Notify([CanBeNull] string propertyName = "")
//        {
//            var sourceObj = GetSource();
//            if (typeHandlers == null)
//            {
//                var handlersNullMessage = $"{nameof(typeHandlers)} dictionary is null and cannot be notified.";
//                throw new ArgumentNullException(handlersNullMessage);
//            }
//            if (typelessHandlers == null)
//            {
//                var handlersNullMessage = $"{nameof(typelessHandlers)} dictionary is null and cannot be notified.";
//                throw new ArgumentNullException(handlersNullMessage);
//            }

//            //  if there was no property name passed in, invoke all of the
//            //  handlers we have for this object
//            if (string.IsNullOrEmpty(propertyName))
//            {
//                foreach(var handler in typeHandlers.Values)
//                {
//                    handler?.Invoke(sourceObj);
//                }
//                foreach(var handler in typelessHandlers.Values)
//                {
//                    handler?.Invoke();
//                }
//                return;
//            }

//            //  try to get the handler specifically for this property from the type handlers
//            //  and the typeless handlers and then invoke the applicable handlers
//            if (typeHandlers.TryGetValue(propertyName, out var regularHandler))
//            {
//                regularHandler?.Invoke(sourceObj);
//            }
//            if (typelessHandlers.TryGetValue(propertyName, out var genericHandler))
//            {
//                genericHandler?.Invoke();
//            }
//        }
//    }
//}