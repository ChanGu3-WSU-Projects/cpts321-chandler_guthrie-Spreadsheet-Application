// <copyright file="DivisionOperatorNode.cs" company="Chandler_Guthrie-WSU_ID:011801740">
// Copyright (c) Chandler_Guthrie-WSU_ID:011801740. All rights reserved.
// </copyright>

namespace SpreadsheetEngine.ExpressionTreeDS
{
    /// <summary>
    /// The division operator node for expression tree.
    /// </summary>
    internal class DivisionOperatorNode : OperatorNode
    {
        /// <summary>
        /// character representing division operator.
        /// </summary>
        private static readonly char Op = '/';

        /// <summary>
        /// the precendence for the division operator.
        /// </summary>
        private static readonly int Prec = 6;

        /// <summary>
        /// associativity of division operator.
        /// </summary>
        private static readonly Associativity Assoc = Associativity.LeftAssociative;

        /// <summary>
        /// Gets static division operator.
        /// </summary>
        public override char Operator
        {
            get => Op;
        }

        /// <summary>
        /// Gets static division precedence.
        /// </summary>
        public override int Precedence
        {
            get => Prec;
        }

        /// <summary>
        /// Gets static division associativity.
        /// </summary>
        public override Associativity Associativity
        {
            get => Assoc;
        }

        /// <summary>
        /// divide left(numerator) of right(denominator) node.
        /// </summary>
        /// <returns> the division of left of right node. </returns>
        public override double Evaluate() => this.Left!.Evaluate() / this.Right!.Evaluate();
    }
}
