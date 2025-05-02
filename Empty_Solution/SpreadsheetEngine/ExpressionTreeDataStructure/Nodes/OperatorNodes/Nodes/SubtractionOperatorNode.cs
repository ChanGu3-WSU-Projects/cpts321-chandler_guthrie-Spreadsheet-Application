// <copyright file="SubtractionOperatorNode.cs" company="Chandler_Guthrie-WSU_ID:011801740">
// Copyright (c) Chandler_Guthrie-WSU_ID:011801740. All rights reserved.
// </copyright>

namespace SpreadsheetEngine.ExpressionTreeDS
{
    /// <summary>
    /// The subtraction operator node for expression tree.
    /// </summary>
    internal class SubtractionOperatorNode : OperatorNode
    {
        /// <summary>
        /// character representing subtraction operator.
        /// </summary>
        private static readonly char Op = '-';

        /// <summary>
        /// the precendence for the subtraction operator.
        /// </summary>
        private static readonly int Prec = 7;

        /// <summary>
        /// associativity of subtraction operator.
        /// </summary>
        private static readonly Associativity Assoc = Associativity.LeftAssociative;

        /// <summary>
        /// Gets static subtraction operator.
        /// </summary>
        public override char Operator
        {
            get => Op;
        }

        /// <summary>
        /// Gets static subtraction precedence.
        /// </summary>
        public override int Precedence
        {
            get => Prec;
        }

        /// <summary>
        /// Gets static subtraction associativity.
        /// </summary>
        public override Associativity Associativity
        {
            get => Assoc;
        }

        /// <summary>
        /// subtract left and right node.
        /// </summary>
        /// <returns> the subtraction of left and right node. </returns>
        public override double Evaluate() => this.Left!.Evaluate() - this.Right!.Evaluate();
    }
}
