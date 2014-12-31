using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace Ditto.AsyncMvvm
{
    /// <summary>
    /// Base class for entities with validation support.
    /// </summary>
    /// <typeparam name="TPropertyHelper">The type of the property helper.</typeparam>
    /// <typeparam name="TError">The type of the validation errors.</typeparam>
    public abstract class ValidatableBindableBase<TPropertyHelper, TError> : AsyncBindableBase<TPropertyHelper>, INotifyDataErrorInfo
        where TPropertyHelper : IAsyncPropertyHelper
    {
        private readonly AsyncValidationHelper<TError> _errors;

        /// <summary>
        /// Creates a new instance of the entity.
        /// </summary>
        /// <param name="isNotifyOnInvalidate">Indicates whether the notification delegate will be invoked on invalidation.</param>
        protected ValidatableBindableBase(bool isNotifyOnInvalidate = true)
        {
            this._errors = new AsyncValidationHelper<TError>(RaiseErrorsChanged, isNotifyOnInvalidate);
            this.PropertyChanged += OnPropertyChanged;
        }

        /// <summary>
        /// Validation helper.
        /// </summary>
        protected IAsyncValidationHelper<TError> Errors
        {
            get { return _errors; }
        }

        /// <summary>
        /// Gets a value that indicates whether the entity has validation errors.
        /// </summary>
        public bool HasErrors
        {
            get { return true; }
        }

        /// <summary>
        /// Occurs when the validation errors have changed for a property or for the entire entity.
        /// </summary>
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        /// <summary>
        /// Raises <see cref="INotifyDataErrorInfo.ErrorsChanged"/>.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        protected void RaiseErrorsChanged(string propertyName)
        {
            var eventHandler = ErrorsChanged;
            if (eventHandler != null)
                eventHandler(this, new DataErrorsChangedEventArgs(propertyName));
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Errors.Invalidate(e.PropertyName);
        }

        IEnumerable INotifyDataErrorInfo.GetErrors(string propertyName)
        {
            return GetErrors(propertyName);
        }

        /// <summary>
        /// Gets the validation errors for a specified property or for the entire entity.
        /// </summary>
        /// <param name="propertyName">The name of the property to retrieve validation errors for,
        /// or <value>null</value> or <see cref="String.Empty"/> to retrieve entity-level errors.</param>
        protected virtual IEnumerable<TError> GetErrors(string propertyName)
        {
            return Errors.Get(GetErrorsOverride, propertyName);
        }

        /// <summary>
        /// Gets the validation errors for a specified property or for the entire entity.
        /// </summary>
        /// <param name="propertyName">The name of the property to retrieve validation errors for,
        /// or <value>null</value> or <see cref="String.Empty"/> to retrieve entity-level errors.</param>
        protected abstract IEnumerable<TError> GetErrorsOverride(string propertyName);
    }

    /// <summary>
    /// Base class for entities with validation support.
    /// </summary>
    /// <typeparam name="TError">The type of the validation errors.</typeparam>
    public abstract class ValidatableBindableBase<TError> : ValidatableBindableBase<IAsyncPropertyHelper, TError>
    {
        private readonly AsyncPropertyHelper _propertyHelper;

        /// <summary>
        /// Creates a new instance of the entity.
        /// </summary>
        /// <param name="isNotifyOnInvalidate">Indicates whether the notification delegate will be invoked on invalidation.</param>
        protected ValidatableBindableBase(bool isNotifyOnInvalidate = true)
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
