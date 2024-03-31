using Spark;

public class Subscriber
{
    public int A;
    
    public void OnTestSignal(Signal signal)
    {
        A += signal.A;
    }
    
    [OnSignal]
    private void OnSignal(Signal signal)
    {
        A += signal.A;
    }
}