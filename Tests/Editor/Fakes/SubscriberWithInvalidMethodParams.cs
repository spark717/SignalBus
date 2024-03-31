using Spark;

public class SubscriberWithInvalidMethodParamType
{
    [OnSignal]
    private void OnSignal(int prm)
    {
    }
}