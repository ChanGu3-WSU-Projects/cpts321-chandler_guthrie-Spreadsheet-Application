// <copyright file="OperatorNodeFactory.cs" company="Chandler_Guthrie-WSU_ID:011801740">
// Copyright (c) Chandler_Guthrie-WSU_ID:011801740. All rights reserved.
// </copyright>

namespace SpreadsheetEngine.ExpressionTreeDS
{
    /// <summary>
    /// Creates instances of all types of operator nodes.
    /// </summary>
    internal static class OperatorNodeFactory
    {
        /// <summary>
        /// using the operator character creates an operator node instance and returns it.
        /// </summary>
        /// <param name="op"> operator as a char. </param>
        /// <returns> instance of a new operator with the char operator. </returns>
        /// <exception cref="NotSupportedException"> when operator accessed is not returned. (should never really reach here). </exception>
        public static OperatorNode CreateOperatorNode(char op)
        {
            object? node = Activator.CreateInstance(OperatorSignToType.Data[op]);
            if (node is not null)
            {
                return (node as OperatorNode)!;
            }

            throw new NotSupportedException($"Operator \"{op}\" not supported.");
        }

        /// <summary>
        /// checks if the operator exists in the dictionary.
        /// </summary>
        /// <param name="op"> operator as a char. </param>
        /// <returns> if the operator exists. </returns>
        public static bool IsOperatorNode(char op)
        {
            Type? temp;
            return OperatorSignToType.Data.TryGetValue(op, out temp);
        }
    }
}
