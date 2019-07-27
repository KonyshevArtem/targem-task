using System;

namespace Calculator
{
    class Program
    {
        static void Main(string[] args)
        {
            EquationParser parser = new EquationParser();
            StringCalculator calculator = new StringCalculator(parser);
            string equation = string.Join("", args);
            try
            {
                float result = calculator.Calculate(equation);
                Console.WriteLine(result);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"ERROR: {exception.Message}");
            }
        }
    }
}
