// <copyright file="PowOperatorNode.cs" company="Chandler_Guthrie-WSU_ID:011801740">
// Copyright (c) Chandler_Guthrie-WSU_ID:011801740. All rights reserved.
// </copyright>

namespace SpreadsheetEngine.ExpressionTreeDS
{
    /// <summary>
    /// the power node for expression tree.
    /// </summary>
    internal class PowOperatorNode : OperatorNode
    {
        /// <summary>
        /// character representing power operator.
        /// </summary>
        private static readonly char Op = '^';

        /// <summary>
        /// the precendence for the power operator.
        /// </summary>
        private static readonly int precedence = 2;

        /// <summary>
        /// associativity of power operator.
        /// </summary>
        private static readonly Associativity associativity = Associativity.RightAssociative;

        /// <summary>
        /// get static power operator
        /// </summary>
        public override char Operator
        {
            get => Op;
        }

        /// <summary>
        /// get static power precedence.
        /// </summary>
        public override int Precedence
        {
            get => precedence;
        }

        /// <summary>
        /// get static power associativity
        /// </summary>
        public override Associativity Associativity
        {
            get => associativity;
        }

        /// <summary>
        /// left raised to the power of the right
        /// </summary>
        /// <returns> the subtraction of left and right node. </returns>
        public override double Evaluate() => Math.Pow(this.Left!.Evaluate(), this.Right!.Evaluate());
    }
}
