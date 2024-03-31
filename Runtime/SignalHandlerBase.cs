using System.Reflection;

namespace Spark
{
    internal abstract class SignalHandlerBase
    {
        public abstract void Subscribe(object subscriber, MethodInfo method);
        public abstract void Unsubscribe(object subscriber);
    }
}