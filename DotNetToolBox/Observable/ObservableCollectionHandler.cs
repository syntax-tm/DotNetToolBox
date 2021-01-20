using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using JetBrains.Annotations;

//namespace DotNetToolBox.Observable
//{

//    /// <summary>
//    /// This class can be used to watch (observe) a <see cref="INotifyCollectionChanged"/> collection source and invoke 
//    /// specific <see cref="Action"/> when the collection's change notifications are raised. This class is based on the
//    /// Observer pattern and utilizes the .NET framework's <c>WeakReference</c> to ensure the object's references are 
//    /// not being referenced in memory which would prevent the garbage collection.
//    /// </summary>
//    /// <typeparam name="T">The <see cref="ICollection{TU}"/> source collection.</typeparam>
//    /// <typeparam name="TU">The <c>Type</c> of item in the <see cref="T"/> source collection.</typeparam>
//    /// <remarks>
//    ///    <list type="bullet">
//    ///       <item>
//    ///         <term><c>On Item Added:</c></term><para />
//    ///         <description>Invokes the <see cref="RaiseAddItem"/> handlers, if any, when an item is added to the collection.</description>
//    ///       </item><para />
//    ///       <item>
//    ///         <term><c>On Item Removed:</c></term><para />
//    ///         <description>Invokes the <see cref="RaiseRemoveItem"/> handlers, if any, when an item is removed from the collection.</description>
//    ///       </item><para />
//    ///       <item>
//    ///         <term><c>On Item Replaced:</c></term><para />
//    ///         <description>Invokes the <see cref="RaiseReplaceItem"/>, <see cref="RaiseAddItem"/>, and <see cref="RaiseRemoveItem"/> handlers, 
//    ///                      if any, when an item in the collection is replaced.</description>
//    ///       </item><para />
//    ///       <item>
//    ///         <term><c>On Collection Reset:</c></term><para />
//    ///         <description>Invokes the <see cref="RaiseReset"/> handlers, if any, when the collection is reset.</description>
//    ///       </item>
//    ///     </list>
//    /// </remarks>
//    /// <seealso cref="NotifyCollectionChangedEventHandler" />
//    /// <seealso cref="NotifyCollectionChangedAction" />
//    /// <seealso cref="WeakReference" />
//    public class ObservableCollectionHandler<T, TU> : IWeakEventListener
//        where T : class, INotifyCollectionChanged, ICollection<TU>
//    {
//        private readonly WeakReference<T> _source;

//        private Action _addItemNoTypeHandler;
//        private Action<T> _addItemCollectionTypeHandler;
//        private Action<T, TU> _addItemHandler;

//        private Action _removeItemNoTypeHandler;
//        private Action<T> _removeItemCollectionHandler;
//        private Action<T, TU> _removeItemHandler;

//        private Action _replaceItemNoTypeHandler;
//        private Action<T> _replaceItemCollectionTypeHandler;
//        private Action<T, TU, TU> _replaceItemHandler;

//        private Action _resetNoTypeHandler;
//        private Action<T> _resetHandler;

//        private bool _isRegistered;

//        /// <summary>
//        /// Creates a new <see cref="ObservableCollectionHandler{T,TU}"/> for the <see cref="source"/> collection.
//        /// </summary>
//        /// <param name="source">The <see cref="ICollection{TU}"/> that implements the <see cref="INotifyCollectionChanged"/> <c>interface</c>.</param>
//        /// <example><see cref="ObservableCollection{T}"/></example>
//        public ObservableCollectionHandler([NotNull] T source)
//        {
//            if (source == null) throw new ArgumentNullException(nameof(source));
//            _source = new WeakReference<T>(source);
//        }

//        bool IWeakEventListener.ReceiveWeakEvent([NotNull] Type managerType, [CanBeNull] object sender, EventArgs e)
//        {
//            return OnCollectionReceiveWeakEvent(managerType, sender, e);
//        }

//        /// <summary>
//        /// Watches the source collection to invoke the <param name="handler" /> when an item is added.
//        /// </summary>
//        /// <param name="handler">The <see cref="Action"/> to be invoked when an item is added to the source collection.</param>
//        /// <example>private void OnItemAdded() { }</example>
//        public ObservableCollectionHandler<T, TU> SetAddItem([NotNull] Action handler)
//        {
//            RegisterEventHandler();
//            _addItemNoTypeHandler = handler;
//            return this;
//        }

