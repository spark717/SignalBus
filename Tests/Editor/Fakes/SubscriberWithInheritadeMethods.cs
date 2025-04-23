using Spark;

public class SubscriberWithInheritadeMethods : Subscriber
{
    public int NewA;
    public int NewB;
    public int NewC;
    
    [OnSignal]
    private void OnNewPrivateSignal(Signal signal)
    {
        NewA += signal.A;
    }
    
    [OnSignal]
    public void OnNewPublicSignal(Signal signal)
    {
        NewB += signal.B;
    }
    
    [OnSignal]
    protected void OnNewProtectedSignal(Signal signal)
    {
        NewC += signal.C;
    }
}