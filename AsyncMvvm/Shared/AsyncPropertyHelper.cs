using Ditto.AsyncMvvm.Internal;
using System;

namespace Ditto.AsyncMvvm
{
    /// <summary>
    /// Asynchronous property helper.
    /// </summary>
    public class AsyncPropertyHelper : AsyncPropertyHelperBase
    {
        private readonly PropertyDictionary _properties;

        /// <summary>
        /// Creates a new property helper instance.
        /// </summary>
        /// <param name="onPropertyChanged">Property change notification delegate.</param>
        public AsyncPropertyHelper(Action<string> onPropertyChanged)
            : base(onPropertyChanged)
        {
            this._properties = new PropertyDictionary();
        }

        /// <summary>
        /// Retrieves the specified property if it exists; otherwise, returns <value>null</value>.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        protected override IProperty GetProperty(string propertyName)
        {
            return _properties.GetValueOrDefault(propertyName);
        }

        /// <summary>
        /// Retrieves the specified property if it exists; otherwise, creates the property and returns it.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="createProperty">The delegate used to create the property.</param>
        /// <param name="propertyName">The name of the property.</param>
        protected override TProperty GetOrAddProperty<TProperty>(Func<TProperty> createProperty, string propertyName)
        {
            return _properties.GetOrAdd(createProperty, propertyName);
        }

        /// <summary>
        /// Invalidates the entire entity.
        /// </summary>
        protected override void InvalidateEntity()
        {
            _properties.Invalidate(false);
            NotifyValueChanged(string.Empty);
        }

        /// <summary>
        /// Notifies on change in property value.
        /// </summary>
        /// <typeparam name="T">The type of the property value.</typeparam>
        /// <param name="value">The new value of the property.</param>
        /// <param name="propertyName">The name of the property.</param>
        protected override void NotifyValueChanged<T>(T value, string propertyName)
        {
            NotifyValueChanged(propertyName);
        }
    }
}
