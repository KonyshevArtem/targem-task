using System;
using System.Collections.Generic;
using System.Linq;
using Calculator;
using NUnit.Framework;

namespace CalculatorTests
{
    [TestFixture]
    public class CalculatorTests
    {
        private StringCalculator calculator;

        private static List<IToken> SimpleParse(string input)
        {
            List<IToken> tokens = new List<IToken>();
            foreach (char c in input)
            {
                if (char.IsDigit(c))
                    tokens.Add(new OperandToken(c.ToString()));
                else
                    tokens.Add(new OperatorToken(c.ToString()));
            }
            return tokens;
        }

        [SetUp]
        public void SetUp()
        {
            calculator = new StringCalculator(null);
        }

        private static object[] reversePolishTransformationTestSource =
        {
            new object[] { SimpleParse(""), ""},
            new object[] { SimpleParse("1"), "1"},
            new object[] { SimpleParse("12"), "12"},
            new object[] { SimpleParse("()"), ""},
            new object[] { SimpleParse("(2)"), "2"},
            new object[] { SimpleParse("+"), "+"},
            new object[] { SimpleParse("2+2"), "22+"},
            new object[] { SimpleParse("2+2*2"), "222*+"},
            new object[] { SimpleParse("2*2+2"), "22*2+"},
            new object[] { SimpleParse("(2+2)*2"), "22+2*"},
            new object[] { SimpleParse("2*(2+2)"), "222+*"},
        };

        [Test, TestCaseSource(nameof(reversePolishTransformationTestSource))]
        public void ReversePolishFunction_ShouldTransformToReversePolishNotation(List<IToken> tokens, string expectedReversePolishEquation)
        {
            List<IToken> reversePolishTokens = calculator.TransformToReversePolish(tokens);
            string reversePolishEquation = string.Join("", reversePolishTokens.Select(token => token.GetStringValue()));
            Assert.AreEqual(expectedReversePolishEquation, reversePolishEquation);
        }

        private static object[] invalidBracketsTestSource =
        {
            new object[] {SimpleParse("(")},
            new object[] {SimpleParse(")")},
            new object[] {SimpleParse("2)")}
        };

        [Test, TestCaseSource(nameof(invalidBracketsTestSource))]
        public void ReversePolishFunction_ShouldThrowException_WhenBracketsInvalid(List<IToken> tokens)
        {
            ArgumentException exception = Assert.Throws<ArgumentException>(() =>
                calculator.TransformToReversePolish(tokens));
            Assert.AreEqual("Invalid brackets", exception.Message);
        }

        private static object[] invalidReversePolishTestSource =
        {
            new object[] {SimpleParse("")},
            new object[] {SimpleParse("2+")},
            new object[] {SimpleParse("222+")},
        };

        [Test, TestCaseSource(nameof(invalidReversePolishTestSource))]
        public void ReversePolishCalculation_ShouldThrowException(List<IToken> tokens)
        {
            ArgumentException exception = Assert.Throws<ArgumentException>(() =>
                calculator.CalculateReversePolishTokens(tokens));
            Assert.AreEqual("Invalid reverse polish notation tokens", exception.Message);
        }

        private static object[] reversePolishCalculationTestSource =
        {
            new object[] {SimpleParse("1"), 1},
            new object[] {SimpleParse("22+"), 4},
            new object[] {SimpleParse("63/"), 2},
            new object[] {SimpleParse("63-"), 3},
            new object[] {SimpleParse("22*2+"), 6},
        };

        [Test, TestCaseSource(nameof(reversePolishCalculationTestSource))]
        public void ReversePolishCalculation_ShouldCalculateTokens(List<IToken> tokens, float expectedResult)
        {
            float result = calculator.CalculateReversePolishTokens(tokens);
            Assert.AreEqual(expectedResult, result);
        }
    }
}
