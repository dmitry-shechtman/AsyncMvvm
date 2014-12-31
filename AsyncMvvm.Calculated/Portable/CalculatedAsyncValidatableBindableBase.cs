namespace Ditto.AsyncMvvm.Calculated
{
    /// <summary>
    /// Base class for entities with validation support and with asynchronous and calculated properties.
    /// </summary>
    public abstract class CalculatedAsyncValidatableBindableBase<TError> : AsyncValidatableBindableBase<ICalculatedAsyncPropertyHelper, TError>
    {
        private readonly CalculatedAsyncPropertyHelper _propertyHelper;

        /// <summary>
        /// Creates a new instance of the entity.
        /// </summary>
        /// <param name="isNotifyOnInvalidate">Indicates whether the notification delegate will be invoked on invalidation.</param>
        protected CalculatedAsyncValidatableBindableBase(bool isNotifyOnInvalidate = true)
            : base(isNotifyOnInvalidate)
        {
            this._propertyHelper = new CalculatedAsyncPropertyHelper(OnPropertyChanged);
        }

        /// <summary>
        /// Asynchronous property helper.
        /// </summary>
        protected override ICalculatedAsyncPropertyHelper Property
        {
            get { return _propertyHelper; }
        }
    }
}
