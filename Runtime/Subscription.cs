namespace Spark
{
    internal class Subscription<T>
    {
        public object Subscriber;
        public OnSignal<T> Action;
        public bool RemoveMark;
    }
}