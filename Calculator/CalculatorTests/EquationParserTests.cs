using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Calculator;
using NUnit.Framework;

namespace CalculatorTests
{
    [TestFixture]
    public class EquationParserTests
    {
        private EquationParser equationParser;

        [OneTimeSetUp]
        public void SetUp()
        {
            equationParser = new EquationParser();
        }

        [Test]
        public void Parser_ShouldReturnEmptyList_WhenEquationEmpty()
        {
            List<IToken> tokens = equationParser.Parse("");
            Assert.AreEqual(0, tokens.Count);
        }

        [TestCase("2", 2), TestCase("222", 222), TestCase("2.2", 2.2f), TestCase("2,2", 2.2f), TestCase(".2", 0.2f), TestCase("2.", 2)]
        public void Parser_ShouldParseOneOperand(string input, float expectedTokenValue)
        {
            List<IToken> tokens = equationParser.Parse(input);
            Assert.AreEqual(1, tokens.Count);
            Assert.IsInstanceOf<OperandToken>(tokens[0]);
            Assert.AreEqual(expectedTokenValue, tokens[0].GetFloatValue());
        }

        [TestCase("+"), TestCase("-"), TestCase("/"), TestCase("*"), TestCase("("), TestCase(")")]
        public void Parser_ShouldParseOneOperator(string input)
        {
            List<IToken> tokens = equationParser.Parse(input);
            Assert.AreEqual(1, tokens.Count);
            Assert.IsInstanceOf<OperatorToken>(tokens[0]);
            Assert.AreEqual(input, tokens[0].GetStringValue());
        }

        [TestCase("2.2.2", "Can't parse float with two dots"), TestCase("aa", "Unknown character: a")]
        public void Parser_ShouldThrowException(string input, string expectedErrorMessage)
        {
            ArgumentException exception = Assert.Throws<ArgumentException>(() => equationParser.Parse(input));
            Assert.AreEqual(expectedErrorMessage, exception.Message);
        }

        [TestCase("2+", 2), TestCase("+2", 2), TestCase("22+2.2", 3), TestCase("(2+2)-10", 7), TestCase(" 2 +   2 ", 3)]
        public void Parser_ShouldParseMultipleTokens(string input, int expectedTokenCount)
        {
            List<IToken> tokens = equationParser.Parse(input);
            input = Regex.Replace(input, @"\s+", string.Empty);
            string result = string.Join("", tokens.Select(token => token.GetStringValue()));
            Assert.AreEqual(expectedTokenCount, tokens.Count);
            Assert.AreEqual(input, result);
        }
    }
}
