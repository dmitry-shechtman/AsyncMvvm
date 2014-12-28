using CalculatedProperties.Internal;
using System;
using System.ComponentModel;
using System.Threading;

namespace Ditto.AsyncMvvm.Calculated.Internal
{
    internal class AsyncTriggerProperty<T> : TriggerPropertyBase<IAsyncProperty<T>, T>, IAsyncProperty<T>
    {
        public AsyncTriggerProperty(Action<PropertyChangedEventArgs> onPropertyChanged, IAsyncProperty<T> property)
            : base(onPropertyChanged, property)
        {
        }

        T IAsyncProperty<T>.GetValue(CancellationToken token, ITaskListener listener, string propertyName)
        {
            SetPropertyName(propertyName);
            DependencyTracker.Instance.Register(this);
            return _property.GetValue(token, listener, propertyName);
        }
    }
}
