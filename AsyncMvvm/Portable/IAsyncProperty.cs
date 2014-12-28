using System.Runtime.CompilerServices;
using System.Threading;

namespace Ditto.AsyncMvvm
{
    /// <summary>
    /// Interface for asynchronous properties.
    /// </summary>
    /// <typeparam name="T">The type of the property value.</typeparam>
    public interface IAsyncProperty<T> : IProperty<T>
    {
        /// <summary>
        /// Gets the property value.
        /// </summary>
        /// <param name="token">The optional cancellation token.</param>
        /// <param name="listener">The optional task listener.</param>
        /// <param name="propertyName">The name of the property.</param>
        T GetValue(CancellationToken token = default(CancellationToken), ITaskListener listener = null, [CallerMemberName] string propertyName = null);
    }
}
