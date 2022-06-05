using View.Fibonacci;

namespace API.MessageBus
{
    public interface IMessageBusService
    {
        void SendCalculationResult(FibonacciCalculationResult calculationResult);
    }
}