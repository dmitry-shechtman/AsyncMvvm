namespace Ditto.AsyncMvvm.Calculated
{
    /// <summary>
    /// Base class for entities with asynchronous and calculated properties.
    /// </summary>
    public abstract class CalculatedAsyncBindableBase : AsyncBindableBase<ICalculatedAsyncPropertyHelper>
    {
        private readonly CalculatedAsyncPropertyHelper _propertyHelper;

        /// <summary>
        /// Creates a new instance of the entity.
        /// </summary>
        protected CalculatedAsyncBindableBase()
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
