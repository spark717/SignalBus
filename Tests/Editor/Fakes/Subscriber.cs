using Spark;

public class Subscriber
{
    public int A;
    public int B;
    public int C;
    
    public void OnTestSignal(Signal signal)
    {
        A += signal.A;
    }
    
    [OnSignal]
    private void OnPrivateSignal(Signal signal)
    {
        A += signal.A;
    }
    
    [OnSignal]
    public void OnPublicSignal(Signal signal)
    {
        B += signal.B;
    }
    
    [OnSignal]
    protected void OnProtectedSignal(Signal signal)
    {
        C += signal.C;
    }
}