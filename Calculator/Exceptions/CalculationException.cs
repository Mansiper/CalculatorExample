using System;

namespace Calculator.Exceptions
{
    public class CalculationException : Exception
    {
        public CalculationException(string message)
            : base(message)
        {
        }
    }
}