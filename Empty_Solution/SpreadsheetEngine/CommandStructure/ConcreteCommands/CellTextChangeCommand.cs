// <copyright file="CellTextChangeCommand.cs" company="Chandler_Guthrie-WSU_ID:011801740">
// Copyright (c) Chandler_Guthrie-WSU_ID:011801740. All rights reserved.
// </copyright>

using SpreadsheetEngine.ACell.ASpreadsheet;

namespace SpreadsheetEngine.CommandStructure
{
    /// <summary>
    /// command for seting the text of a cell.
    /// </summary>
    public class CellTextChangeCommand : ICommand
    {
        /// <summary>
        /// spreadsheet command is being acted upon.
        /// </summary>
        private Spreadsheet spreadsheet;

        /// <summary>
        /// previous text in cell.
        /// </summary>
        private string previousCellText;

        /// <summary>
        /// the row index of the cell.
        /// </summary>
        private int cellRowIndex;

        /// <summary>
        /// the column index of the cell.
        /// </summary>
        private int cellColumnIndex;

        /// <summary>
        /// text to be set to cell text in execute.
        /// </summary>
        private string textToSet;

        /// <summary>
        /// Initializes a new instance of the <see cref="CellTextChangeCommand"/> class.
        /// </summary>
        /// <param name="spreadsheet"> spreadsheet being acted on. </param>
        /// <param name="cellRowIndex"> row of cell. </param>
        /// <param name="cellColumnIndex"> row of text. </param>
        /// <param name="textToSet"> text to set. </param>
        public CellTextChangeCommand(ref Spreadsheet spreadsheet, int cellRowIndex, int cellColumnIndex, string textToSet)
        {
            this.previousCellText = spreadsheet.GetCell(cellRowIndex, cellColumnIndex)!.Text;
            this.spreadsheet = spreadsheet;
            this.cellRowIndex = cellRowIndex;
            this.cellColumnIndex = cellColumnIndex;
            this.textToSet = textToSet;
        }

        /// <summary>
        /// gets the name of the celltochanged command.
        /// </summary>
        public string CommandName
        {
            get => "cell text change";
        }

        /// <summary>
        /// executes the command.
        /// </summary>
        public void ExecuteAction()
        {
            this.spreadsheet.GetCell(this.cellRowIndex, this.cellColumnIndex)!.Text = this.textToSet;
        }

        /// <summary>
        /// undoes the command executed.
        /// </summary>
        public void UndoAction()
        {
            this.spreadsheet.GetCell(this.cellRowIndex, this.cellColumnIndex)!.Text = this.previousCellText;
        }
    }
}
