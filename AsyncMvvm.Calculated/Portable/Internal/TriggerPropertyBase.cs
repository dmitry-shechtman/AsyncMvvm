using CalculatedProperties.Internal;
using System;
using System.ComponentModel;

namespace Ditto.AsyncMvvm.Calculated.Internal
{
    internal abstract class TriggerPropertyBase<TProperty, T> : SourcePropertyBase, ITriggerProperty<T>
        where TProperty : IProperty<T>
    {
        protected readonly TProperty _property;
        protected T _value;
        private Delegate _collectionChangedHandler;

        protected TriggerPropertyBase(Action<PropertyChangedEventArgs> onPropertyChanged, TProperty property)
            : base(onPropertyChanged)
        {
            this._property = property;
        }

        void IProperty.Invalidate(bool isNotify, string propertyName)
        {
            _property.Invalidate(isNotify, propertyName);
        }

        void ITriggerProperty<T>.NotifyValueChanged(T value, string propertyName)
        {
            SetPropertyName(propertyName);
            Detach(_value);
            _value = value;
            Attach(_value);
            Invalidate();
        }

        private void Attach(T value)
        {
            _collectionChangedHandler = ReflectionHelper.For<T>.AddEventHandler(this, value);
        }

        private void Detach(T value)
        {
            ReflectionHelper.For<T>.RemoveEventHandler(value, _collectionChangedHandler);
        }
    }
}
