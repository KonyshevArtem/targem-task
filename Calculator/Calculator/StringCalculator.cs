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

        /// <summary>
        /// Метод, проверяющий является ли унарным токен, находящийся во входном листе на заданном индексе
        /// </summary>
        /// <param name="tokens">Лист токенов</param>
        /// <param name="index">Индекс проверяемого токена</param>
        /// <returns>Является ли токен унарным</returns>
        private bool IsUnaryOperator(List<IToken> tokens, int index)
        {
            return index == 0 || tokens[index - 1] is OperatorToken && tokens[index - 1].GetStringValue() != ")";
        }

        /// <summary>
        /// Метод, заменяющий унарные операторы на бинарные
        /// Алгоритм: https://philcurnow.wordpress.com/2015/01/24/conversion-of-expressions-from-infix-to-postfix-notation-in-c-part-2-unary-operators/
        /// </summary>
        /// <param name="tokens">Лист токенов с унарными операторами</param>
        /// <exception cref="ArgumentException">Выбрасывается, когда унарный минус стоит в конце строки или перед другим оператором</exception>
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

        /// <summary>
        /// Метод, преобразующий лист токенов в инфиксном порядке в постфиксный (обратная польская запись)
        /// Алгоритм: https://en.wikipedia.org/wiki/Shunting-yard_algorithm
        /// </summary>
        /// <param name="tokens">Лист токенов в инфиксном порядке без унарных операторов</param>
        /// <returns>Лист токенов в постфиксном порядке</returns>
        /// <exception cref="ArgumentException">Выбрасывается, при неправильном положении скобок в примере</exception>
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

        /// <summary>
        /// Метод, вычисляющий результат постфиксной записи
        /// Алгоритм: https://en.wikipedia.org/wiki/Reverse_Polish_notation#Postfix_evaluation_algorithm
        /// </summary>
        /// <param name="tokens">Лист токенов в постфиксном порядке</param>
        /// <returns>Результат вычисления</returns>
        /// <exception cref="ArgumentException">Выбрасывается, когда в постфиксной записи остаются неиспользованные операторы
        /// или когда не хватает операндов для применения очередного оператора</exception>
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
