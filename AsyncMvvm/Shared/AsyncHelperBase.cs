using Ditto.AsyncMvvm.Internal;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Ditto.AsyncMvvm
{
    /// <summary>
    /// Base class for asynchronous helpers.
    /// </summary>
    public abstract class AsyncHelperBase
    {
        private readonly Action<string> _onValueChanged;

        /// <summary>
        /// Creates a new helper instance.
        /// </summary>
        /// <param name="onValueChanged">Value change notification delegate.</param>
        protected AsyncHelperBase(Action<string> onValueChanged)
        {
            if (onValueChanged == null)
                throw new ArgumentNullException("onValueChanged");
            this._onValueChanged = onValueChanged;
        }

        /// <summary>
        /// Invalidates a specified property or the entire entity.
        /// </summary>
        /// <param name="propertyName">The name of the property to invalidate, or <value>null</value>
        /// or <see cref="String.Empty"/> to invalidate the entire entity.</param>
        /// <returns><value>true</value> if successful.</returns>
        /// <exception cref="NotSupportedException"/>
        public bool Invalidate([CallerMemberName] string propertyName = null)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                InvalidateEntity();
                return true;
            }
            return InvalidateProperty(propertyName);
        }

        /// <summary>
        /// Retrieves the specified lazy property if it exists; otherwise, creates the lazy property and returns it.
        /// </summary>
        /// <typeparam name="T">The type of the property value.</typeparam>
        /// <param name="getValue">The delegate used to calculate the property value.</param>
        /// <param name="comparer">The optional equality comparer.</param>
        /// <param name="propertyName">The name of the property.</param>
        protected ILazyProperty<T> GetOrAddLazyProperty<T>(Func<T> getValue, IEqualityComparer<T> comparer = null,
            [CallerMemberName] string propertyName = null)
        {
            return GetOrAddProperty(() => CreateLazyProperty(getValue, comparer), propertyName);
        }

        /// <summary>
        /// Retrieves the specified asynchronous property if it exists; otherwise, creates the asynchronous property and returns it.
        /// </summary>
        /// <typeparam name="T">The type of the property value.</typeparam>
        /// <param name="getValueAsync">The delegate used to calculate the property value.</param>
        /// <param name="propertyName">The name of the property.</param>
        protected IAsyncProperty<T> GetOrAddAsyncProperty<T>(Func<CancellationToken, Task<T>> getValueAsync,
            [CallerMemberName] string propertyName = null)
        {
            return GetOrAddProperty(() => CreateAsyncProperty(getValueAsync), propertyName);
        }

        /// <summary>
        /// Creates a lazy property.
        /// </summary>
        /// <typeparam name="T">The type of the property value.</typeparam>
        /// <param name="getValue">The delegate used to calculate the property value.</param>
        /// <param name="comparer">The optional equality comparer.</param>
        protected virtual ILazyProperty<T> CreateLazyProperty<T>(Func<T> getValue, IEqualityComparer<T> comparer)
        {
            return new LazyProperty<T>(NotifyValueChanged, getValue, comparer);
        }

        /// <summary>
        /// Creates an asynchronous property.
        /// </summary>
        /// <typeparam name="T">The type of the property value.</typeparam>
        /// <param name="getValueAsync">The delegate used to calculate the property value.</param>
        protected virtual IAsyncProperty<T> CreateAsyncProperty<T>(Func<CancellationToken, Task<T>> getValueAsync)
        {
            return new AsyncProperty<T>(NotifyValueChanged, getValueAsync);
        }

        /// <summary>
        /// Retrieves the specified property if it exists; otherwise, returns <value>null</value>.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        protected abstract IProperty GetProperty(string propertyName);

        /// <summary>
        /// Retrieves the specified property if it exists; otherwise, creates the property and returns it.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="createProperty">The delegate used to create the property.</param>
        /// <param name="propertyName">The name of the property.</param>
        protected abstract TProperty GetOrAddProperty<TProperty>(Func<TProperty> createProperty, string propertyName)
            where TProperty : IProperty;

        /// <summary>
        /// Invalidates the entire entity.
        /// </summary>
        /// <exception cref="NotSupportedException"/>
        protected abstract void InvalidateEntity();

        /// <summary>
        /// Notifies on change in property value.
        /// </summary>
        /// <typeparam name="T">The type of the property value.</typeparam>
        /// <param name="value">The new value of the property.</param>
        /// <param name="propertyName">The name of the property.</param>
        protected abstract void NotifyValueChanged<T>(T value, string propertyName);

        /// <summary>
        /// Indicates whether the notification delegate will be invoked on invalidation.
        /// </summary>
        protected abstract bool IsNotifyOnInvalidate { get; }

        /// <summary>
        /// Invalidates the specified property.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns><value>true</value> if successful.</returns>
        protected bool InvalidateProperty(string propertyName)
        {
            var property = GetProperty(propertyName);
            if (property == null)
                return false;
            property.Invalidate(IsNotifyOnInvalidate, propertyName);
            return true;
        }

        /// <summary>
        /// Invokes the value change notification delegate.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        protected void NotifyValueChanged(string propertyName)
        {
            _onValueChanged(propertyName);
        }
    }
}
