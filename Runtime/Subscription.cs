using System;

namespace Spark
{
    internal class Subscription<T>
    {
        public object Subscriber;
        public Action<T> Action;
        public bool RemoveMark;
    }
}