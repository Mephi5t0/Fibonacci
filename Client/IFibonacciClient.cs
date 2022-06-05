using System.Threading;
using System.Threading.Tasks;
using View.Fibonacci;

namespace Client
{
    public interface IFibonacciClient
    {
        Task<ClientResult<FibonacciCalculationResult>> CalculateNextNumberAsync(
            FibonacciCalculateInfo calculateInfo,
            CancellationToken token);
    }
}