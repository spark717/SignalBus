using System;
using Spark;

public static class SignalBusExtensions
{
    public static Func<ISignalBus> GetSignalBus;

    public static void Fire<T>(this T signal) where T : ISignal
    {
        GetSignalBus().Fire(signal);
    }
}
