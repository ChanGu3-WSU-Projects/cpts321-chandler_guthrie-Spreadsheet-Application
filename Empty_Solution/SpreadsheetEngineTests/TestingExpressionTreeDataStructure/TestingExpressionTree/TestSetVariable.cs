// <copyright file="TestSetVariable.cs" company="Chandler_Guthrie-WSU_ID:011801740">
// Copyright (c) Chandler_Guthrie-WSU_ID:011801740. All rights reserved.
// </copyright>

using System.Reflection;
using SpreadsheetEngineTests;

namespace SpreadsheetEngine.ExpressionTreeDS.Test.SetVariable
{
    /// <summary>
    /// Set Variable testing.
    /// </summary>
    [TestFixture]
    internal class TestSetVariable
    {
        /// <summary>
        /// expression tree under testing.
        /// </summary>
        private ExpressionTree expressionTreeTest;

        /// <summary>
        /// double variable field.
        /// </summary>
        private FieldInfo? doubleVariablesField;

        /// <summary>
        /// setup for the tests.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.doubleVariablesField = ReflectionMethods.GetField("variables", typeof(ExpressionTree), BindingFlags.Instance);
            this.expressionTreeTest = new ExpressionTree("num1+5+num2+3+5");
        }

        /// <summary>
        /// normal case testing.
        /// </summary>
        /// <param name="key"> key in dictionary. </param>
        /// <param name="setValue"> value to set to key in dictionary. </param>
        /// <returns> the result of the set value from dictionary. </returns>
        [Test]
        [TestCase("num1", 5, ExpectedResult = 5)]
        [TestCase("num2", 3, ExpectedResult = 3)]
        public double? NormalCaseTest(string key, double setValue)
        {
            this.expressionTreeTest.SetVariable(key, setValue);

            if (this.doubleVariablesField?.GetValue(this.expressionTreeTest) is not null)
            {
                Dictionary<string, double?>? theDoubleVariable = this.doubleVariablesField.GetValue(this.expressionTreeTest) as Dictionary<string, double?>;
                if (theDoubleVariable is not null)
                {
                    double? value;
                    if (theDoubleVariable.TryGetValue(key, out value))
                    {
                        return value;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// edge case testing.
        /// </summary>
        /// <param name="key"> key in dictionary. </param>
        /// <param name="setValue"> value to set to key in dictionary. </param>
        /// <param name="expression"> expression to create tree. </param>
        /// <returns> returns value of the dictionary item. </returns>
        [Test]
        [TestCase("num1", (double)int.MaxValue, "5+5+num1", ExpectedResult = (double)int.MaxValue)]
        public double? EdgeCaseTest(string key, double setValue, string expression)
        {
            ExpressionTree expressionTreeEdgeTest = new ExpressionTree(expression);

            expressionTreeEdgeTest.SetVariable(key, setValue);

            if (this.doubleVariablesField?.GetValue(expressionTreeEdgeTest) is not null)
            {
                Dictionary<string, double?>? theDoubleVariable = this.doubleVariablesField.GetValue(expressionTreeEdgeTest) as Dictionary<string, double?>;
                if (theDoubleVariable is not null)
                {
                    double? value;
                    if (theDoubleVariable.TryGetValue(key, out value))
                    {
                        return value;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// exception case testing.
        /// </summary>
        /// <returns> exception type thrown inside try block. </returns>
        [Test]
        [TestCase(ExpectedResult = typeof(KeyNotFoundException))]
        public Type? TestExceptionCase()
        {
            try
            {
                this.expressionTreeTest.SetVariable("notakey", 0);
                return null;
            }
            catch (Exception exception)
            {
                return exception.GetType();
            }
        }
    }
}
