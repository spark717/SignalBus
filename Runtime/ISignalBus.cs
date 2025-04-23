namespace Spark
{
    public delegate void OnSignal<T>(T signal);
    
    public interface ISignalBus
    {
        public void Subscribe(object subscriber);
        public void Subscribe<T>(object subscriber, OnSignal<T> action) where T : ISignal;
        public void Subscribe<T>(OnSignal<T> action) where T : ISignal;
        public void Unsubscribe(object subscriber);
        public void Unsubscribe<T>(OnSignal<T> action) where T : ISignal;
        public void Fire<T>(T signal) where T : ISignal;
    }
}