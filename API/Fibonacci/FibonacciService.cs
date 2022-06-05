using System;
using View.Fibonacci;

namespace API.Fibonacci
{
    public sealed class FibonacciService : IFibonacciService
    {
        public FibonacciCalculationResult CalculateNextFibonacciNumber(FibonacciCalculateInfo calculateInfo)
        {
            if (calculateInfo == null)
            {
                throw new ArgumentNullException(nameof(calculateInfo));
            }

            var fibonacciPosition = GetFibonacciPositionByNumber(calculateInfo.CurrentNumber);
            var previousFibonacciNumber = GetFibonacciNumberByPosition(fibonacciPosition - 1);

            if (previousFibonacciNumber > long.MaxValue - calculateInfo.CurrentNumber)
            {
                throw new FibonacciOverFlowException($"Cannot calculate fibonacci number greater than {long.MaxValue}.");
            }

            var calculationResult = new FibonacciCalculationResult
            {
                NextNumber = previousFibonacciNumber + calculateInfo.CurrentNumber,
            };
            return calculationResult;
        }

        private static long GetFibonacciNumberByPosition(int position)
        {
            var curPosition = 1;
            long curNumber = 1;
            long nextNumber = 1;

            while (curPosition < position)
            {
                nextNumber += curNumber;
                curNumber = nextNumber - curNumber;
                curPosition++;
            }

            return curNumber;
        }

        private static int GetFibonacciPositionByNumber(long number)
        {
            if (number is 0 or 1)
            {
                return (int) (number + 1);
            }

            var curPosition = 1;
            long curNumber = 1;
            long nextNumber = 1;

            while (curNumber <= number)
            {
                if (curNumber == number)
                {
                    return curPosition;
                }

                nextNumber += curNumber;
                curNumber = nextNumber - curNumber;
                curPosition++;
            }

            throw new FibonacciNotValidNumber($"Number {number} is not fibonacci number.");
        }
    }
}