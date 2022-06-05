using System;

namespace API.Fibonacci
{
    public sealed class FibonacciNotValidNumber : Exception
    {
        public FibonacciNotValidNumber(string message) : base(message)
        {
        }
    }
}