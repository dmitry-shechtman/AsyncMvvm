namespace Ditto.AsyncMvvm.Calculated.Internal
{
    internal interface ITriggerProperty<T> : IProperty<T>
    {
        void NotifyValueChanged(T value, string propertyName);
    }
}
