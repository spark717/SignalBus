#if SPARK_DI
namespace Spark
{
    public class SignalBusProcessor : IServiceProcessor
    {
        private readonly ISignalBus _signalBus;

        public SignalBusProcessor(ISignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        
        public void OnServiceCreated(object service)
        {
            if (service is ISignalReceiver)
                _signalBus.Subscribe(service);
        }

        public void OnServiceDestroyed(object service)
        {
            if (service is ISignalReceiver)
                _signalBus.Unsubscribe(service);
        }
    }
}
#endif