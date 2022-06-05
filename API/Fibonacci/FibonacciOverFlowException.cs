using System;

namespace API.Fibonacci
{
    public sealed class FibonacciOverFlowException : Exception
    {
        public FibonacciOverFlowException(string message) : base(message)
        {
        }
    }
}