// <copyright file="TestEvaluate.cs" company="Chandler_Guthrie-WSU_ID:011801740">
// Copyright (c) Chandler_Guthrie-WSU_ID:011801740. All rights reserved.
// </copyright>

namespace SpreadsheetEngine.ExpressionTreeDS.Test.AllNodeETEvaluate
{
    /// <summary>
    /// testing evaluate method in every ANode.
    /// </summary>
    [TestFixture]
    internal class TestEvaluate
    {
        /// <summary>
        /// normal case testing.
        /// </summary>
        /// <param name="constant"> constant value for a constant value node. </param>
        /// <param name="variableName"> name of a variable node. </param>
        /// <param name="variableValue"> value of variable node. </param>
        /// <param name="aOperator"> operator for a operator node. </param>
        /// <returns> true when all values evaluate properly. </returns>
        [Test]
        [TestCase(5, "A1", 5, '+', ExpectedResult = true)] // addition operator evaluate
        [TestCase(5, "A1", 5, '-', ExpectedResult = true)] // subtraction operator evaluate
        [TestCase(5, "A1", 5, '*', ExpectedResult = true)] // multiplication operator evaluate
        [TestCase(5, "A1", 5, '/', ExpectedResult = true)] // division operator evaluate
        public bool? TestNormalCase(double constant, string variableName, double variableValue, char aOperator)
        {
            Node constantNode = new ConstantNode(constant);
            Dictionary<string, double?> variables = new Dictionary<string, double?>();
            variables.Add(variableName, variableValue);
            Node variableNode = new VariableNode(variableName, ref variables);
            OperatorNode? operatorNode = Activator.CreateInstance(OperatorSignToType.Data[aOperator]) as OperatorNode;

            operatorNode!.Left = variableNode;
            operatorNode!.Right = constantNode;

            return constantNode.Evaluate() == constant &&
                variableNode.Evaluate() == variableValue &&
                aOperator == '+' ? operatorNode.Evaluate() == (variableValue + constant) :
                aOperator == '-' ? operatorNode.Evaluate() == (variableValue - constant) :
                aOperator == '*' ? operatorNode.Evaluate() == (variableValue * constant) :
                aOperator == '/' ? operatorNode.Evaluate() == (variableValue / constant) : false;
        }

        /// <summary>
        /// edge case testing.
        /// </summary>
        /// <returns> the result of a evaluation of a node. </returns>
        [Test]
        [TestCase(ExpectedResult = double.PositiveInfinity)] // divide by zero becomes infinity
        public double? TestEdgeCase()
        {
            Node leftConstantNode = new ConstantNode(5);
            Node rightConstantNode = new ConstantNode(0);
            OperatorNode? operatorNode = Activator.CreateInstance(OperatorSignToType.Data['/']) as OperatorNode;

            operatorNode!.Left = leftConstantNode;
            operatorNode!.Right = rightConstantNode;

            return operatorNode.Evaluate();
        }

        /// <summary>
        /// exception case testing.
        /// </summary>
        /// <param name="constant"> constant value for a constant value node. </param>
        /// <param name="variableName"> name of a variable node. </param>
        /// <param name="variableValue"> value of variable node. </param>
        /// <param name="aOperator"> operator for a operator node. </param>
        /// <param name="someException"> the exception under testing. </param>
        /// <returns> exception type thrown inside try block. </returns>
        [Test]
        [TestCase(5, "A1", 5, '+', typeof(NullReferenceException), ExpectedResult = typeof(NullReferenceException))] // operator node has no children
        [TestCase(0, "A1", 5, '+', typeof(KeyNotFoundException), ExpectedResult = typeof(KeyNotFoundException))] // variable node without variable in dictionary
        [TestCase(0, "A1", null, '+', typeof(InvalidOperationException), ExpectedResult = typeof(InvalidOperationException))] // When A Variable does not contatin a value (is null in dictionary)
        public Type? TestExceptionCase(double constant, string variableName, double? variableValue, char aOperator, Type someException)
        {
            try
            {
                // setup each node type with current parameters
                ConstantNode constantNode = new ConstantNode(constant);
                Dictionary<string, double?> variables = new Dictionary<string, double?>();

                // Dont add when doing exception for variables
                if (someException != typeof(KeyNotFoundException))
                {
                    variables.Add(variableName, variableValue);
                }

                VariableNode variableNode = new VariableNode(variableName, ref variables);
                OperatorNode? operatorNode = Activator.CreateInstance(OperatorSignToType.Data[aOperator]) as OperatorNode;

                if (someException != typeof(NullReferenceException))
                {
                    operatorNode!.Left = variableNode;
                    operatorNode!.Right = constantNode;
                }

                // Testing of each evaluation
                constantNode.Evaluate();
                variableNode.Evaluate();
                operatorNode!.Evaluate();

                // method call here
                return null;
            }
            catch (Exception exception)
            {
                return exception.GetType();
            }
        }
    }
}
