// <copyright file="TestInstantiate.cs" company="Chandler_Guthrie-WSU_ID:011801740">
// Copyright (c) Chandler_Guthrie-WSU_ID:011801740. All rights reserved.
// </copyright>

using System.Reflection;
using SpreadsheetEngineTests;

namespace SpreadsheetEngine.ExpressionTreeDS.Test.TreeConstructor
{
    /// <summary>
    /// expression tree constructor testing.
    /// </summary>
    [TestFixture]
    internal class TestInstantiate
    {
        private MethodInfo? methodInfoInstantiate;

        /// <summary>
        /// setup for the tests.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.methodInfoInstantiate = ReflectionMethods.GetMethod("Instantiate", typeof(ExpressionTree), BindingFlags.Static);
        }

        /// <summary>
        /// normal case testing.
        /// </summary>
        /// <returns> true when the nodes contain the correct data in the correct places. </returns>
        [Test]
        [TestCase(ExpectedResult = true)]
        public bool? TestNormalCase()
        {
            Dictionary<string, double?> variables = new Dictionary<string, double?>();

            OperatorNode? theRoot = (this.methodInfoInstantiate!.Invoke(null, new object?[] { "num1+5+num2+3+1", variables }) as OperatorNode)!;
            if (theRoot is null)
            {
                return null;
            }

            // Verify each node in tree is correct and has the correct data in children
            OperatorNode? op2 = theRoot.Left as OperatorNode;
            OperatorNode? op3 = op2?.Left as OperatorNode;
            OperatorNode? op4 = op3?.Left as OperatorNode;
            return
            theRoot.Right?.Evaluate() == 1 // constant noode verified
            && op2?.Right?.Evaluate() == 3 // constant node verified
            && op3?.Right?.Evaluate() == 0 // 0 verifies that this is variable node num2
            && op4?.Left?.Evaluate() == 0 // 0 verifies that this is variable num1
            && op4?.Right?.Evaluate() == 5; // constant node verified
        }

        /// <summary>
        /// edge case testing.
        /// </summary>
        /// <param name="key"> a possible key in dictionary. </param>
        /// <param name="expression"> expression used to create tree. </param>
        /// <param name="variableAmount"> the amount of variables in expression. </param>
        /// <returns> true if key exists in dictionary. </returns>
        [Test]
        [TestCase("num1", "num1+num2+num1", 2, ExpectedResult = true)]
        [TestCase("num2", "num1+num2+5", 2, ExpectedResult = true)]
        [TestCase("notakey", "num1+5", 1, ExpectedResult = false)]
        [TestCase("notakey", "5+5", 0, ExpectedResult = false)]
        public bool? TestEdgeCase(string key, string expression, int variableAmount)
        {
            Dictionary<string, double?> variables = new Dictionary<string, double?>();

            OperatorNode? theRoot = (this.methodInfoInstantiate!.Invoke(null, new object?[] { expression, variables }) as OperatorNode)!;

            if (variables is not null)
            {
                if (variables?.Count == variableAmount)
                {
                    return variables?.ContainsKey(key);
                }
            }

            return null;
        }

        /// <summary>
        /// exception case testing.
        /// </summary>
        /// <param name="expression"> expression under testing. </param>
        /// <returns> exception type thrown inside try block. </returns>
        [Test]
        [TestCase("5A+5", ExpectedResult = typeof(FormatException))]
        [TestCase("5+5A+5", ExpectedResult = typeof(FormatException))]
        [TestCase("5+5-5", ExpectedResult = typeof(FormatException))]
        [TestCase("5+5+5A", ExpectedResult = typeof(FormatException))]
        [TestCase("5", ExpectedResult = typeof(FormatException))]
        [TestCase("num1", ExpectedResult = typeof(FormatException))]
        [TestCase("", ExpectedResult = typeof(FormatException))]
        public Type? TestExceptionCase(string expression)
        {
            // after creating instance once exception is thrown make sure specific exception for input is being thrown.
            try
            {
                Dictionary<string, double?> variables = new Dictionary<string, double?>();

                OperatorNode? theRoot = (this.methodInfoInstantiate!.Invoke(null, new object?[] { expression, variables }) as OperatorNode)!;
                return null;
            }
            catch (Exception exception)
            {
                return exception.GetBaseException().GetType();
            }
        }
    }
}
