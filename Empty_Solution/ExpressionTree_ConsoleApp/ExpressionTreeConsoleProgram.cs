// <copyright file="ExpressionTreeConsoleProgram.cs" company="Chandler_Guthrie-WSU_ID:011801740">
// Copyright (c) Chandler_Guthrie-WSU_ID:011801740. All rights reserved.
// </copyright>

using SpreadsheetEngine.ExpressionTreeDS;

namespace ExpressionTree_ConsoleApp
{
    /// <summary>
    /// The Program.
    /// </summary>
    internal class ExpressionTreeConsoleProgram
    {
        /// <summary>
        ///  The main entry point for the Console application.
        /// </summary>
        private static void Main()
        {
            ExpressionTree currentExpressionTree = new ExpressionTree("A1+B1+C1");

            bool isDone = false;

            DisplayMenuUI(currentExpressionTree.Expression!);
            while (!isDone)
            {
                // depending on read input operates on expression tree.
                string? optionNumber = Console.ReadLine();
                if (optionNumber is "1" or "2" or "3" or "4")
                {
                    switch (Convert.ToInt32(optionNumber))
                    {
                        case 1:
                            Console.Write("Enter new expression: ");
                            currentExpressionTree = new ExpressionTree(Console.ReadLine()!);
                            break;
                        case 2:
                            Console.Write("Enter variable name: ");
                            string? variableName = Console.ReadLine();
                            Console.Write("Enter variable value: ");
                            currentExpressionTree.SetVariable(variableName!, Convert.ToInt32(Console.ReadLine()!));
                            break;
                        case 3:
                            Console.WriteLine(currentExpressionTree.Evaluate());
                            break;
                        case 4:
                            isDone = true;
                            break;
                        default:
                            break;
                    }
                }

                DisplayMenuUI(currentExpressionTree.Expression!);
            }
        }

        /// <summary>
        /// Displays the UI of the menu and options user can choose.
        /// </summary>
        /// <param name="currentExpression"> current expression stored. </param>
        private static void DisplayMenuUI(string currentExpression)
        {
            Console.WriteLine(
               $"Menu (current expression=\"{currentExpression}\")\n" +
                "   1 = Enter a new expression\n" +
                "   2 = Set a variable value\n" +
                "   3 = Evaluate Tree\n" +
                "   4 = Quit");
        }
    }
}
