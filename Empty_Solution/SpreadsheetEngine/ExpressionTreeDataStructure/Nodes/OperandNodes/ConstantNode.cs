// <copyright file="ConstantNode.cs" company="Chandler_Guthrie-WSU_ID:011801740">
// Copyright (c) Chandler_Guthrie-WSU_ID:011801740. All rights reserved.
// </copyright>

namespace SpreadsheetEngine.ExpressionTreeDS
{
    /// <summary>
    /// constant operand node in expression tree.
    /// </summary>
    internal class ConstantNode : Node
    {
        /// <summary>
        /// constant value.
        /// </summary>
        public readonly double Value;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstantNode"/> class.
        ///     assigns node with specifc constant value.
        /// </summary>
        /// <param name="constantValue"> constant value to assign to node. </param>
        public ConstantNode(double constantValue)
        {
            this.Value = constantValue;
        }

        /// <summary>
        /// Evaluates constant node double.
        /// </summary>
        /// <returns> constant value of node. </returns>
        public override double Evaluate() => this.Value;
    }
}
