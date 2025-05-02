// <copyright file="CellBGColorChangeCommand.cs" company="Chandler_Guthrie-WSU_ID:011801740">
// Copyright (c) Chandler_Guthrie-WSU_ID:011801740. All rights reserved.
// </copyright>

using SpreadsheetEngine.ACell.ASpreadsheet;

namespace SpreadsheetEngine.CommandStructure
{
    /// <summary>
    /// command for setting the text of cells.
    /// </summary>
    public class CellBGColorChangeCommand : ICommand
    {
        /// <summary>
        /// spreadsheet command is being acted upon.
        /// </summary>
        private Spreadsheet spreadsheet;

        /// <summary>
        /// the previous info about the cells.
        /// </summary>
        private List<Tuple<int, int, uint>> cellsInfo;

        /// <summary>
        /// the previous info about the cells.
        /// </summary>
        private List<Tuple<int, int, uint>> previousCellsInfo;

        /// <summary>
        /// the new color to assign.
        /// </summary>
        private uint bGColor;

        /// <summary>
        /// Initializes a new instance of the <see cref="CellBGColorChangeCommand"/> class.
        /// </summary>
        /// <param name="spreadsheet"> current spreadsheet being acted upon. </param>
        /// <param name="cellsInfo"> cells row index, column index, and previous Argb in that order. </param>
        /// <param name="bGColor"> the current color to update cells. </param>
        public CellBGColorChangeCommand(ref Spreadsheet spreadsheet, List<Tuple<int, int, uint>> cellsInfo, uint bGColor)
        {
            this.spreadsheet = spreadsheet;
            this.cellsInfo = cellsInfo;
            this.previousCellsInfo = this.cellsInfo;
            this.bGColor = bGColor;
        }

        /// <summary>
        /// gets the name of the cell BG color changed command.
        /// </summary>
        public string CommandName
        {
            get => "cell(s) color change";
        }

        /// <summary>
        /// executes the command changing color of cell(s).
        /// </summary>
        public void ExecuteAction()
        {
            foreach (Tuple<int, int, uint> cellInfo in this.cellsInfo)
            {
                this.spreadsheet.GetCell(cellInfo.Item1, cellInfo.Item2)!.BGColor = this.bGColor;
            }
        }

        /// <summary>
        /// undoes the command executed reverses color(s) back to original.
        /// </summary>
        public void UndoAction()
        {
            foreach (Tuple<int, int, uint> previousCellInfo in this.previousCellsInfo)
            {
                this.spreadsheet.GetCell(previousCellInfo.Item1, previousCellInfo.Item2)!.BGColor = previousCellInfo.Item3;
            }
        }
    }
}
