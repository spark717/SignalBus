using System;
using System.Collections.Generic;
using System.Reflection;

namespace Spark
{
    internal class SignalHandler<T> : SignalHandlerBase where T : ISignal
    {
        private readonly List<Subscription<T>> _subscriptions = new List<Subscription<T>>();

        private int _callsCount;
        private bool _hasAnyRemoveMark;
        
        public void Fire(T signal)
        {
            _callsCount++;
            if (_callsCount >= SignalBusSettings.MaxCalls)
            {
                Log.Excetion(new Exception("Infinity signal call detected"));
                return;
            }
            
            try
            {
                for (int i = 0; i < _subscriptions.Count; i++)
                {
                    if (_subscriptions[i].RemoveMark == false)
                        _subscriptions[i].Action.Invoke(signal);
                }
            }
            catch (Exception e)
            {
                Log.Excetion(e);
            }

            if (_hasAnyRemoveMark)
                _subscriptions.RemoveAll(x => x.RemoveMark);

            _hasAnyRemoveMark = false;
            _callsCount--;
        }

        public void Subscribe(object subscriber, Action<T> action)
        {
            var newSubscription = new Subscription<T>()
            {
                Action = action,
                Subscriber = subscriber,
            };
            
            _subscriptions.Add(newSubscription);
        }

        public override void Subscribe(object subscriber, MethodInfo method)
        {
            var action = (Action<T>)method.CreateDelegate(typeof(Action<T>), subscriber);
            Subscribe(subscriber, action);
        }

        public override void Unsubscribe(object subscriber)
        {
            foreach (var subscription in _subscriptions)
            {
                if (subscription.Subscriber == subscriber)
                {
                    subscription.RemoveMark = true;
                    _hasAnyRemoveMark = true;
                }
            }
        }
        
        public void Unsubscribe(Action<T> action)
        {
            foreach (var subscription in _subscriptions)
            {
                if (subscription.Action == action)
                {
                    subscription.RemoveMark = true;
                    _hasAnyRemoveMark = true;
                }
            }
        }
    }
}