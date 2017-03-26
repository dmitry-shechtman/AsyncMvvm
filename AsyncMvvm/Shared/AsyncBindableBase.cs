namespace Ditto.AsyncMvvm
{
    /// <summary>
    /// Base class for entities with asynchronous properties.
    /// </summary>
    /// <typeparam name="TPropertyHelper">The type of the asynchronous property helper.</typeparam>
    public abstract class AsyncBindableBase<TPropertyHelper> : BindableBase
    {
        /// <summary>
        /// Asynchronous property helper.
        /// </summary>
        protected abstract TPropertyHelper Property
        {
            get;
        }
    }

    /// <summary>
    /// Base class for entities with asynchronous properties.
    /// </summary>
    public abstract class AsyncBindableBase : AsyncBindableBase<IAsyncPropertyHelper>
    {
        private readonly AsyncPropertyHelper _propertyHelper;

        /// <summary>
        /// Creates a new instance of the entity.
        /// </summary>
        protected AsyncBindableBase()
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
