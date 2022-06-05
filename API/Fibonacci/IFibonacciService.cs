using View.Fibonacci;

namespace API.Fibonacci
{
    public interface IFibonacciService
    {
        FibonacciCalculationResult CalculateNextFibonacciNumber(FibonacciCalculateInfo calculateInfo);
    }
}