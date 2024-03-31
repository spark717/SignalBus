using System;

namespace Spark
{
    public interface ISignalBus
    {
        public void Subscribe(object subscriber);
        public void Subscribe<T>(object subscriber, Action<T> action) where T : ISignal;
        public void Subscribe<T>(Action<T> action) where T : ISignal;
        public void Unsubscribe(object subscriber);
        public void Unsubscribe<T>(Action<T> action) where T : ISignal;
        public void Fire<T>(T signal) where T : ISignal;
    }
}