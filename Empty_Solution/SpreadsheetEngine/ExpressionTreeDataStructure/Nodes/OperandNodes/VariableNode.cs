// <copyright file="VariableNode.cs" company="Chandler_Guthrie-WSU_ID:011801740">
// Copyright (c) Chandler_Guthrie-WSU_ID:011801740. All rights reserved.
// </copyright>

namespace SpreadsheetEngine.ExpressionTreeDS
{
    /// <summary>
    /// variable operand node in expression tree.
    /// </summary>
    internal class VariableNode : Node
    {
        /// <summary>
        /// variable name.
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// expression dictionary reference.
        /// </summary>
        private readonly Dictionary<string, double?> variables;

        /// <summary>
        /// Initializes a new instance of the <see cref="VariableNode"/> class.
        ///     assigns node with specific variable.
        /// </summary>
        /// <param name="variable"> variable to assign to node. </param>
        /// <param name="variables"> variables and there values. </param>
        public VariableNode(string variable, ref Dictionary<string, double?> variables)
        {
            this.Name = variable;
            this.variables = variables;
        }

        /// <summary>
        /// Evaluates variable nodes string as a double.
        /// </summary>
        /// <returns> value of variable node. </returns>
        public override double Evaluate()
        {
            double? value = this.variables[this.Name];
            if (value is not null)
            {
                return (double)value;
            }

            throw new InvalidOperationException("Variable Does Not Contain A Value");
        }
    }
}
