using System;

namespace Calculator
{
    class Program
    {
        static void Main(string[] args)
        {
            string equation = string.Join("", args);
            if (equation.Length == 0)
                throw new ArgumentException("Equation string is empty");
        }
    }
}
