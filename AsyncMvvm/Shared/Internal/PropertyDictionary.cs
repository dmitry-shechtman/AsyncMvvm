using System;
using System.Collections.Generic;

namespace Ditto.AsyncMvvm.Internal
{
    /// <summary>
    /// Property dictionary.
    /// </summary>
    internal class PropertyDictionary
    {
        private readonly IDictionary<string, IProperty> _properties;

        /// <summary>
        /// Creates a new dictionary instance.
        /// </summary>
        public PropertyDictionary()
        {
            this._properties = new Dictionary<string, IProperty>();
        }

        /// <summary>
        /// Retrieves the specified property if it exists; otherwise, returns <value>null</value>.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        public IProperty GetValueOrDefault(string propertyName)
        {
            if (propertyName == null)
                throw new ArgumentNullException("propertyName");
            IProperty property;
            _properties.TryGetValue(propertyName, out property);
            return property;
        }

        /// <summary>
        /// Retrieves the specified property if it exists; otherwise, creates the property and returns it.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="createProperty">The delegate used to create the property.</param>
        /// <param name="propertyName">The name of the property.</param>
        public TProperty GetOrAdd<TProperty>(Func<TProperty> createProperty, string propertyName)
            where TProperty : IProperty
        {
            if (propertyName == null)
                throw new ArgumentNullException("propertyName");
            IProperty property;
            if (!_properties.TryGetValue(propertyName, out property))
            {
                property = createProperty();
                _properties.Add(propertyName, property);
            }
            return (TProperty)property;
        }

        /// <summary>
        /// Invalidates all known properties.
        /// </summary>
        /// <param name="isNotify">Indicates whether the notification delegate will be invoked.</param>
        public void Invalidate(bool isNotify)
        {
            foreach (var kvp in _properties)
                kvp.Value.Invalidate(isNotify, kvp.Key);
        }
    }
}
