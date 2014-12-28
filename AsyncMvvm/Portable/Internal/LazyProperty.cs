using System;
using System.Collections.Generic;

namespace Ditto.AsyncMvvm.Internal
{
    /// <summary>
    /// Lazy property.
    /// </summary>
    /// <typeparam name="T">The type of the property value.</typeparam>
    internal class LazyProperty<T> : PropertyBase<T>, ILazyProperty<T>
    {
        private readonly Func<T> _getValue;

        /// <summary>
        /// Creates a new lazy property instance.
        /// </summary>
        /// <param name="onValueChanged">Value change notification delegate.</param>
        /// <param name="getValue">The delegate used to calculate the property value.</param>
        /// <param name="comparer">The optional equality comparer.</param>
        public LazyProperty(Action<T, string> onValueChanged, Func<T> getValue, IEqualityComparer<T> comparer)
            : base(onValueChanged, comparer)
        {
            if (getValue == null)
                throw new ArgumentNullException("getValue");
            this._getValue = getValue;
        }

        /// <summary>
        /// Gets the property value.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        public T GetValue(string propertyName)
        {
            if (propertyName == null)
                throw new ArgumentNullException("propertyName");
            if (!IsValueValid)
                CalculateValue();
            return DoGetValue();
        }

        /// <summary>
        /// Sets the property value.
        /// </summary>
        /// <param name="value">The new property value.</param>
        /// <param name="propertyName">The name of the property.</param>
        public void SetValue(T value, string propertyName)
        {
            if (propertyName == null)
                throw new ArgumentNullException("propertyName");
            DoSetValue(value, propertyName);
        }

        private void CalculateValue()
        {
            var value = _getValue();
            DoSetValue(value);
        }
    }
}
