using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ditto.AsyncMvvm
{
    /// <summary>
    /// Interface for asynchronous validation helpers.
    /// </summary>
    /// <typeparam name="TError">The type of the validation error.</typeparam>
    public interface IAsyncValidationHelper<TError>
    {
        /// <summary>
        /// Gets the validation errors for a specified property or for the entire entity.
        /// </summary>
        /// <param name="getErrors">The delegate used to retrieve the validation errors.</param>
        /// <param name="propertyName">The name of the property to retrieve validation errors for,
        /// or <value>null</value> or <see cref="String.Empty"/> to retrieve entity-level errors.</param>
        IEnumerable<TError> Get(Func<string, IEnumerable<TError>> getErrors, string propertyName);

        /// <summary>
        /// Gets the validation errors for a specified property or for the entire entity.
        /// </summary>
        /// <param name="getErrorsAsync">The delegate used to retrieve the validation errors.</param>
        /// <param name="propertyName">The name of the property to retrieve validation errors for,
        /// or <value>null</value> or <see cref="String.Empty"/> to retrieve entity-level errors.</param>
        IEnumerable<TError> AsyncGet(Func<string, Task<IEnumerable<TError>>> getErrorsAsync, string propertyName);

        /// <summary>
        /// Invalidates the errors for a specified property or for the entire entity.
        /// </summary>
        /// <param name="propertyName">The name of the property to invalidate the errors for,
        /// or <value>null</value> or <see cref="String.Empty"/> to invalidate entity-level errors.</param>
        /// <returns><value>true</value> if successful.</returns>
        bool Invalidate(string propertyName);
    }
}
