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

        private bool IsUnaryOperator(List<IToken> tokens, int index)
        {
            return index == 0 || tokens[index - 1] is OperatorToken && tokens[index - 1].GetStringValue() != ")";
        }

        public void RemoveUnaryOperators(List<IToken> tokens)
        {
            for (int i = 0; i < tokens.Count; ++i)
            {
                IToken token = tokens[i];
                if (token.GetStringValue() == "-")
                {
                    if (IsUnaryOperator(tokens, i))
                    {
                        if (i == tokens.Count - 1 || !(tokens[i + 1] is OperandToken))
                            throw new ArgumentException("Invalid unary minus");
                        tokens.Insert(i, new OperatorToken("("));
                        tokens.Insert(i + 1, new OperandToken("0"));
                        tokens.Insert(i + 4, new OperatorToken(")"));
                    }
                }
                else if (token.GetStringValue() == "+")
                {
                    if (IsUnaryOperator(tokens, i))
                    {
                        tokens.RemoveAt(i);
                    }
                }
            }
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
            RemoveUnaryOperators(tokens);
            List<IToken> reversedPolishTokens = TransformToReversePolish(tokens);
            return CalculateReversePolishTokens(reversedPolishTokens);
        }
    }
}
