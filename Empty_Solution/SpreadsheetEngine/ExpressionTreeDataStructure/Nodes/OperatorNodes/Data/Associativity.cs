// <copyright file="Associativity.cs" company="Chandler_Guthrie-WSU_ID:011801740">
// Copyright (c) Chandler_Guthrie-WSU_ID:011801740. All rights reserved.
// </copyright>

namespace SpreadsheetEngine.ExpressionTreeDS
{
    /// <summary>
    /// The associativity of int.
    /// </summary>
    internal enum Associativity : int
    {
        /// <summary>
        /// respresents left associative.
        ///     evaluation in same precidence left to right.
        /// </summary>
        LeftAssociative = -1,

        /// <summary>
        /// respresents non associative.
        ///     evaluation in same precidence not allowed
        /// </summary>
        NonAssociative = 0,

        /// <summary>
        /// respresents right associative.
        ///     evaluation in same precidence right to left.
        /// </summary>
        RightAssociative = 1,
    }
}
