using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Spark
{
    public class SignalBus : ISignalBus
    {
        private readonly Dictionary<Type, SignalHandlerBase> _handlersBySignalType = new Dictionary<Type, SignalHandlerBase>();
        
        public void Subscribe(object subscriber)
        {
            if (subscriber == null)
            {
                Log.Excetion(new NullReferenceException("Signal subscriber is null"));
                return;
            }

            var methods = subscriber.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var methodInfo in methods)
            {
                if (Attribute.IsDefined(methodInfo, typeof(OnSignalAttribute)) == false)
                    continue;

                var prms = methodInfo.GetParameters();

                if (prms.Length != 1)
                {
                    Log.Excetion(new ArgumentException("Signal action has invalid arguments count"));
                    return;
                }

                var arg = prms.First();

                if (typeof(ISignal).IsAssignableFrom(arg.ParameterType) == false)
                {
                    Log.Excetion(new ArgumentException("Signal action has invalid argument type"));
                    return;
                }

                var handler = GetHandler(arg.ParameterType);
                handler.Subscribe(subscriber, methodInfo);
            }
        }

        public void Subscribe<T>(object subscriber, OnSignal<T> action) where T : ISignal
        {
            if (subscriber == null)
            {
                Log.Excetion(new NullReferenceException("Signal subscriber is null"));
                return;
            }
            
            if (action == null)
            {
                Log.Excetion(new NullReferenceException("Signal action is null"));
                return;
            }
            
            var handler = GetHandler<T>();
            handler.Subscribe(subscriber, action);
        }

        public void Subscribe<T>(OnSignal<T> action) where T : ISignal
        {
            if (action == null)
            {
                Log.Excetion(new NullReferenceException("Signal action is null"));
                return;
            }
            
            var handler = GetHandler<T>();
            handler.Subscribe(null, action);
        }

        public void Unsubscribe(object subscriber)
        {
            if (subscriber == null)
            {
                Log.Excetion(new NullReferenceException("Signal subscriber is null"));
                return;
            }

            foreach (var handler in _handlersBySignalType.Values)
            {
                handler.Unsubscribe(subscriber);
            }
        }

        public void Unsubscribe<T>(OnSignal<T> action) where T : ISignal
        {
            if (action == null)
            {
                Log.Excetion(new NullReferenceException("Signal action is null"));
                return;
            }

            var handler = GetHandler<T>();
            handler.Unsubscribe(action);
        }

        public void Fire<T>(T signal) where T : ISignal
        {
            var handler = GetHandler<T>();
            handler.Fire(signal);
        }

        private SignalHandler<T> GetHandler<T>() where T : ISignal
        {
            if (_handlersBySignalType.TryGetValue(typeof(T), out var handler))
                return (SignalHandler<T>)handler;

            var newHandler = new SignalHandler<T>();
            _handlersBySignalType[typeof(T)] = newHandler;
            return newHandler;
        }
        
        private SignalHandlerBase GetHandler(Type signalType)
        {
            if (_handlersBySignalType.TryGetValue(signalType, out var handler))
                return handler;

            var handlerType = typeof(SignalHandler<>).MakeGenericType(signalType);
            var newHandler = (SignalHandlerBase)Activator.CreateInstance(handlerType);
            _handlersBySignalType[signalType] = newHandler;
            return newHandler;
        }
    }
}