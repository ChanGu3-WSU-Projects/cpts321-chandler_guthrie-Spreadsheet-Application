// <copyright file="TestEvaluate.cs" company="Chandler_Guthrie-WSU_ID:011801740">
// Copyright (c) Chandler_Guthrie-WSU_ID:011801740. All rights reserved.
// </copyright>

using System.Data;

namespace SpreadsheetEngine.ExpressionTreeDS.Test.Evaluate
{
    /// <summary>
    /// Evalute testing.
    /// </summary>
    [TestFixture]
    internal class TestEvaluate
    {
        /// <summary>
        /// normal case testing.
        /// </summary>
        /// <param name="expression"> expression to create tree. </param>
        /// <param name="num1Value"> value of the variable num1 in expression. </param>
        /// <param name="num2Value"> value of the variable num2 in expression. </param>
        /// <returns> the evaluted tree. </returns>
        [Test]
        [TestCase("5+num1+num2+5", 5, 5, ExpectedResult = 20)]
        [TestCase("1-num1-num2-1", 1, 1, ExpectedResult = -2)]
        [TestCase("5*num1*num2*5", 5, 5, ExpectedResult = 625)]
        [TestCase("5/num1/num2/5", 5, 5, ExpectedResult = 0.04)]
        public double? NormalCaseTest(string expression, double num1Value, double num2Value)
        {
            ExpressionTree expTreeException = new ExpressionTree(expression);
            expTreeException.SetVariable("num1", num1Value);
            expTreeException.SetVariable("num2", num2Value);

            return expTreeException.Evaluate();
        }

        /// <summary>
        /// edge case testing.
        /// </summary>
        /// <param name="expression"> expression to create tree. </param>
        /// <returns> the result of a evaluated expression tree. </returns>
        [Test]
        [TestCase("num1+1", ExpectedResult = 1)]
        [TestCase("1+num1", ExpectedResult = 1)]
        [TestCase("1/1/1/0", ExpectedResult = double.PositiveInfinity)]
        public double? EdgeCaseTest(string expression)
        {
            ExpressionTree expTreeException = new ExpressionTree(expression);
            foreach (string variableName in expTreeException.VariableNames)
            {
                expTreeException.SetVariable(variableName, 0);
            }

            return expTreeException.Evaluate();
        }

        /// <summary>
        /// exception case testing.
        /// </summary>
        /// <param name="expression"> expression to create tree. </param>
        /// <returns> exception type thrown inside try block. </returns>
        [Test]
        [TestCase("#", ExpectedResult = typeof(InvalidExpressionException))] // when the expression is invlaid cannot evaluate it.
        [TestCase("num1+1", ExpectedResult = typeof(InvalidOperationException))] // when variable does not contain a value.
        public Type? TestExceptionCase(string expression)
        {
            // throw exception after evaluate call
            try
            {
                ExpressionTree expTreeException = new ExpressionTree(expression);
                expTreeException.Evaluate();
                return null;
            }
            catch (Exception exception)
            {
                return exception.GetType();
            }
        }
    }
}
