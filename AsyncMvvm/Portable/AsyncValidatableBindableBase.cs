using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Ditto.AsyncMvvm
{
    /// <summary>
    /// Base class for entities with asynchronous validation support.
    /// </summary>
    /// <typeparam name="TPropertyHelper">The type of the property helper.</typeparam>
    /// <typeparam name="TError">The type of the validation errors.</typeparam>
    public abstract class AsyncValidatableBindableBase<TPropertyHelper, TError> : ValidatableBindableBase<TPropertyHelper, TError>, INotifyDataErrorInfo
        where TPropertyHelper : IAsyncPropertyHelper
    {
        /// <summary>
        /// Creates a new instance of the entity.
        /// </summary>
        /// <param name="isNotifyOnInvalidate">Indicates whether the notification delegate will be invoked on invalidation.</param>
        protected AsyncValidatableBindableBase(bool isNotifyOnInvalidate = true)
            : base(isNotifyOnInvalidate)
        {
        }

        /// <summary>
        /// Gets the validation errors for a specified property or for the entire entity.
        /// </summary>
        /// <param name="propertyName">The name of the property to retrieve validation errors for,
        /// or <value>null</value> or <see cref="String.Empty"/> to retrieve entity-level errors.</param>
        protected override IEnumerable<TError> GetErrors(string propertyName)
        {
            return Errors.AsyncGet(GetErrorsAsync, propertyName);
        }

        /// <summary>
        /// Gets the validation errors for a specified property or for the entire entity.
        /// </summary>
        /// <param name="propertyName">The name of the property to retrieve validation errors for,
        /// or <value>null</value> or <see cref="String.Empty"/> to retrieve entity-level errors.</param>
        protected override IEnumerable<TError> GetErrorsOverride(string propertyName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the validation errors for a specified property or for the entire entity.
        /// </summary>
        /// <param name="propertyName">The name of the property to retrieve validation errors for,
        /// or <value>null</value> or <see cref="String.Empty"/> to retrieve entity-level errors.</param>
        protected abstract Task<IEnumerable<TError>> GetErrorsAsync(string propertyName);
    }

    /// <summary>
    /// Base class for entities with asynchronous validation support.
    /// </summary>
    /// <typeparam name="TError">The type of the validation errors.</typeparam>
    public abstract class AsyncValidatableBindableBase<TError> : AsyncValidatableBindableBase<IAsyncPropertyHelper, TError>
    {
        private readonly AsyncPropertyHelper _propertyHelper;

        /// <summary>
        /// Creates a new instance of the entity.
        /// </summary>
        /// <param name="isNotifyOnInvalidate">Indicates whether the notification delegate will be invoked on invalidation.</param>
        protected AsyncValidatableBindableBase(bool isNotifyOnInvalidate = true)
            : base(isNotifyOnInvalidate)
        {
            this._propertyHelper = new AsyncPropertyHelper(OnPropertyChanged);
        }

        /// <summary>
        /// Asynchronous property helper.
        /// </summary>
        protected override IAsyncPropertyHelper Property
        {
            get { return _propertyHelper; }
        }
    }
}
