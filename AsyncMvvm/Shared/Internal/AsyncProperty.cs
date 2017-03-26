using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ditto.AsyncMvvm.Internal
{
    /// <summary>
    /// Asynchronous property.
    /// </summary>
    /// <typeparam name="T">The type of the property value.</typeparam>
    internal class AsyncProperty<T> : PropertyBase<T>, IAsyncProperty<T>
    {
        private readonly Func<CancellationToken, Task<T>> _getValueAsync;
        private bool _isCalculating;

        /// <summary>
        /// Creates a new asynchronous property instance.
        /// </summary>
        /// <param name="onValueChanged">Value change notification delegate.</param>
        /// <param name="getValueAsync">The delegate used to calculate the property value.</param>
        public AsyncProperty(Action<T, string> onValueChanged, Func<CancellationToken, Task<T>> getValueAsync)
            : base(onValueChanged, null)
        {
            if (getValueAsync == null)
                throw new ArgumentNullException("getValueAsync");
            this._getValueAsync = getValueAsync;
        }

        /// <summary>
        /// Gets the property value.
        /// </summary>
        /// <param name="token">The optional cancellation token.</param>
        /// <param name="listener">The optional task listener.</param>
        /// <param name="propertyName">The name of the property.</param>
        public T GetValue(CancellationToken token, ITaskListener listener, string propertyName)
        {
            if (propertyName == null)
                throw new ArgumentNullException("propertyName");
            if (!IsValueValid)
                CalculateValue(token, listener, propertyName);
            return DoGetValue();
        }

        private void CalculateValue(CancellationToken token, ITaskListener listener, string propertyName)
        {
            if (!_isCalculating)
            {
                _isCalculating = true;
                StartGetValue(token, listener, propertyName);
            }
        }

        private void StartGetValue(CancellationToken token, ITaskListener listener, string propertyName)
        {
            var scheduler = TaskScheduler.FromCurrentSynchronizationContext();
            listener = listener ?? AggregateTaskListener.Empty;
            Task.Factory.StartNew(() => GetValueAsync(token, listener, propertyName),
                CancellationToken.None, TaskCreationOptions.None, scheduler);
        }

        private async Task GetValueAsync(CancellationToken token, ITaskListener listener, string propertyName)
        {
            listener.NotifyTaskStarting();
            T value = default(T);
            bool? isSuccess;
            try
            {
                value = await _getValueAsync(token);
                isSuccess = true;
            }
            catch (OperationCanceledException)
            {
                isSuccess = null;
            }
            catch
            {
                isSuccess = false;
            }
            finally
            {
                _isCalculating = false;
            }
            if (isSuccess == true)
                DoSetValue(value, propertyName);
            listener.NotifyTaskCompleted(isSuccess);
        }
    }
}
