using Ditto.AsyncMvvm.Internal;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Ditto.AsyncMvvm
{
    /// <summary>
    /// Base class for asynchronous property helpers.
    /// </summary>
    public abstract class AsyncPropertyHelperBase : AsyncHelperBase, IAsyncPropertyHelper
    {
        /// <summary>
        /// Creates a new property helper instance.
        /// </summary>
        /// <param name="onPropertyChanged">Property change notification delegate.</param>
        protected AsyncPropertyHelperBase(Action<string> onPropertyChanged)
            : base(onPropertyChanged)
        {
        }

        /// <summary>
        /// Gets the value of a lazy property.
        /// </summary>
        /// <typeparam name="T">The type of the property value.</typeparam>
        /// <param name="getValue">The delegate used to calculate the property value.</param>
        /// <param name="comparer">The optional equality comparer.</param>
        /// <param name="propertyName">The name of the property.</param>
        public T Get<T>(Func<T> getValue, IEqualityComparer<T> comparer = null, [CallerMemberName] string propertyName = null)
        {
            return GetOrAddLazyProperty(getValue, comparer, propertyName).GetValue(propertyName);
        }

        /// <summary>
        /// Gets the value of an asynchronous property.
        /// </summary>
        /// <typeparam name="T">The type of the property value.</typeparam>
        /// <param name="getValueAsync">The delegate used to calculate the property value.</param>
        /// <param name="listener">The optional task listener.</param>
        /// <param name="propertyName">The name of the property.</param>
        public T Get<T>(Func<Task<T>> getValueAsync, ITaskListener listener = null,
            [CallerMemberName] string propertyName = null)
        {
            if (getValueAsync == null)
                throw new ArgumentNullException("getValueAsync");
            return Get(_ => getValueAsync(), CancellationToken.None, listener, propertyName);
        }

        /// <summary>
        /// Gets the value of an asynchronous property.
        /// </summary>
        /// <typeparam name="T">The type of the property value.</typeparam>
        /// <param name="getValueAsync">The delegate used to calculate the property value.</param>
        /// <param name="token">The optional cancellation token.</param>
        /// <param name="listener">The optional task listener.</param>
        /// <param name="propertyName">The name of the property.</param>
        public T Get<T>(Func<CancellationToken, Task<T>> getValueAsync, CancellationToken token = default(CancellationToken),
            ITaskListener listener = null, [CallerMemberName] string propertyName = null)
        {
            return GetOrAddAsyncProperty(getValueAsync, propertyName).GetValue(token, listener, propertyName);
        }

        /// <summary>
        /// Invalidates the specified property.
        /// </summary>
        /// <typeparam name="T">The type of the property value.</typeparam>
        /// <param name="propertyExpression">The property expression (e.g. p => p.PropertyName).</param>
        /// <returns><value>true</value> if successful.</returns>
        public bool Invalidate<T>(Expression<Func<T>> propertyExpression)
        {
            var propertyName = PropertySupport.ExtractPropertyName(propertyExpression);
            return Invalidate<T>(propertyName);
        }

        /// <summary>
        /// Invalidates the specified property.
        /// </summary>
        /// <typeparam name="T">The type of the property value.</typeparam>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns><value>true</value> if successful.</returns>
        public virtual bool Invalidate<T>([CallerMemberName] string propertyName = null)
        {
            return InvalidateProperty(propertyName);
        }

        /// <summary>
        /// Indicates whether the notification delegate will be invoked on invalidation.
        /// </summary>
        protected override bool IsNotifyOnInvalidate
        {
            get { return true; }
        }
    }
}
