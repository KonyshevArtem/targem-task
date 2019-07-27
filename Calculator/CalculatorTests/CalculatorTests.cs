using System;
using System.Collections.Generic;
using System.Linq;
using Calculator;
using FakeItEasy;
using NUnit.Framework;

namespace CalculatorTests
{
    [TestFixture]
    public class CalculatorTests
    {
        private StringCalculator calculator;
        private IParser<List<IToken>> parser;

        [SetUp]
        public void SetUp()
        {
            parser = A.Fake<IParser<List<IToken>>>();
            calculator = new StringCalculator(parser);
        }

        [Test]
        public void Calculator_ShouldThrowException_WhenStringEmpty()
        {
            A.CallTo(() => parser.Parse("")).Returns(new List<IToken>());
            ArgumentException exception = Assert.Throws<ArgumentException>(() => calculator.Calculate(""));
            Assert.AreEqual("Input equation is empty", exception.Message);
        }

        private static object[] reversePolishTestSource =
        {
            new object[] { new List<IToken>(), ""},
            new object[] { new List<IToken> {new OperandToken("1")}, "1"},
            new object[] { new List<IToken> {new OperandToken("1"), new OperandToken("2")}, "12"},
            new object[] { new List<IToken> {new OperatorToken("("), new OperatorToken(")")}, ""},
            new object[] { new List<IToken> {new OperatorToken("("), new OperandToken("2") , new OperatorToken(")") }, "2"},
            new object[] { new List<IToken> {new OperatorToken("+")}, "+"},
            new object[] { new List<IToken> {new OperandToken("2"), new OperatorToken("+"), new OperandToken("2") }, "22+"},
            new object[] { new List<IToken> {new OperandToken("2"), new OperatorToken("+"), new OperandToken("2"),
                new OperatorToken("*"), new OperandToken("2")}, "222*+"},
            new object[] { new List<IToken> {new OperandToken("2"), new OperatorToken("*"), new OperandToken("2"),
                new OperatorToken("+"), new OperandToken("2")}, "22*2+"},
            new object[] { new List<IToken> { new OperatorToken("("), new OperandToken("2"), new OperatorToken("+"),
                new OperandToken("2"), new OperatorToken(")"), new OperatorToken("*"), new OperandToken("2")}, "22+2*"},
            new object[] { new List<IToken> {new OperandToken("2"), new OperatorToken("*"), new OperatorToken("("),
                new OperandToken("2"), new OperatorToken("+"), new OperandToken("2"), new OperatorToken(")")}, "222+*"},
        };

        [Test, TestCaseSource(nameof(reversePolishTestSource))]
        public void ReversePolishFunction_ShouldTransformToReversePolishNotation(List<IToken> tokens, string expectedReversePolishEquation)
        {
            List<IToken> reversePolishTokens = calculator.TransformToReversePolish(tokens);
            string reversePolishEquation = string.Join("", reversePolishTokens.Select(token => token.GetStringValue()));
            Assert.AreEqual(expectedReversePolishEquation, reversePolishEquation);
        }

        private static object[] invalidBracketsTestSource =
        {
            new object[] {new List<IToken> {new OperatorToken("(")}},
            new object[] {new List<IToken> {new OperatorToken(")")}},
            new object[] {new List<IToken> {new OperandToken("2"), new OperatorToken(")")}}
        };

        [Test, TestCaseSource(nameof(invalidBracketsTestSource))]
        public void ReversePolishFunction_ShouldThrowException_WhenBracketsInvalid(List<IToken> tokens)
        {
            ArgumentException exception = Assert.Throws<ArgumentException>(() =>
                calculator.TransformToReversePolish(new List<IToken> { new OperatorToken(")") }));
            Assert.AreEqual("Invalid brackets", exception.Message);
        }
    }
}
