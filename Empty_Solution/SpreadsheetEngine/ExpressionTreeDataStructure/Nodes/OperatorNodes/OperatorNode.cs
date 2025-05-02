// <copyright file="OperatorNode.cs" company="Chandler_Guthrie-WSU_ID:011801740">
// Copyright (c) Chandler_Guthrie-WSU_ID:011801740. All rights reserved.
// </copyright>

namespace SpreadsheetEngine.ExpressionTreeDS
{
    /// <summary>
    /// Node with an operator.
    /// </summary>
    internal abstract class OperatorNode : Node
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OperatorNode"/> class.
        ///     assigns node right and left to null.
        /// </summary>
        public OperatorNode()
        {
            this.Left = null;
            this.Right = null;
        }

        /// <summary>
        /// Gets or sets node on the left of the operand.
        /// </summary>
        public Node? Left
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets node on the right of the operand.
        /// </summary>
        public Node? Right
        {
            get;
            set;
        }

        /// <summary>
        /// Gets static operator.
        /// </summary>
        public abstract char Operator
        {
            get;
        }

        /// <summary>
        /// Gets static precedence.
        /// </summary>
        public abstract int Precedence
        {
            get;
        }

        /// <summary>
        /// Gets static associativity.
        /// </summary>
        public abstract Associativity Associativity
        {
            get;
        }
    }
}
