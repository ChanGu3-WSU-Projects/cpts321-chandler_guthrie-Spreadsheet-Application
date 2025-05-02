// <copyright file="Spreadsheet.cs" company="Chandler_Guthrie-WSU_ID:011801740">
// Copyright (c) Chandler_Guthrie-WSU_ID:011801740. All rights reserved.
// </copyright>

using System.ComponentModel;
using System.Text.RegularExpressions;
using SpreadsheetEngine.ExpressionTreeDS;

namespace SpreadsheetEngine.ACell.ASpreadsheet
{
    /// <summary>
    /// maintainer for spreadsheet cells.
    /// </summary>
    public partial class Spreadsheet
    {
        /// <summary>
        /// 2D list of cells contained in this spreadsheet.
        /// </summary>
        private List<List<Cell>> spreadsheetCells;

        /// <summary>
        /// Initializes a new instance of the <see cref="Spreadsheet"/> class.
        ///      Insantiates the 2D list of this.spreadsheetCells.
        /// </summary>
        /// <param name="numberOfRows"> number of rows in 2D list of cells. </param>
        /// <param name="numberOfColumns"> number of columns in 2D list of cells. </param>
        public Spreadsheet(int numberOfRows, int numberOfColumns)
        {
            if (numberOfRows < 1 || numberOfColumns < 1)
            {
                throw new ArgumentException();
            }

            this.RowCount = numberOfRows;
            this.ColumnCount = numberOfColumns;

            this.spreadsheetCells = new List<List<Cell>>();
            for (int rowCount = 0; rowCount < numberOfRows; rowCount++)
            {
                List<Cell> newCellList = new List<Cell>();
                for (int columnCount = 0; columnCount < numberOfColumns; columnCount++)
                {
                    Cell newCell = new SpreadsheetCell(rowCount, columnCount);
                    newCell.PropertyChanged += this.OnCellTextSet;
                    newCell.PropertyChanged += this.OnCellBGColorSet;
                    newCellList.Add(newCell);
                }

                this.spreadsheetCells.Add(newCellList);
            }
        }

        /// <summary>
        /// List of subscribers to notify when a cells text property has been changed.
        /// </summary>
        public event PropertyChangedEventHandler? CellPropertyChanged;

        /// <summary>
        /// Gets Amount of columns in spreadsheet.
        /// </summary>
        public int ColumnCount
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets Amount of rows in spreadsheet.
        /// </summary>
        public int RowCount
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the Cell object at columnIndex and rowIndex within the spreadsheet.
        /// </summary>
        /// <param name="rowIndex"> row index of cell. </param>
        /// <param name="columnIndex"> column index of cell. </param>
        /// <returns> specifed Cell object. </returns>
        public Cell? GetCell(int rowIndex, int columnIndex)
        {
            if (rowIndex < 0 || columnIndex < 0)
            {
                throw new IndexOutOfRangeException();
            }
            else if (rowIndex > this.RowCount || columnIndex > this.ColumnCount)
            {
                return null;
            }

            return this.spreadsheetCells[rowIndex][columnIndex];
        }

        /// <summary>
        /// gets the column index from a column name.
        /// </summary>
        /// <param name="cellColumnName"> column portion of a column name. </param>
        /// <returns> column index. </returns>
        /// <exception cref="FormatException"> when cellColumnName is not in the correct name format for a column. </exception>
        private static int GetColumnIndex(string cellColumnName)
        {
            // matching for columnName
            Regex regexColumnName = new Regex("([A-Z]+)");
            Match cellColumnMatch = regexColumnName.Match(cellColumnName);

            if (!cellColumnMatch.Success || cellColumnMatch.Groups[0].Value != cellColumnName)
            {
                throw new FormatException("Variable Does Not Contain A Cell Column Name");
            }

            int columnIndex = 0;

            // Get the column index using character ascii
            for (int charIndex = cellColumnName.Length - 1; charIndex >= 0; charIndex--)
            {
                if (charIndex != cellColumnName.Length - 1)
                {
                    columnIndex = columnIndex + ((int)MathF.Pow(26, (cellColumnName.Length - 1) - charIndex) * ((int)(cellColumnName[charIndex] - 'A') + 1));
                }
                else
                {
                    columnIndex = columnIndex + (cellColumnName[charIndex] - 'A');
                }
            }

            return columnIndex;
        }

