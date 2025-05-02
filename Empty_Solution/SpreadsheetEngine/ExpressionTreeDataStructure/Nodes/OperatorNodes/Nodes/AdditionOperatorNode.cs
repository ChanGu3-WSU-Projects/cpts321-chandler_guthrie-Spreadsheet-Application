// <copyright file="AdditionOperatorNode.cs" company="Chandler_Guthrie-WSU_ID:011801740">
// Copyright (c) Chandler_Guthrie-WSU_ID:011801740. All rights reserved.
// </copyright>

namespace SpreadsheetEngine.ExpressionTreeDS
{
    /// <summary>
    /// The addition operator node for expression tree.
    /// </summary>
    internal class AdditionOperatorNode : OperatorNode
    {
        /// <summary>
        /// character representing addition operator.
        /// </summary>
        private static readonly char Op = '+';

        /// <summary>
        /// the precendence for the addition operator.
        /// </summary>
        private static readonly int Prec = 7;

        /// <summary>
        /// associativity of addition operator.
        /// </summary>
        private static readonly Associativity Assoc = Associativity.LeftAssociative;

        /// <summary>
        /// Gets static addition operator.
        /// </summary>
        public override char Operator
        {
            get => Op;
        }

        /// <summary>
        /// Gets static addition precedence.
        /// </summary>
        public override int Precedence
        {
            get => Prec;
        }

        /// <summary>
        /// Gets static addition associativity.
        /// </summary>
        public override Associativity Associativity
        {
            get => Assoc;
        }

        /// <summary>
        /// Adds left and right node.
        /// </summary>
        /// <returns> the addition of left and right node. </returns>
        public override double Evaluate() => this.Left!.Evaluate() + this.Right!.Evaluate();
    }
}
