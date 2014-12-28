using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Ditto.AsyncMvvm.Calculated
{
    /// <summary>
    /// Interface for asynchronous and calculated property helpers.
    /// </summary>
    public interface ICalculatedAsyncPropertyHelper : IAsyncPropertyHelper
    {
        /// <summary>
        /// Implements the getter for a trigger property.
        /// </summary>
        /// <typeparam name="T">The type of the property value.</typeparam>
        /// <param name="value">The initial value of the property.</param>
        /// <param name="comparer">The optional comparer used to determine when the value of the property has changed.
        /// If this is specified, then the same comparer should be passed to the setter implementation.</param>
        /// <param name="propertyName">The name of the property.</param>
        T Get<T>(T value, IEqualityComparer<T> comparer = null, [CallerMemberName] string propertyName = null);

        /// <summary>
        /// Implements the setter for a trigger property.
        /// </summary>
        /// <typeparam name="T">The type of the property value.</typeparam>
        /// <param name="value">The new value of the property.</param>
        /// <param name="comparer">The optional comparer used to determine when the value of the property has changed.
        /// If this is specified, then the same comparer should be passed to the setter implementation.</param>
        /// <param name="propertyName">The name of the property.</param>
        void Set<T>(T value, IEqualityComparer<T> comparer = null, [CallerMemberName] string propertyName = null);

        /// <summary>
        /// Implements the getter for a calculated property.
        /// </summary>
        /// <typeparam name="T">The type of the property value.</typeparam>
        /// <param name="calculateValue">The delegate used to calculate the property value.</param>
        /// <param name="propertyName">The name of the property.</param>
        T Calculated<T>(Func<T> calculateValue, [CallerMemberName] string propertyName = null);
    }
}