        /// <summary>
        /// setting cells value based off a change in text from an event.
        /// </summary>
        /// <param name="sender"> the cell sending the message. </param>
        /// <param name="cellInfo"> message context. </param>
        private void OnCellTextSet(object? sender, PropertyChangedEventArgs cellInfo)
        {
            if (sender == null)
            {
                throw new ArgumentNullException("sender cannot be null");
            }

            if (sender is not null)
            {
                SpreadsheetCell cell = (SpreadsheetCell)sender;

                // stop execution when property changed is not the text
                if (cellInfo.PropertyName != nameof(Cell.Text))
                {
                    return;
                }

                // unsubscribe from every cellReference
                foreach (SpreadsheetCell cellReference in cell.CellReferences)
                {
                    cellReference.ValuePropertyChanged -= cell.OnReferencedCellPropertyChanged;
                }

                // clear list to be setup from new cell text
                cell.CellReferences.Clear();

                // Assign Value to another cell if the matching regular expression is equal to the cell text
                Regex regexCellEvaluate = new Regex("=(.+)");
                Match evaluateTextMatch = regexCellEvaluate.Match(cell.Text);

                if (evaluateTextMatch.Success && evaluateTextMatch.Value == cell.Text) // Finds row and column index of cell and assigns cell value to cell broadcaster
                {
                    // try catch places errors/exceptions from user into cell if one is caught.
                    try
                    {
                        ExpressionTree expressionTree = new ExpressionTree(evaluateTextMatch.Groups[1].Value);

                        // matching for cell names
                        Regex regexCellName = new Regex("([A-Z]+)(\\d+)");

                        // matching for doubles
                        Regex regexConstantValue = new Regex("\\d+(\\.\\d+)?");

                        // cells value from variableName
                        string cellValue = string.Empty;

                        // cells name from variableName
                        string cellName = string.Empty;

                        // for each variable set the variable value only if it has a constant value.
                        foreach (string variableName in expressionTree.VariableNames)
                        {
                            Match cellNameMatch = regexCellName.Match(variableName);

                            // set value of a cell in the expression.
                            if (cellNameMatch.Success && cellNameMatch.Groups[0].Value == variableName)
                            {
                                cellName = variableName;

                                // get cell value from variable name.
                                int columnIndex = Spreadsheet.GetColumnIndex(cellNameMatch.Groups[1].Value);

                                // column index of cell name is out of bounds
                                if (columnIndex < 0 || columnIndex >= this.ColumnCount)
                                {
                                    throw new FormatException("!(bad reference)");
                                }

                                int rowIndex = Convert.ToInt32(cellNameMatch.Groups[2].Value) - 1;

                                // rowIndex index of cell name is out of bounds
                                if (rowIndex < 0 || rowIndex >= this.ColumnCount)
                                {
                                    throw new FormatException("!(bad reference)");
                                }

                                cellValue = this.spreadsheetCells[rowIndex][columnIndex].Value;

                                // add a cell to references that current cell is referencing and listens to that cells values change to change current value.
                                if ((this.spreadsheetCells[rowIndex][columnIndex] as SpreadsheetCell)!.CellReferences is not null)
                                {
                                    (this.spreadsheetCells[rowIndex][columnIndex] as SpreadsheetCell)!.ValuePropertyChanged += cell.OnReferencedCellPropertyChanged;
                                    cell.CellReferences!.Add(this.spreadsheetCells[rowIndex][columnIndex]);
                                }

                                // only assign a value if the cell is a double
                                Match cellValueMatch = regexConstantValue.Match(cellValue);
                                if (cellValueMatch.Success && cellValueMatch.Groups[0].Value == cellValue)
                                {
                                    expressionTree.SetVariable(variableName, Convert.ToDouble(cellValue));
                                }
                                else
                                {
                                    expressionTree.SetVariable(variableName, 0f);
                                }
                            }
                            else // variable name does not match to a cellName
                            {
                                throw new FormatException("!(bad reference)");
                            }
                        }

                        // detects circular reference from any other cell and itself.
                        SpreadsheetCell? cellCircleReference;
                        if (cell.IsCircleReference(out cellCircleReference))
                        {
                            if ((object)cell == (object)cellCircleReference!)
                            {
                                throw new FormatException("!(self reference)");
                            }
                            else
                            {
                                throw new FormatException("!(circular reference)");
                            }
                        }

                        // when the value isnt an expression but just equal to a single cell.
                        if (expressionTree.VariableNames.Count == 1 && cellValue != string.Empty && $"={cellName}" == evaluateTextMatch.Groups[0].Value)
                        {
                            cell.SetValue = cellValue;
                        }
                        else // otherwise its an expression evaluate it.
                        {
                            cell.SetValue = Convert.ToString(expressionTree.Evaluate());
                        }
                    }
                    catch (Exception ex)
                    {
                        cell.SetValue = $"{ex.Message}";
                    }
                }
                else // Set the broadcasted cell value to its evaluated value when no operator
                {
                    cell.SetValue = cell.Text;
                }
            }

            this.CellPropertyChanged?.Invoke(sender, new PropertyChangedEventArgs(nameof(Cell.Text)));
        }

        /// <summary>
        /// setting cells color based off a change in color from an event.
        /// </summary>
        /// <param name="sender"> the cell sending the message. </param>
        /// <param name="cellInfo"> message context. </param>
        private void OnCellBGColorSet(object? sender, PropertyChangedEventArgs cellInfo)
        {
            if (sender == null)
            {
                throw new ArgumentNullException();
            }

            if (sender is not null)
            {
                SpreadsheetCell cell = (SpreadsheetCell)sender;

                // stop execution when property changed is not the text
                if (cellInfo.PropertyName != nameof(Cell.BGColor))
                {
                    return;
                }
            }

            this.CellPropertyChanged?.Invoke(sender, new PropertyChangedEventArgs(nameof(Cell.BGColor)));
        }
    }
}
