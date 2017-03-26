using CalculatedProperties.Internal;
using System;
using System.ComponentModel;

namespace Ditto.AsyncMvvm.Calculated.Internal
{
    internal class LazyTriggerProperty<T> : TriggerPropertyBase<ILazyProperty<T>, T>, ILazyProperty<T>
    {
        public LazyTriggerProperty(Action<PropertyChangedEventArgs> onPropertyChanged, ILazyProperty<T> property)
            : base(onPropertyChanged, property)
        {
        }

        T ILazyProperty<T>.GetValue(string propertyName)
        {
            SetPropertyName(propertyName);
            DependencyTracker.Instance.Register(this);
            return _property.GetValue(propertyName);
        }

        void ILazyProperty<T>.SetValue(T value, string propertyName)
        {
            _property.SetValue(value, propertyName);
        }
    }
}
