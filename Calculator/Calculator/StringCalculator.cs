using System;
using System.Collections.Generic;

namespace Calculator
{
    public class StringCalculator
    {
        private readonly IParser<List<IToken>> equationParser;
        private static readonly Dictionary<string, int> OperatorsPriority = new Dictionary<string, int>()
        {
            { "(", 0 },
            { "+", 1 },
            { "-", 1 },
            { "*", 2 },
            { "/", 2 }
        };

        private static readonly Dictionary<string, Func<float, float, float>> OperatorsFunction =
            new Dictionary<string, Func<float, float, float>>
            {
                { "+", (a, b) => a + b },
                { "-", (a, b) => a - b },
                { "*", (a, b) => a * b },
                { "/", (a, b) => a / b }
            };

        public StringCalculator(IParser<List<IToken>> equationParser)
        {
            this.equationParser = equationParser;
        }

        public List<IToken> TransformToReversePolish(List<IToken> tokens)
        {
            List<IToken> reversePolishTokens = new List<IToken>();
            Stack<IToken> stack = new Stack<IToken>();
            foreach (IToken token in tokens)
            {
                if (token is OperandToken)
                    reversePolishTokens.Add(token);
                else
                {
                    if (token.GetStringValue() == "(")
                        stack.Push(token);
                    else if (token.GetStringValue() == ")")
                    {
                        while (true)
                        {
                            if (stack.Count == 0)
                                throw new ArgumentException("Invalid brackets");
                            if (stack.Peek().GetStringValue() == "(")
                            {
                                stack.Pop();
                                break;
                            }
                            reversePolishTokens.Add(stack.Pop());
                        }
                    }
                    else if (OperatorsPriority.ContainsKey(token.GetStringValue()))
                    {
                        while (true)
                        {
                            if (stack.Count == 0 || OperatorsPriority[stack.Peek().GetStringValue()] <= OperatorsPriority[token.GetStringValue()])
                                break;
                            reversePolishTokens.Add(stack.Pop());
                        }
                        stack.Push(token);
                    }
                }
            }
            while (stack.Count > 0)
            {
                if (stack.Peek().GetStringValue() == "(")
                    throw new ArgumentException("Invalid brackets");
                reversePolishTokens.Add(stack.Pop());
            }
            return reversePolishTokens;
        }

        public float CalculateReversePolishTokens(List<IToken> tokens)
        {
            Stack<IToken> stack = new Stack<IToken>();
            foreach (IToken token in tokens)
            {
                if (token is OperandToken)
                    stack.Push(token);
                else
                {
                    if (stack.Count < 2)
                        throw new ArgumentException("Invalid equation");
                    IToken a = stack.Pop();
                    IToken b = stack.Pop();
                    float result = OperatorsFunction[token.GetStringValue()](b.GetFloatValue(), a.GetFloatValue());
                    stack.Push(new OperandToken(result));
                }
            }
            if (stack.Count != 1)
                throw new ArgumentException("Invalid equation");
            return stack.Peek().GetFloatValue();
        }

        public float Calculate(string equation)
        {
            List<IToken> tokens = equationParser.Parse(equation);
            List<IToken> reversedPolishTokens = TransformToReversePolish(tokens);
            return CalculateReversePolishTokens(reversedPolishTokens);
        }
    }
}
