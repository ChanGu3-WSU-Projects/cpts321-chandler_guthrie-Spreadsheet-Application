// <copyright file="TestInfixToPostfix.cs" company="Chandler_Guthrie-WSU_ID:011801740">
// Copyright (c) Chandler_Guthrie-WSU_ID:011801740. All rights reserved.
// </copyright>

using System.Data;
using System.Reflection;
using SpreadsheetEngineTests;

namespace SpreadsheetEngine.ExpressionTreeDS
{
    /// <summary>
    /// testing InfixToPostfix method.
    /// </summary>
    [TestFixture]
    internal class TestInfixToPostfix
    {
        private MethodInfo? methodInfoInfixToPostfix;

        /// <summary>
        /// setup for the testcases.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            this.methodInfoInfixToPostfix = ReflectionMethods.GetMethod("InfixToPostfix", typeof(ExpressionTree), BindingFlags.Static);
        }

        /// <summary>
        /// normal case testing for InfixToPostfix.
        /// </summary>
        /// <param name="infixExpression"> infix expression. </param>
        /// <returns> postfix expression of infix. </returns>
        [Test]
        [TestCase("2+2-2", ExpectedResult = "2 2 + 2 -")] // Addition and subtraction with same precedence (LA)
        [TestCase("2*2/2", ExpectedResult = "2 2 * 2 /")] // Multiplication and division with saem precedence (LA)
        [TestCase("2+2*2-2/2", ExpectedResult = "2 2 2 * + 2 2 / -")] // different precedence addition, multiplication, subtraction, division
        [TestCase("num1+5", ExpectedResult = "num1 5 +")] // different precedence addition, multiplication, subtraction, division
        [TestCase("num1+5*3", ExpectedResult = "num1 5 3 * +")] // without parenthese
        [TestCase("(num1+5)*3", ExpectedResult = "num1 5 + 3 *")] // with parenthese
        [TestCase("((num1+5)*3*(1+1))", ExpectedResult = "num1 5 + 3 * 1 1 + *")] // more parenthese
        [TestCase("(4*(2+3)-(6/2))*3+((5-1)*(7+2)/3)-(8*2-(1+6))", ExpectedResult = "4 2 3 + * 6 2 / - 3 * 5 1 - 7 2 + * 3 / + 8 2 * 1 6 + - -")] // more complex (ChatGPT Generated)
        [TestCase("2^3", ExpectedResult = "2 3 ^")] // power operator
        [TestCase("2^3^4", ExpectedResult = "2 3 4 ^ ^")] // double power operator
        public string? TestNormalCaseInfixToPostfix(string infixExpression)
        {
            List<string>? postExpressionList = this.methodInfoInfixToPostfix!.Invoke(null, new object?[] { infixExpression }) as List<string>;

            string postExpressionString = string.Empty;

            for (int index = 0; index < postExpressionList!.Count; index++)
            {
                if (index == postExpressionList!.Count - 1)
                {
                    postExpressionString += postExpressionList[index];
                }
                else
                {
                    postExpressionString += postExpressionList[index] + " ";
                }
            }

            return postExpressionString;
        }

        /// <summary>
        /// edge case testing for InfixToPostfix.
        /// </summary>
        /// <param name="infixExpression"> infix expression. </param>
        /// <returns> postfix expression of infix. </returns>
        [Test]
        [TestCase("1               *             (5+5)      *        1", ExpectedResult = "1 5 5 + * 1 *")] // leading whitespace
        [TestCase("5", ExpectedResult = "5")] // constant value
        [TestCase("A", ExpectedResult = "A")] // variable value
        [TestCase("", ExpectedResult = "")] // empty expression
        [TestCase("    ", ExpectedResult = "")] // whitespace expression
        [TestCase("(A)", ExpectedResult = "A")] // more parenthese
        [TestCase("(((((((5+5)*6))))))", ExpectedResult = "5 5 + 6 *")] // more parenthese
        public string? TestEdgeCaseInfixToPostfix(string infixExpression)
        {
            List<string>? postExpressionList = this.methodInfoInfixToPostfix!.Invoke(null, new object?[] { infixExpression }) as List<string>;

            string postExpressionString = string.Empty;

            for (int index = 0; index < postExpressionList!.Count; index++)
            {
                if (index == postExpressionList!.Count - 1)
                {
                    postExpressionString += postExpressionList[index];
                }
                else
                {
                    postExpressionString += postExpressionList[index] + " ";
                }
            }

            return postExpressionString;
        }

        /// <summary>
        /// exception case testing for InfixToPostfix
        ///     (Also means that all exceptions are caught before postfix is sent to PostfixToExpressionTree).
        /// </summary>
        /// <param name="infixExpression"> infix expression. </param>
        /// <returns> exception type thrown. </returns>
        [Test]
        [TestCase("1++1", ExpectedResult = typeof(InvalidExpressionException))] // operator after operator
        [TestCase("(1+)", ExpectedResult = typeof(InvalidExpressionException))] // missing operand on right
        [TestCase("+1", ExpectedResult = typeof(InvalidExpressionException))] // missing operand on left
        [TestCase("1+5A", ExpectedResult = typeof(InvalidExpressionException))] // variable name starting with number
        [TestCase("(1+A))", ExpectedResult = typeof(InvalidExpressionException))] // extra parenthese at end
        [TestCase("((1+A)", ExpectedResult = typeof(InvalidExpressionException))] // extra parenthese at begining
        [TestCase("1#1", ExpectedResult = typeof(InvalidExpressionException))] // extra parenthese at begining
        public Type? TestExceptionCaseInfixToPostfix(string infixExpression)
        {
            try
            {
                this.methodInfoInfixToPostfix!.Invoke(null, new object?[] { infixExpression });
                return null;
            }
            catch (Exception exMsg)
            {
                return exMsg.GetBaseException().GetType();
            }
        }
    }
}
