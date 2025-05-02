// <copyright file="SpreadsheetCell.cs" company="Chandler_Guthrie-WSU_ID:011801740">
// Copyright (c) Chandler_Guthrie-WSU_ID:011801740. All rights reserved.
// </copyright>

using System.ComponentModel;

namespace SpreadsheetEngine.ACell.ASpreadsheet
{
    /// <summary>
    /// maintainer for spreadsheet cells.
    /// </summary>
    public partial class Spreadsheet
    {
        /// <summary>
        /// inherits from abstract Cell class without concrete implementation.
        /// </summary>
        internal class SpreadsheetCell : Cell
        {
            /// <summary>
            /// cells that spreadsheet cell references.
            /// </summary>
            private List<object> cellReferences = new List<object>();

            /// <summary>
            /// Initializes a new instance of the <see cref="SpreadsheetCell"/> class.
            /// </summary>
            /// <param name="rowIndex"> row index of cell. </param>
            /// <param name="columnIndex"> column index of cell. </param>
            public SpreadsheetCell(int rowIndex, int columnIndex)
                : base(rowIndex, columnIndex)
            {
            }

            /// <summary>
            /// list of subscribers to notify when the value property of this cell is changed.
            /// </summary>
            public event PropertyChangedEventHandler? ValuePropertyChanged;

            /// <summary>
            /// Sets the value string to the specific value.
            /// </summary>
            public string SetValue
            {
                set
                {
                    if (this.value == value)
                    {
                        return;
                    }

                    this.value = value;
                    this.ValuePropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
                }
            }

            /// <summary>
            /// Gets the cellreferences within this spreadsheetcell.
            /// </summary>
            public List<object> CellReferences
            {
                get => this.cellReferences;
            }

            /// <summary>
            /// Checks if the cell has any circle references.
            /// </summary>
            /// <returns> true if it does. </returns>
            public bool IsCircleReference()
            {
                SpreadsheetCell? temp;
                return this.IsCircleReference(out temp);
            }

            /// <summary>
            /// checks if the cell has any circle references.
            /// </summary>
            /// <param name="cellCausingCircleReference"> cell being circle refrenced referenced. </param>
            /// <returns> true if it has a reference to a cell that references itself somewhere. </returns>
            public bool IsCircleReference(out SpreadsheetCell? cellCausingCircleReference)
            {
                HashSet<SpreadsheetCell> vistedCells = new HashSet<SpreadsheetCell>();

                Queue<SpreadsheetCell> queueCellsToVisit = new Queue<SpreadsheetCell>();

                queueCellsToVisit.Enqueue(this);
                SpreadsheetCell? currentCell = null;

                vistedCells.Add(this);

                // until every cell has been visted that is either a reference, a reference of a reference and so on keep looking at each cell to see
                // if instance of cell is being reference. if this instance cell is being referenced then its a circular reference.
                while (queueCellsToVisit.Count > 0)
                {
                    currentCell = queueCellsToVisit.Dequeue();

                    foreach (SpreadsheetCell cellReference in currentCell.CellReferences)
                    {
                        if ((object)this == cellReference as object)
                        {
                            cellCausingCircleReference = currentCell;
                            return true;
                        }

                        if (!vistedCells.Contains(cellReference))
                        {
                            vistedCells.Add(cellReference);
                            queueCellsToVisit.Enqueue(cellReference);
                        }
                    }
                }

                cellCausingCircleReference = null;
                return false;
            }
        }
    }
}