//        /// <summary>
//        /// Watches the source collection to invoke the <param name="handler" /> when an item is added.
//        /// </summary>
//        /// <param name="handler">The <see cref="Action"/> to be invoked when an item is added to the source collection.</param>
//        /// <example>private void OnItemAdded([NotNull] T source) { }</example>
//        public ObservableCollectionHandler<T, TU> SetAddItem([NotNull] Action<T> handler)
//        {
//            RegisterEventHandler();
//            _addItemCollectionTypeHandler = handler;
//            return this;
//        }

//        /// <summary>
//        /// Watches the source collection to invoke the <param name="handler" /> when an item is added.
//        /// </summary>
//        /// <param name="handler">The <see cref="Action"/> to be invoked when an item is added to the source collection.</param>
//        /// <example>private void OnItemAdded([NotNull] T source, [NotNull] TU addedItem) { }</example>
//        public ObservableCollectionHandler<T, TU> SetAddItem([NotNull] Action<T, TU> handler)
//        {
//            RegisterEventHandler();
//            _addItemHandler = handler;
//            return this;
//        }

//        /// <summary>
//        /// Invokes the <param name="handler" /> now <c>and</c> watches for when an item is added to the collection.
//        /// </summary>
//        /// <param name="handler">The <see cref="Action"/> to be invoked when an item is added to the source collection.</param>
//        /// <example>private void OnItemAdded() { }</example>
//        public ObservableCollectionHandler<T, TU> SetAddItemAndInvoke([NotNull] Action handler)
//        {
//            if (handler == null) throw new ArgumentNullException(nameof(handler));

//            SetAddItem(handler);

//            var source = GetSource();
//            if (source == null)
//            {
//                var sourceNullMessage = $"The {nameof(source)} has been garbage collected.";
//                throw new InvalidOperationException(sourceNullMessage);
//            }
//            handler.Invoke();
//            return this;
//        }

//        /// <summary>
//        /// Invokes the <param name="handler" /> now <c>and</c> watches for when an item is added to the collection.
//        /// </summary>
//        /// <param name="handler">The <see cref="Action"/> to be invoked when an item is added to the source collection.</param>
//        /// <example>private void OnItemAdded([NotNull] T source) { }</example>
//        public ObservableCollectionHandler<T, TU> SetAddItemAndInvoke([NotNull] Action<T> handler)
//        {
//            if (handler == null) throw new ArgumentNullException(nameof(handler));

//            SetAddItem(handler);

//            var source = GetSource();
//            if (source == null)
//            {
//                var sourceNullMessage = $"The {nameof(source)} has been garbage collected.";
//                throw new InvalidOperationException(sourceNullMessage);
//            }
//            handler.Invoke(source);
//            return this;
//        }

//        /// <summary>
//        /// Invokes the <param name="handler" /> now <c>and then</c> watches for when an item is added to the collection.
//        /// </summary>
//        /// <param name="handler">The <see cref="Action"/> to be invoked when an item is added to the source collection.</param>
//        /// <example>private void OnItemAdded([NotNull] T source, [NotNull] TU addedItem) { }</example>
//        public ObservableCollectionHandler<T, TU> SetAddItemAndInvoke([NotNull] Action<T, TU> handler)
//        {
//            if (handler == null) throw new ArgumentNullException(nameof(handler));
//            SetAddItem(handler);
//            var source = GetSource();
//            if (source == null)
//            {
//                var sourceNullMessage = $"The {nameof(source)} has been garbage collected.";
//                throw new InvalidOperationException(sourceNullMessage);
//            }
//            foreach (var item in source)
//            {
//                handler.Invoke(source, item);
//            }
//            return this;
//        }

//        /// <summary>
//        /// Watches the source collection to invoke the <param name="handler" /> when an item is removed.
//        /// </summary>
//        /// <param name="handler">The <see cref="Action"/> to be invoked when an item is removed from the source collection.</param>
//        /// <example>private void OnItemRemoved() { }</example>
//        public ObservableCollectionHandler<T, TU> SetRemoveItem([NotNull] Action handler)
//        {
//            RegisterEventHandler();
//            _removeItemNoTypeHandler = handler ?? throw new ArgumentNullException(nameof(handler));
//            return this;
//        }

