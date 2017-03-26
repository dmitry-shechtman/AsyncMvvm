using System.Runtime.CompilerServices;

namespace Ditto.AsyncMvvm
{
    /// <summary>
    /// Interface for lazy properties.
    /// </summary>
    /// <typeparam name="T">The type of the property value.</typeparam>
    public interface ILazyProperty<T> : IProperty<T>
    {
        /// <summary>
        /// Gets the property value.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        T GetValue([CallerMemberName] string propertyName = null);

        /// <summary>
        /// Sets the property value.
        /// </summary>
        /// <param name="value">The new property value.</param>
        /// <param name="propertyName">The name of the property.</param>
        void SetValue(T value, [CallerMemberName] string propertyName = null);
    }
}
