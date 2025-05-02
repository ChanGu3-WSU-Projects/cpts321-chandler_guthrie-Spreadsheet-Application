// <copyright file="MultiplicationOperatorNode.cs" company="Chandler_Guthrie-WSU_ID:011801740">
// Copyright (c) Chandler_Guthrie-WSU_ID:011801740. All rights reserved.
// </copyright>

namespace SpreadsheetEngine.ExpressionTreeDS
{
    /// <summary>
    /// The multiplication operator node for expression tree.
    /// </summary>
    internal class MultiplicationOperatorNode : OperatorNode
    {
        /// <summary>
        /// character representing multiplication operator.
        /// </summary>
        private static readonly char Op = '*';

        /// <summary>
        /// the precendence for the multiplication operator.
        /// </summary>
        private static readonly int Prec = 6;

        /// <summary>
        /// associativity of multiplication operator.
        /// </summary>
        private static readonly Associativity Assoc = Associativity.LeftAssociative;

        /// <summary>
        /// Gets static multiplication operator.
        /// </summary>
        public override char Operator
        {
            get => Op;
        }

        /// <summary>
        /// Gets static multiplication precedence.
        /// </summary>
        public override int Precedence
        {
            get => Prec;
        }

        /// <summary>
        /// Gets static multiplication associativity.
        /// </summary>
        public override Associativity Associativity
        {
            get => Assoc;
        }

        /// <summary>
        /// multiply left and right node.
        /// </summary>
        /// <returns> the multiplication of left and right node. </returns>
        public override double Evaluate() => this.Left!.Evaluate() * this.Right!.Evaluate();
    }
}
