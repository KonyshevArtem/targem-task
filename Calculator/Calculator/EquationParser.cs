using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Calculator
{
    public interface IParser<out T>
    {
        T Parse(string input);
    }

    public interface IToken
    {
        string GetStringValue();
        float GetFloatValue();
    }

    public class OperandToken : IToken
    {
        private readonly float value;

        public OperandToken(string value)
        {
            value = value.Replace('.', ',');
            if (!float.TryParse(value, out this.value))
            {
                throw new ArgumentException($"Can not parse {value} as number");
            }
        }

        public OperandToken(float value)
        {
            this.value = value;
        }

        public string GetStringValue()
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        public float GetFloatValue()
        {
            return value;
        }
    }

    public class OperatorToken : IToken
    {
        private readonly string value;

        public OperatorToken(string value)
        {
            this.value = value;
        }

        public string GetStringValue()
        {
            return value;
        }

        public float GetFloatValue()
        {
            throw new InvalidOperationException("Operator can not be cast to number");
        }
    }

    public class EquationParser : IParser<List<IToken>>
    {
        private static readonly HashSet<char> PossibleOperands = new HashSet<char> { '+', '-', '*', '/', '(', ')' };

        public List<IToken> Parse(string equation)
        {
            List<IToken> tokens = new List<IToken>();
            equation = Regex.Replace(equation, @"\s+", string.Empty);
            bool isFloat = false;
            StringBuilder numberBuilder = new StringBuilder();
            foreach (char c in equation)
            {
                if (char.IsDigit(c) || c == '.' || c == ',')
                {
                    if (c == '.' || c == ',')
                    {
                        if (isFloat)
                            throw new ArgumentException("Can't parse float with two dots");
                        isFloat = true;
                    }

                    numberBuilder.Append(c);
                }
                else if (PossibleOperands.Contains(c))
                {
                    if (numberBuilder.Length > 0)
                    {
                        tokens.Add(new OperandToken(numberBuilder.ToString()));
                        isFloat = false;
                        numberBuilder.Clear();
                    }

                    tokens.Add(new OperatorToken(c.ToString()));
                }
                else
                {
                    throw new ArgumentException($"Unknown character: {c}");
                }
            }

            if (numberBuilder.Length > 0)
                tokens.Add(new OperandToken(numberBuilder.ToString()));
            return tokens;
        }
    }
}
