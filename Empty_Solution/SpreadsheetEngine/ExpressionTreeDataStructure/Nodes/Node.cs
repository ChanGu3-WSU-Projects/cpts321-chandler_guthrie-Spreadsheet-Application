// <copyright file="Node.cs" company="Chandler_Guthrie-WSU_ID:011801740">
// Copyright (c) Chandler_Guthrie-WSU_ID:011801740. All rights reserved.
// </copyright>

namespace SpreadsheetEngine.ExpressionTreeDS
{
    /// <summary>
    /// abstract Node for the expression tree.
    /// </summary>
    internal abstract class Node
    {
        /// <summary>
        /// Evaluates current node.
        /// </summary>
        /// <returns> value of node. </returns>
        public abstract double Evaluate();
    }
}
