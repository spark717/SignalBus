using NUnit.Framework;
using Spark;

public class SignalBusTests
{
    [Test]
    public void Fire()
    {
        var sb = new SignalBus();
        var signal = new Signal();
        sb.Fire(signal);
    }

    [Test]
    public void SubscribeAction()
    {
        var sb = new SignalBus();
        var subscriber = new Subscriber();
        var a = 7;

        sb.Subscribe<Signal>(subscriber.OnTestSignal);
        sb.Fire(new Signal()
        {
            A = a,
        });

        Assert.IsTrue(subscriber.A == a);
    }
    
    [Test]
    public void SubscribeObject()
    {
        var sb = new SignalBus();
        var subscriber = new Subscriber();
        var a = 7;

        sb.Subscribe(subscriber);
        sb.Fire(new Signal()
        {
            A = a,
        });

        Assert.IsTrue(subscriber.A == a);
    }
    
    [Test]
    public void SubscribeObjectWithAction()
    {
        var sb = new SignalBus();
        var subscriber = new Subscriber();
        var a = 7;

        sb.Subscribe<Signal>(subscriber, subscriber.OnTestSignal);
        sb.Fire(new Signal()
        {
            A = a,
        });

        Assert.IsTrue(subscriber.A == a);
    }
    
    [Test]
    public void UnsubscribeAction()
    {
        var sb = new SignalBus();
        var subscriber = new Subscriber();
        var a = 7;

        sb.Subscribe<Signal>(subscriber.OnTestSignal);
        sb.Unsubscribe<Signal>(subscriber.OnTestSignal);
        sb.Fire(new Signal()
        {
            A = a,
        });

        Assert.IsFalse(subscriber.A == a);
    }
    
    [Test]
    public void UnsubscribeObject()
    {
        var sb = new SignalBus();
        var subscriber = new Subscriber();
        var a = 7;

        sb.Subscribe(subscriber);
        sb.Unsubscribe(subscriber);
        sb.Fire(new Signal()
        {
            A = a,
        });

        Assert.IsFalse(subscriber.A == a);
    }
    
    [Test]
    public void UnsubscribeObjectAndLinkedAction()
    {
        var sb = new SignalBus();
        var subscriber = new Subscriber();
        var a = 7;

        sb.Subscribe(subscriber);
        sb.Subscribe<Signal>(subscriber, subscriber.OnTestSignal);
        sb.Unsubscribe(subscriber);
        sb.Fire(new Signal()
        {
            A = a,
        });

        Assert.IsFalse(subscriber.A == a);
    }
    
    [Test]
    public void UnsubscribeObjectExceptSeparateAction()
    {
        var sb = new SignalBus();
        var subscriber = new Subscriber();
        var a = 7;

        sb.Subscribe(subscriber);
        sb.Subscribe<Signal>(subscriber.OnTestSignal);
        sb.Unsubscribe(subscriber);
        sb.Fire(new Signal()
        {
            A = a,
        });

        Assert.IsTrue(subscriber.A == a);
    }
    
    [Test]
    public void ExceptionOnInfinityCall()
    {
        SignalBusSettings.LogExceptions = false;
        SignalBusSettings.MaxCalls = 5;
        var sb = new SignalBus();
        
        sb.Subscribe<Signal>(s =>
        {
            sb.Fire(new Signal());

        });

        Assert.Catch(() =>
        {
            sb.Fire(new Signal());
        });
    }
    
    [Test]
    public void ExceptionOnInvalidMethodArgsType()
    {
        SignalBusSettings.LogExceptions = false;
        var sb = new SignalBus();
        var subscriber = new SubscriberWithInvalidMethodParamType();

        Assert.Catch(() =>
        {
            sb.Subscribe(subscriber);
        });
    }
    
    [Test]
    public void ExceptionOnInvalidMethodArgsCount()
    {
        SignalBusSettings.LogExceptions = false;
        var sb = new SignalBus();
        var subscriber = new SubscriberWithInvalidMethodParamsCount();

        Assert.Catch(() =>
        {
            sb.Subscribe(subscriber);
        });
    }
    
    [Test]
    public void ExceptionOnNullArgs()
    {
        SignalBusSettings.LogExceptions = false;
        var sb = new SignalBus();

        Assert.Catch(() =>
        {
            sb.Subscribe(null);
        });
        
        Assert.Catch(() =>
        {
            sb.Subscribe<Signal>(null, _ => {});
        });
        
        Assert.Catch(() =>
        {
            sb.Subscribe<Signal>(null, null);
        });
    }
    
    [Test]
    public void UnsubscribeWhileFire()
    {
        var sb = new SignalBus();
        var subscriber1 = new Subscriber();
        var subscriber2 = new Subscriber();
        var a = 7;

        sb.Subscribe(subscriber1);
        sb.Subscribe<Signal>(_ => sb.Unsubscribe(subscriber2));
        sb.Subscribe(subscriber2);
        sb.Fire(new Signal()
        {
            A = a
        });

        Assert.IsTrue(subscriber1.A == a);
        Assert.IsFalse(subscriber2.A == a);
    }
    
    [Test]
    public void SubscribeWhileFire()
    {
        var sb = new SignalBus();
        var subscriber1 = new Subscriber();
        var subscriber2 = new Subscriber();
        var a = 7;

        sb.Subscribe(subscriber1);
        sb.Subscribe<Signal>(_ => sb.Subscribe(subscriber2));
        sb.Fire(new Signal()
        {
            A = a
        });

        Assert.IsTrue(subscriber1.A == a);
        Assert.IsTrue(subscriber2.A == a);
    }
}