//        /// <summary>
//        /// Watches the source collection to invoke the <param name="handler" /> when an item is removed.
//        /// </summary>
//        /// <param name="handler">The <see cref="Action"/> to be invoked when an item is removed from the source collection.</param>
//        /// <example>private void OnItemRemoved([NotNull] T source) { }</example>
//        public ObservableCollectionHandler<T, TU> SetRemoveItem([NotNull] Action<T> handler)
//        {
//            RegisterEventHandler();
//            _removeItemCollectionHandler = handler ?? throw new ArgumentNullException(nameof(handler));
//            return this;
//        }

//        /// <summary>
//        /// Watches the source collection to invoke the <param name="handler" /> when an item is removed.
//        /// </summary>
//        /// <param name="handler">The <see cref="Action"/> to be invoked when an item is removed from the source collection.</param>
//        /// <example>private void OnItemRemoved([NotNull] T source, [NotNull] TU removedItem) { }</example>
//        public ObservableCollectionHandler<T, TU> SetRemoveItem([NotNull] Action<T, TU> handler)
//        {
//            RegisterEventHandler();
//            _removeItemHandler = handler ?? throw new ArgumentNullException(nameof(handler));
//            return this;
//        }

//        /// <summary>
//        /// Watches the source collection to invoke the <param name="handler" /> when an item is replaced.
//        /// </summary>
//        /// <param name="handler">The <see cref="Action"/> to be invoked when an item is replaced in the source collection.</param>
//        /// <example>private void OnItemReplaced() { }</example>
//        public ObservableCollectionHandler<T, TU> SetReplaceItem([NotNull] Action handler)
//        {
//            RegisterEventHandler();
//            _replaceItemNoTypeHandler = handler ?? throw new ArgumentNullException(nameof(handler));
//            return this;
//        }

//        /// <summary>
//        /// Watches the source collection to invoke the <param name="handler" /> when an item is replaced.
//        /// </summary>
//        /// <param name="handler">The <see cref="Action"/> to be invoked when an item is replaced in the source collection.</param>
//        /// <example>private void OnItemReplaced([NotNull] T source) { }</example>
//        /// <seealso cref="NotifyCollectionChangedAction.Replace" />
//        public ObservableCollectionHandler<T, TU> SetReplaceItem([NotNull] Action<T> handler)
//        {
//            RegisterEventHandler();
//            _replaceItemCollectionTypeHandler = handler ?? throw new ArgumentNullException(nameof(handler));
//            return this;
//        }

//        /// <summary>
//        /// Watches the source collection to invoke the <param name="handler" /> when an item is replaced.
//        /// </summary>
//        /// <param name="handler">The <see cref="Action"/> to be invoked when an item is replaced in the source collection.</param>
//        /// <example>private void OnItemReplaced([NotNull] T source, [NotNull] TU oldItem, [NotNull] TU newItem) { }</example>
//        public ObservableCollectionHandler<T, TU> SetReplaceItem([NotNull] Action<T, TU, TU> handler)
//        {
//            RegisterEventHandler();
//            _replaceItemHandler = handler ?? throw new ArgumentNullException(nameof(handler));
//            return this;
//        }
        
//        /// <summary>
//        /// Watches the source collection for a <see cref="NotifyCollectionChangedAction.Reset" /> change and invokes the <param name="handler" />.
//        /// </summary>
//        /// <param name="handler">The <see cref="Action"/> to be invoked when the source collection is reset.</param>
//        /// <example>private void OnCollectionReset() { }</example>
//        public ObservableCollectionHandler<T, TU> SetReset([NotNull] Action handler)
//        {
//            RegisterEventHandler();
//            _resetNoTypeHandler = handler ?? throw new ArgumentNullException(nameof(handler));
//            return this;
//        }

