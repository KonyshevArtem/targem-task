using System;
using System.Collections.Generic;

namespace Calculator
{
    public class StringCalculator
    {
        private readonly IParser<List<IToken>> equationParser;
        private static readonly Dictionary<string, int> OperandsPriority = new Dictionary<string, int>()
        {
            { "(", 0 },
            { "+", 1 },
            { "-", 1 },
            { "*", 2 },
            { "/", 2 }
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
                    else if (OperandsPriority.ContainsKey(token.GetStringValue()))
                    {
                        while (true)
                        {
                            if (stack.Count == 0 || OperandsPriority[stack.Peek().GetStringValue()] <= OperandsPriority[token.GetStringValue()])
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

        public float Calculate(string equation)
        {
            List<IToken> tokens = equationParser.Parse(equation);
            if (tokens.Count == 0)
            {
                throw new ArgumentException("Input equation is empty");
            }
            return 0;
        }
    }
}
