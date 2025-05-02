// <copyright file="TestPostfixToExpressionTree.cs" company="Chandler_Guthrie-WSU_ID:011801740">
// Copyright (c) Chandler_Guthrie-WSU_ID:011801740. All rights reserved.
// </copyright>

using System.Data;
using System.Reflection;
using SpreadsheetEngineTests;

namespace SpreadsheetEngine.ExpressionTreeDS
{
    /// <summary>
    /// testing two methods that are used with each other to create a expression tree from an infix expression.
    /// </summary>
    [TestFixture]
    internal class TestPostfixToExpressionTree
    {
        private MethodInfo? methodInfoPostfixToExpressionTree;

        /// <summary>
        /// setup for the testcases.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            this.methodInfoPostfixToExpressionTree = ReflectionMethods.GetMethod("PostfixToExpressionTree", typeof(ExpressionTree), BindingFlags.Static);
        }

        /// <summary>
        /// normal testing for method PostfixToExpressionTree.
        /// </summary>
        /// <param name="postfixExpression">  postfix expression for creating data structure. </param>
        /// <returns> evaluation of expression tree. </returns>
        [Test]
        [TestCase("2 2 +", ExpectedResult = 4)] // Normal Addition
        [TestCase("2 2 -", ExpectedResult = 0)] // Normal subtraction
        [TestCase("2 2 *", ExpectedResult = 4)] // Normal multiplication
        [TestCase("2 2 /", ExpectedResult = 1)] // Normal division
        [TestCase("2 2 + 2 *", ExpectedResult = 8)] // parenthesis made as postfix
        [TestCase("2 3 ^", ExpectedResult = 8)] // power operator
        [TestCase("2 3 2 ^ ^", ExpectedResult = 512)] // double power operator
        public double? TestNormalCase(string postfixExpression)
        {
            List<string> postfixList = postfixExpression.Split(' ').ToList();

            Dictionary<string, double?> variables = new Dictionary<string, double?>();
            Node? rootNode = this.methodInfoPostfixToExpressionTree!.Invoke(null, new object?[] { postfixList, variables }) as Node;
            if (rootNode != null)
            {
                return rootNode.Evaluate();
            }

            return double.NaN;
        }

        /// <summary>
        /// edge case testing for method PostfixToExpressionTree.
        /// </summary>
        /// <param name="postfixExpression">  postfix expression for creating data structure. </param>
        /// <returns> the evaluation of the expression. </returns>
        [Test]
        [TestCase("2 2 + 2 * 2 2 / -", ExpectedResult = 7)] // complex postfix
        [TestCase("A A +", ExpectedResult = 0)] // same variable usage
        [TestCase("5", ExpectedResult = 5)] // constant value
        [TestCase("A", ExpectedResult = 0)] // variable value
        [TestCase("", ExpectedResult = double.NaN)] // empty expression
        public double? TestEdgeCase(string postfixExpression)
        {
            List<string> postfixList = postfixExpression.Split(' ').ToList();

            Dictionary<string, double?> variables = new Dictionary<string, double?>();
            Node? rootNode = this.methodInfoPostfixToExpressionTree!.Invoke(null, new object?[] { postfixList, variables }) as Node;

            foreach (string variable in variables.Keys)
            {
                variables[variable] = 0;
            }

            if (rootNode != null)
            {
                return rootNode.Evaluate();
            }

            return double.NaN;
        }

        /// <summary>
        /// exception case testing for InfixToPostfix
        ///     (Also means that all exceptions are caught before postfix is sent to PostfixToExpressionTree).
        /// </summary>
        /// <param name="postfixExpression"> postfix expression. </param>
        /// <returns> exception type thrown. </returns>
        [Test]
        [TestCase("1 1 + 1 #", ExpectedResult = typeof(InvalidExpressionException))] // operator is not supported meaning not in dictionary for factory
        public Type? TestExceptionCase(string postfixExpression)
        {
            List<string> postfixList = postfixExpression.Split(' ').ToList();

            Dictionary<string, double?> variables = new Dictionary<string, double?>();

            try
            {
                this.methodInfoPostfixToExpressionTree!.Invoke(null, new object?[] { postfixList, variables });
                return null;
            }
            catch (Exception exMsg)
            {
                return exMsg.GetBaseException().GetType();
            }
        }
    }
}