//        /// <summary>
//        /// Watches the source collection for a <see cref="NotifyCollectionChangedAction.Reset" /> change and invokes the <param name="handler" />.
//        /// </summary>
//        /// <param name="handler">The <see cref="Action"/> to be invoked when the source collection is reset.</param>
//        /// <example>private void OnCollectionReset([NotNull] T source) { }</example>
//        public ObservableCollectionHandler<T, TU> SetReset([NotNull] Action<T> handler)
//        {
//            RegisterEventHandler();
//            _resetHandler = handler ?? throw new ArgumentNullException(nameof(handler));
//            return this;
//        }

//        private T GetSource()
//        {
//            var hasSource = _source.TryGetTarget(out var sourceObj);
//            if (!hasSource) throw new InvalidOperationException($"Failed to get the instance of the {nameof(_source)} object from the stored {nameof(WeakReference)}.");
//            return sourceObj;
//        }

//        private void RegisterEventHandler()
//        {
//            if (_isRegistered) return;
//            var source = GetSource();
//            if (source == null)
//            {
//                var sourceNullMessage = $"The {nameof(source)} has been garbage collected.";
//                throw new InvalidOperationException(sourceNullMessage);
//            }
//            CollectionChangedEventManager.AddListener(source, this);
//            _isRegistered = true;
//        }
        
//        /// <summary>
//        ///    <c>Not</c> intended to be used outside of the <see cref="ObservableCollectionHandler{T,TU}"/> class. Handles the collection
//        ///    changed event and calls the appropriate handlers.
//        /// </summary>
//        /// <param name="managerType">The <see cref="CollectionChangedEventManager"/> type.</param>
//        /// <param name="sender">The source of the event, this is always the <see cref="ICollection&lt;T&gt;"/> source collection.</param>
//        /// <param name="e">The arguments for the raised event. Contains the <see cref="NotifyCollectionChangedAction"/> action type and new/removed items.</param>
//        public virtual bool OnCollectionReceiveWeakEvent([NotNull] Type managerType, [CanBeNull] object sender, [CanBeNull] EventArgs e)
//        {
//            if (managerType != typeof(CollectionChangedEventManager)) return false;
//            if (_addItemHandler == null) return false;

//            var source = GetSource();
//            if (source == null)
//            {
//                var sourceNullMessage = "Confused, received a CollectionChanged event from a source that has been garbage collected.";
//                throw new InvalidOperationException(sourceNullMessage);
//            }

//            if (!(e is NotifyCollectionChangedEventArgs actualEventArgs))
//            {
//                var actualEventArgsNullMessage = $"Event args were not of type {nameof(NotifyCollectionChangedEventArgs)}";
//                throw new NotSupportedException(actualEventArgsNullMessage);
//            }

//            var collectionAction = actualEventArgs.Action;

//            //  if the collection was reset, raise the handler for the reset
//            //  event (if any) and then return because add/remove aren't needeed
//            if (actualEventArgs.Action == NotifyCollectionChangedAction.Reset)
//            {
//                RaiseReset(actualEventArgs, source);
//                return true;
//            }

//            //  if we are replacing an item in the collection, invoke the specific
//            //  replace event handlers (if any)
//            if (collectionAction == NotifyCollectionChangedAction.Replace)
//            {
//                RaiseReplaceItem(actualEventArgs, source);
//            }

//            //  if we are adding an item or replacing (remove+add), raise the
//            //  actions for the add item handlers
//            if (collectionAction == NotifyCollectionChangedAction.Add 
//                || collectionAction == NotifyCollectionChangedAction.Replace)
//            {
//                RaiseAddItem(actualEventArgs, source);
//            }

//            //  if we are removing an item or replacing (remove+add), raise the
//            //  actions for the remove item handlers
//            if (collectionAction == NotifyCollectionChangedAction.Remove
//                || collectionAction == NotifyCollectionChangedAction.Replace)
//            {
//                RaiseRemoveItem(actualEventArgs, source);
//            }

//            return true;
//        }

//        private void RaiseReset([NotNull] NotifyCollectionChangedEventArgs e, [NotNull] T source)
//        {
//            if (_resetHandler == null && _resetNoTypeHandler == null) return;
//            if (e.Action != NotifyCollectionChangedAction.Reset) return;

