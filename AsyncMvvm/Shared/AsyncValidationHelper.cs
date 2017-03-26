using Ditto.AsyncMvvm.Internal;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ditto.AsyncMvvm
{
    /// <summary>
    /// Asynchronous validation helper.
    /// </summary>
    /// <typeparam name="TError">The type of the validation error.</typeparam>
    public class AsyncValidationHelper<TError> : AsyncHelperBase, IAsyncValidationHelper<TError>
    {
        private readonly PropertyDictionary _errors;
        private readonly bool _isNotifyOnInvalidate;

        /// <summary>
        /// Creates a new validation helper instance.
        /// </summary>
        /// <param name="onErrorsChanged">Error collection change notification delegate.</param>
        /// <param name="isNotifyOnInvalidate">Indicates whether the notification delegate will be invoked on invalidation.</param>
        public AsyncValidationHelper(Action<string> onErrorsChanged, bool isNotifyOnInvalidate = true)
            : base(onErrorsChanged)
        {
            this._errors = new PropertyDictionary();
            this._isNotifyOnInvalidate = isNotifyOnInvalidate;
        }

        /// <summary>
        /// Gets the validation errors for a specified property or for the entire entity.
        /// </summary>
        /// <param name="getErrors">The delegate used to retrieve the validation errors.</param>
        /// <param name="propertyName">The name of the property to retrieve validation errors for,
        /// or <value>null</value> or <see cref="String.Empty"/> to retrieve entity-level errors.</param>
        public IEnumerable<TError> Get(Func<string, IEnumerable<TError>> getErrors, string propertyName)
        {
            if (getErrors == null)
                throw new ArgumentNullException("getErrors");
            if (propertyName == null)
                propertyName = string.Empty;
            return GetOrAddLazyProperty(() => getErrors(propertyName), null, propertyName).GetValue(propertyName);
        }

        /// <summary>
        /// Gets the validation errors for a specified property or for the entire entity.
        /// </summary>
        /// <param name="getErrorsAsync">The delegate used to retrieve the validation errors.</param>
        /// <param name="propertyName">The name of the property to retrieve validation errors for,
        /// or <value>null</value> or <see cref="String.Empty"/> to retrieve entity-level errors.</param>
        public IEnumerable<TError> AsyncGet(Func<string, Task<IEnumerable<TError>>> getErrorsAsync, string propertyName)
        {
            if (getErrorsAsync == null)
                throw new ArgumentNullException("getErrorsAsync");
            if (propertyName == null)
                propertyName = string.Empty;
            return GetOrAddAsyncProperty(_ => getErrorsAsync(propertyName), propertyName).GetValue(CancellationToken.None, null, propertyName);
        }

        /// <summary>
        /// Retrieves the specified property validator if it exists; otherwise, returns <value>null</value>.
        /// </summary>
        /// <param name="propertyName">The name of the property or <see cref="String.Empty"/>.</param>
        protected override IProperty GetProperty(string propertyName)
        {
            return _errors.GetValueOrDefault(propertyName);
        }

        /// <summary>
        /// Retrieves the specified property validator if it exists; otherwise, creates the validator and returns it.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="createValidator">The delegate used to create the validator.</param>
        /// <param name="propertyName">The name of the property or <see cref="String.Empty"/>.</param>
        protected override TProperty GetOrAddProperty<TProperty>(Func<TProperty> createValidator, string propertyName)
        {
            return _errors.GetOrAdd(createValidator, propertyName);
        }

        /// <summary>
        /// Invalidates the errors for the entire entity.
        /// </summary>
        protected override void InvalidateEntity()
        {
            _errors.Invalidate(false);
            NotifyValueChanged(string.Empty);
        }

        /// <summary>
        /// Notifies on change in the validation errors for a specified property or for the entire entity.
        /// </summary>
        /// <typeparam name="T">The type of the validation error collection.</typeparam>
        /// <param name="value">The new validation error collection.</param>
        /// <param name="propertyName">The name of the property or <see cref="String.Empty"/>.</param>
        protected override void NotifyValueChanged<T>(T value, string propertyName)
        {
            if (value != null || IsNotifyOnInvalidate)
                NotifyValueChanged(propertyName);
        }

        /// <summary>
        /// Indicates whether the notification delegate will be invoked on invalidation.
        /// </summary>
        protected override bool IsNotifyOnInvalidate
        {
            get { return _isNotifyOnInvalidate; }
        }
    }
}
