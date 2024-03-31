using Spark;

public class SubscriberWithInvalidMethodParamsCount
{
    [OnSignal]
    private void OnSignal(Signal signal, int prm)
    {
    }
}