//            //  raise the collection/default event handlers once for the reset
//            //  collection, no need to raise it for each item because this event
//            //  is not at the item level
//            _resetHandler?.Invoke(source);
//            _resetNoTypeHandler?.Invoke();
//        }

//        private void RaiseRemoveItem([NotNull] NotifyCollectionChangedEventArgs e, [NotNull] T source)
//        {
//            //  if we aren't tracking the remove item event at all, leave
//            if (_removeItemHandler == null && _removeItemNoTypeHandler == null
//                && _removeItemCollectionHandler == null)
//            {
//                return;
//            }

//            //  if there's no removed items, leave
//            if (e.OldItems == null) return;

//            //  raise the item-level remove item event handlers for each removed
//            //  item in the collection's event args
//            foreach (var item in e.OldItems.Cast<TU>())
//            {
//                _removeItemHandler?.Invoke(source, item);
//            }

//            //  raise the collection/default event handlers once for the removed
//            //  items, no need to raise it for each removed item because it doesn't
//            //  track/pass the removed item in the event args
//            _removeItemNoTypeHandler?.Invoke();
//            _removeItemCollectionHandler?.Invoke(source);
//        }

//        private void RaiseAddItem([NotNull] NotifyCollectionChangedEventArgs e, [NotNull] T source)
//        {
//            //  if we aren't tracking the add item event at all, leave
//            if (_addItemHandler == null && _addItemNoTypeHandler == null
//                && _addItemCollectionTypeHandler == null)
//            {
//                return;
//            }

//            //  if there's no new items, leave
//            if (e.NewItems == null) return;

//            //  raise the item-level add item event handlers for each new
//            //  item in the collection's event args
//            foreach (var item in e.NewItems.Cast<TU>())
//            {
//                _addItemHandler?.Invoke(source, item);
//            }

//            //  raise the collection/default event handlers once for the new
//            //  items, no need to raise it for each new item because it doesn't
//            //  track/pass the new item in the event args
//            _addItemNoTypeHandler?.Invoke();
//            _addItemCollectionTypeHandler?.Invoke(source);
//        }

//        private void RaiseReplaceItem([NotNull] NotifyCollectionChangedEventArgs e, [NotNull] T source)
//        {
//            //  if we aren't tracking the add item event at all, leave
//            if (_replaceItemHandler == null && _replaceItemCollectionTypeHandler == null
//                && _replaceItemNoTypeHandler == null)
//            {
//                return;
//            }

//            //  if there's no new items or removed items, leave. the
//            //  replace event should have one of each
//            if (e.NewItems == null || e.OldItems == null) return;

//            //  if there's not the same number of added/removed items
//            //  from the collection we can't try to pair up what was replaced
//            //  with what since we won't have enough info, so just invoke the events
//            //  for the replace that aren't at the item level
//            if (e.NewItems.Count != e.OldItems.Count)
//            {
//                _replaceItemNoTypeHandler?.Invoke();
//                _replaceItemCollectionTypeHandler?.Invoke(source);
//            }

//            //  loop through the new items and match up the index with the
//            //  removed items and add them to the item pairs dictionary, the key
//            //  is the index, the value is the Tuple<NewItem,OldItem>
//            var itemPairs = new Dictionary<int, Tuple<TU, TU>>();
//            var index = 0;
//            foreach (var newItem in e.NewItems.Cast<TU>())
//            {
//                var oldItem = (TU) e.OldItems[index];
//                itemPairs.Add(index, new Tuple<TU, TU>(oldItem, newItem));
//                index++;
//            }

//            //  raise the replaced, item pair-level event handlers for each replaced
//            //  item pair in the Dictionary<int, Tuple<TU, TU>> we put together
//            foreach (var replacedPair in itemPairs)
//            {
//                //  signature of method: void OnReplacedItem(T source, TU oldItem, TU newItem)
//                _replaceItemHandler?.Invoke(source, replacedPair.Value.Item1, replacedPair.Value.Item2);
//            }

//            //  raise the collection/default event handlers once for the replaced
//            //  item pairs, no need to raise it for each pair because it doesn't
//            //  track/pass the pairs in the event args so there's no point
//            _replaceItemNoTypeHandler?.Invoke();
//            _replaceItemCollectionTypeHandler?.Invoke(source);
//        }

//    }
//}