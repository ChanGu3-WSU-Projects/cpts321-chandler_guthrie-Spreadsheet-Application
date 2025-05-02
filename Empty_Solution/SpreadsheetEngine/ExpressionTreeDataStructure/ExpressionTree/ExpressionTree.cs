// <copyright file="ExpressionTree.cs" company="Chandler_Guthrie-WSU_ID:011801740">
// Copyright (c) Chandler_Guthrie-WSU_ID:011801740. All rights reserved.
// </copyright>

using System.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace SpreadsheetEngine.ExpressionTreeDS
{
    /// <summary>
    /// contains expression to be evaluated.
    /// </summary>
    internal class ExpressionTree
    {
        /// <summary>
        /// entrance node to the expression tree.
        /// </summary>
        private Node? root;

        /// <summary>
        /// dictionary where the key is the variable name and the value is the value of the variable.
        /// </summary>
        private Dictionary<string, double?> variables = new Dictionary<string, double?>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionTree"/> class.
        ///     construct tree from string expression.
        /// </summary>
        /// <param name="expression"> expression under tree construction. </param>
        public ExpressionTree(string expression) // maybe ref spreadsheet
        {
            this.Expression = expression;
            this.root = PostfixToExpressionTree(InfixToPostfix(expression), ref this.variables);
            this.VariableNames = new List<string>(this.variables.Keys);
        }

        private enum ExpressionState
        {
            Start = 0,
            Variable = 1,
            Constant = 2,
            Operator = 3,
            Whitespace = 4,
        }

        /// <summary>
        /// Gets the constructed tree expression.
        /// </summary>
        public string? Expression
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a list of the variable names in dictionary.
        /// </summary>
        public List<string> VariableNames
        {
            get;
            private set;
        }

        /// <summary>
        /// Sets a specific variables value in the dictionary list to the value specified
        ///     in input.
        /// </summary>
        /// <param name="variableName"> variable name in expression. </param>
        /// <param name="variableValue"> value to assign to variable. </param>
        public void SetVariable(string variableName, double variableValue)
        {
            if (!this.variables.ContainsKey(variableName))
            {
                throw new KeyNotFoundException("Variable Does Not Exist");
            }

            this.variables[variableName] = variableValue;
        }

        /// <summary>
        /// using the expression tree evaluates the expression made from constructor
        ///     then returns the value recursively through all nodes evaluate method.
        /// </summary>
        /// <returns> the result of the expression. </returns>
        public double Evaluate()
        {
            if (this.root != null)
            {
                return this.root.Evaluate();
            }

            return double.NaN;
        }

        /// <summary>
        /// Convert a infix expression into a postfix expression.
        /// </summary>
        /// <param name="infixExpression"> infix expression as a string. </param>
        /// <returns> postfix expression with whitespace. </returns>
        private static List<string> InfixToPostfix(string infixExpression)
        {
            // setup
            Stack<char> opStack = new Stack<char>();
            ExpressionState currentState = ExpressionState.Start;
            StringBuilder currentExpression = new StringBuilder(infixExpression);

            List<string> postfixExpression = new List<string>();

            StringBuilder currentOperand = new StringBuilder(string.Empty);
            List<string> tempOperatorsHolder = new List<string>();

            bool isSubExpressionLeftOfOperator = false;

            // used for checking paranthese
            char parentheseTemp;

            while (currentExpression.Length != 0)
            {
                // finds what type of character is in current index in expression.
                if (currentExpression[0] >= '0' && currentExpression[0] <= '9') // character is 0-9
                {
                    currentState = ExpressionState.Constant;
                }
                else if ((currentExpression[0] >= 'a' && currentExpression[0] <= 'z') || (currentExpression[0] >= 'A' && currentExpression[0] <= 'Z')) // character a-z or A-Z
                {
                    if (currentState == ExpressionState.Constant && currentOperand.Length == 1)
                    {
                        throw new InvalidExpressionException("A Variable Starts With A Number");
                    }

                    currentState = ExpressionState.Variable;
                }
                else if (currentExpression[0] == ' ') // character is whitespace
                {
                    currentState = ExpressionState.Whitespace;
                }
                else // otherwise treat all other characters as operators
                {
                    if (!OperatorNodeFactory.IsOperatorNode(currentExpression[0]) && currentExpression[0] != '(' && currentExpression[0] != ')')
                    {
                        throw new InvalidExpressionException("Operator Not Supported");
                    }

                    currentState = ExpressionState.Operator;
                }

                // depending on type of character in index do a specific task.
                switch (currentState)
                {
                    case ExpressionState.Constant: // char is 0-9
                        currentOperand.Append(currentExpression[0]);
                        break;

                    case ExpressionState.Variable: // char is a-z or A-Z
                        currentOperand.Append(currentExpression[0]);
                        break;

                    case ExpressionState.Whitespace: // character with whitespace
                        break;

                    case ExpressionState.Operator: // any other character

                        if (currentExpression[0] == '(') // add a parenthese when its a start of a sub expression
                        {
                            opStack.Push(currentExpression[0]);
                        }
                        else if (currentExpression[0] == ')') // end the sub expression created by opening parenthese
                        {
                            if (opStack.Count != 0)
                            {
                                // add operations from sub expression until opening parenthese
                                while (opStack.Peek() != '(')
                                {
                                    tempOperatorsHolder.Add(opStack.Pop().ToString());
                                }
                            }

                            parentheseTemp = ' ';
                            opStack.TryPop(out parentheseTemp);
                            if (parentheseTemp != '(')
                            {
                                throw new InvalidExpressionException("Expression Has An Extra Closing Parenthese");
                            }

                            // only add operand and operators if its the first closing parenthese
                            if (!isSubExpressionLeftOfOperator)
                            {
                                // add a single operand to POSTFIX LIST and clear the temporary holder of a operand.
                                if (currentOperand.Length != 0)
                                {
                                    postfixExpression.Add(currentOperand.ToString());
                                    currentOperand.Clear();
                                }
                                else
                                {
                                    throw new InvalidExpressionException("Operator In Expression Is Missing A Operand Infront Of It");
                                }

                                isSubExpressionLeftOfOperator = true;
                            }

                            // add operators to POSTFIX LIST and clear tempOperatorHolder.
                            for (int index = 0; index < tempOperatorsHolder.Count; index++)
                            {
                                postfixExpression.Add(tempOperatorsHolder[index]);
                            }

                            tempOperatorsHolder.Clear();
                        }
                        else if (opStack.Count != 0) // stack is not empty
                        {
                            // add a single operand to POSTFIX LIST and clear the temporary holder of a operand.
                            if (currentOperand.Length != 0)
                            {
                                postfixExpression.Add(currentOperand.ToString());
                                currentOperand.Clear();
                            }
                            else if (!isSubExpressionLeftOfOperator)
                            {
                                throw new InvalidExpressionException("Operator In Expression Is Missing A Operand Behind It");
                            }

                            // push an operator onto stack
                            if (opStack.Peek() == '(') // begining of a sub expression created by parenthese (Acting like start of stack)
                            {
                                OperatorNode currentOperator = OperatorNodeFactory.CreateOperatorNode(currentExpression[0]);
                                opStack.Push(currentOperator.Operator);
                            }
                            else // continuing sub expression or original expression.
                            {
                                OperatorNode currentOperator = OperatorNodeFactory.CreateOperatorNode(currentExpression[0]);
                                OperatorNode topOfStackOperator = OperatorNodeFactory.CreateOperatorNode(opStack.Peek());

                                // ---Associativity & Precendence--- depending on the precedence of the top of stack and current operator pop, push or do both to the stack. adding operators to POSTFIX LIST
                                if (currentOperator.Precedence > topOfStackOperator.Precedence)
                                {
                                    parentheseTemp = ' ';
                                    opStack.TryPeek(out parentheseTemp);
                                    while (opStack.Count != 0 && parentheseTemp != '(')
                                    {
                                        postfixExpression.Add(opStack.Pop().ToString());
                                        opStack.TryPeek(out parentheseTemp);
                                    }

                                    opStack.Push(currentOperator.Operator);
                                }
                                else if (currentOperator.Precedence < topOfStackOperator.Precedence)
                                {
                                    opStack.Push(currentOperator.Operator);
                                }
                                else
                                {
                                    if (currentOperator.Associativity == Associativity.LeftAssociative)
                                    {
                                        postfixExpression.Add(opStack.Pop().ToString());
                                        opStack.Push(currentOperator.Operator);
                                    }
                                    else if (currentOperator.Associativity == Associativity.RightAssociative)
                                    {
                                        opStack.Push(currentOperator.Operator);
                                    }
                                }
                            }

                            // setting up next operator after this to check for next operand and if its a sub expression
                            isSubExpressionLeftOfOperator = false;
                        }
                        else // stack is empty
                        {
                            // add a single operand to POSTFIX LIST and clear the temporary holder of a operand.
                            if (currentOperand.Length != 0)
                            {
                                postfixExpression.Add(currentOperand.ToString());
                                currentOperand.Clear();
                            }
                            else if (!isSubExpressionLeftOfOperator)
                            {
                                throw new InvalidExpressionException("Operator In Expression Is Missing A Operand Behind It");
                            }

                            OperatorNode currentOperator = OperatorNodeFactory.CreateOperatorNode(currentExpression[0]);
                            opStack.Push(currentOperator.Operator);

                            // setting up next operator after this to check for next operand and if its a sub expression
                            isSubExpressionLeftOfOperator = false;
                        }

                        break;
                }

                // go to next character to evaluate in original expression.
                currentExpression.Remove(0, 1);
            }

            if (currentOperand.Length != 0 && currentOperand.ToString() != string.Empty)
            {
                // add a single operand to POSTFIX LIST and clear the temporary holder of a operand.
                postfixExpression.Add(currentOperand.ToString());
                currentOperand.Clear();
            }
            else
            {
                // When original expression is not wrapped, there doesnt exists a currentoperand, and no operators added to operator holder on original sub expression
                if (!isSubExpressionLeftOfOperator && tempOperatorsHolder.Count == 0 && postfixExpression.Count != 0)
                {
                    throw new InvalidExpressionException("Expression Is Missing A Operand At The End");
                }
                else if (tempOperatorsHolder.Count > 0)
                {
                    // add operators to POSTFIX LIST and clear tempOperatorHolder.
                    for (int index = 0; index < tempOperatorsHolder.Count; index++)
                    {
                        postfixExpression.Add(tempOperatorsHolder[index]);
                    }

                    tempOperatorsHolder.Clear();
                }
            }

            if (opStack.Count != 0)
            {
                // adding operators to POSTFIX LIST
                parentheseTemp = ' ';
                opStack.TryPeek(out parentheseTemp);
                while (opStack.Count != 0 && parentheseTemp != '(')
                {
                    postfixExpression.Add(opStack.Pop().ToString());
                    opStack.TryPeek(out parentheseTemp);
                }
            }

            if (opStack.Count != 0)
            {
                throw new InvalidExpressionException("Expression Has An Extra Opening Parenthese");
            }

            return postfixExpression;
        }

        /// <summary>
        /// creates a expression tree from a postfix expression.
        /// </summary>
        /// <param name="postfixList"> operands and operators in postfix expression form as a list. </param>
        /// <param name="variables"> variables dictionary. </param>
        /// <returns> the root node of expression tree. </returns>
        private static Node? PostfixToExpressionTree(List<string> postfixList, ref Dictionary<string, double?> variables)
        {
            ExpressionState currentState = ExpressionState.Start;
            Stack<Node> nodeStack = new Stack<Node>();

            // special case when expression is empty
            if (postfixList.Count == 0 || postfixList[0] == string.Empty)
            {
                return null;
            }

            while (postfixList.Count != 0)
            {
                // Depending on either constant, variable, or operator go to a state
                if (postfixList[0][0] >= '0' && postfixList[0][0] <= '9') // character is 0-9
                {
                    currentState = ExpressionState.Constant;
                }
                else if ((postfixList[0][0] >= 'a' && postfixList[0][0] <= 'z') || (postfixList[0][0] >= 'A' && postfixList[0][0] <= 'Z')) // character a-z or A-Z
                {
                    currentState = ExpressionState.Variable;
                }
                else // otherwise treat all other characters as operators
                {
                    if (!OperatorNodeFactory.IsOperatorNode(postfixList[0][0]))
                    {
                        throw new InvalidExpressionException("Operator Not Supported");
                    }

                    currentState = ExpressionState.Operator;
                }

                switch (currentState)
                {
                    case ExpressionState.Constant: // add constant to stack
                        Node constantNode = new ConstantNode(Convert.ToDouble(postfixList[0]));
                        nodeStack.Push(constantNode);
                        break;

                    case ExpressionState.Variable: // add variable to stack
                        Node variableNode = new VariableNode(postfixList[0], ref variables);
                        nodeStack.Push(variableNode);
                        variables.TryAdd(postfixList[0], null);
                        break;

                    case ExpressionState.Operator: // pop first two onto operator node children then add to stack.
                        OperatorNode operatorNode = OperatorNodeFactory.CreateOperatorNode(postfixList[0][0]);
                        operatorNode.Right = nodeStack.Pop();
                        operatorNode.Left = nodeStack.Pop();
                        nodeStack.Push(operatorNode);
                        break;
                }

                // next index type state remoce current.
                postfixList.Remove(postfixList[0]);
            }

            return nodeStack.Pop();
        }

        /// <summary>
        /// (Depricated) creates left leaning simplified expression tree.
        /// </summary>
        /// <exception cref="InvalidDataException"> when values between operations is not valid in expression. </exception>
        [Obsolete("Used in previous homework")]
        private static Node? Instantiate(string expression, ref Dictionary<string, double?> variables)
        {
            // Make sure only specific values are allowed
            Regex constantRegex = new Regex("([\\d]+)");
            Regex variableRegex = new Regex("([a-zA-Z]+[\\d]*)+");

            // setup for deciding a specific operator only
            string[] valueTokens;
            char op;
            if ((valueTokens = expression.Split('+')).Length > 1)
            {
                op = '+';
            }
            else if ((valueTokens = expression.Split('-')).Length > 1)
            {
                op = '-';
            }
            else if ((valueTokens = expression.Split('*')).Length > 1)
            {
                op = '*';
            }
            else if ((valueTokens = expression.Split('/')).Length > 1)
            {
                valueTokens = expression.Split('/');
                op = '/';
            }
            else
            {
                throw new FormatException($"an operator exists that is not a valid");
            }

            // creates left leaning tree for simplified expression tree.
            OperatorNode? currentOperatorNode = Activator.CreateInstance(OperatorSignToType.Data[op]) as OperatorNode;
            Node? theRoot = currentOperatorNode!;
            for (int tokenCount = valueTokens.Length - 1; tokenCount > 0; tokenCount--)
            {
                // assigns currentoperatornode to left of operator node and a specific value node to the right
                if (tokenCount != 1)
                {
                    OperatorNode? operatorNode = Activator.CreateInstance(OperatorSignToType.Data[op]) as OperatorNode;

                    // create new node of variable or constant type
                    Node? newNode;
                    if (constantRegex.Match(valueTokens[tokenCount]).Value == valueTokens[tokenCount])
                    {
                        newNode = new ConstantNode(Convert.ToDouble(valueTokens[tokenCount]));
                    }
                    else if (variableRegex.Match(valueTokens[tokenCount]).Value == valueTokens[tokenCount])
                    {
                        variables.TryAdd(valueTokens[tokenCount], 0);
                        newNode = new VariableNode(valueTokens[tokenCount], ref variables);
                    }
                    else
                    {
                        throw new FormatException($"The value {valueTokens[tokenCount]} is not a valid variable or constant");
                    }

                    currentOperatorNode!.Right = newNode;
                    currentOperatorNode!.Left = operatorNode;

                    currentOperatorNode = operatorNode;
                }
                else // last operator will have two value nodes so assign one to left and right.
                {
                    Node?[] newValueNode = new Node[2];
                    for (int ind = 0; ind < 2; ind++)
                    {
                        // create new node of variable or constant type
                        if (constantRegex.Match(valueTokens[tokenCount - ind]).Value == valueTokens[tokenCount - ind])
                        {
                            newValueNode[ind] = new ConstantNode(Convert.ToDouble(valueTokens[tokenCount - ind]));
                        }
                        else if (variableRegex.Match(valueTokens[tokenCount - ind]).Value == valueTokens[tokenCount - ind])
                        {
                            variables.TryAdd(valueTokens[tokenCount - ind], 0);
                            newValueNode[ind] = new VariableNode(valueTokens[tokenCount - ind], ref variables);
                        }
                        else
                        {
                            throw new FormatException($"The value {valueTokens[tokenCount - ind]} is not a valid variable or constant");
                        }
                    }

                    currentOperatorNode!.Left = newValueNode[1];
                    currentOperatorNode!.Right = newValueNode[0];
                }
            }

            return theRoot;
        }
    }
}
