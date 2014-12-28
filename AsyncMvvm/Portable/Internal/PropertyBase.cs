using System;
using System.Collections.Generic;

namespace Ditto.AsyncMvvm.Internal
{
    /// <summary>
    /// Base class for properties.
    /// </summary>
    internal abstract class PropertyBase<T> : IProperty
    {
        private readonly Action<T, string> _onValueChanged;
        private readonly IEqualityComparer<T> _comparer;
        private T _value;
        private bool _isValueValid;

        /// <summary>
        /// Creates a new property instance.
        /// </summary>
        /// <param name="onValueChanged">Value change notification delegate.</param>
        /// <param name="comparer">The optional equality comparer.</param>
        protected PropertyBase(Action<T, string> onValueChanged, IEqualityComparer<T> comparer)
        {
            this._onValueChanged = onValueChanged;
            this._comparer = comparer ?? EqualityComparer<T>.Default;
        }

        /// <summary>
        /// Invalidates the property.
        /// </summary>
        /// <param name="isNotify">Indicates whether the notification delegate will be invoked.</param>
        /// <param name="propertyName">The name of the property.</param>
        public void Invalidate(bool isNotify, string propertyName)
        {
            if (propertyName == null)
                throw new ArgumentNullException("propertyName");
            _value = default(T);
            _isValueValid = false;
            if (isNotify)
                NotifyValueChanged(propertyName);
        }

        /// <summary>
        /// Gets the property value.
        /// </summary>
        protected T DoGetValue()
        {
            return _value;
        }

        /// <summary>
        /// Sets the property value.
        /// </summary>
        /// <param name="value">The new property value.</param>
        /// <param name="propertyName">The name of the property.</param>
        protected void DoSetValue(T value, string propertyName)
        {
            if (!_comparer.Equals(_value, value))
            {
                DoSetValue(value);
                NotifyValueChanged(propertyName);
            }
        }

        /// <summary>
        /// Sets the property value.
        /// </summary>
        /// <param name="value">The new property value.</param>
        protected void DoSetValue(T value)
        {
            _value = value;
            _isValueValid = true;
        }

        /// <summary>
        /// Indicates whether the property value is valid.
        /// </summary>
        protected bool IsValueValid
        {
            get { return _isValueValid; }
        }

        /// <summary>
        /// Notifies on change in property value.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        private void NotifyValueChanged(string propertyName)
        {
            _onValueChanged(_value, propertyName);
        }
    }
}
