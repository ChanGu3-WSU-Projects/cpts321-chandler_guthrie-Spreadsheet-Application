// <copyright file="Cell.cs" company="Chandler_Guthrie-WSU_ID:011801740">
// Copyright (c) Chandler_Guthrie-WSU_ID:011801740. All rights reserved.
// </copyright>

using System.ComponentModel;

namespace SpreadsheetEngine.ACell
{
    /// <summary>
    /// represents a cell in the spreadsheet.
    /// </summary>
    public abstract class Cell : INotifyPropertyChanged
    {
        /// <summary>
        /// contains typed text in cell.
        /// </summary>
        protected string text = string.Empty;

        /// <summary>
        /// contains evaluated text in cell.
        /// </summary>
        protected string value = string.Empty;

        /// <summary>
        /// color of the cells background.
        /// </summary>
        protected uint bGColor = 0xFFFFFFFF;

        /// <summary>
        /// Initializes a new instance of the <see cref="Cell"/> class.
        /// </summary>
        /// <param name="rowIndex"> row index of cell. </param>
        /// <param name="columnIndex"> column index of cell. </param>
        public Cell(int rowIndex, int columnIndex)
        {
            this.text = string.Empty;
            this.value = string.Empty;
            this.ColumnIndex = columnIndex;
            this.RowIndex = rowIndex;

            if (columnIndex < 0 || rowIndex < 0)
            {
                throw new IndexOutOfRangeException("Argument index was outside the bounds of a cell.");
            }
        }

        /// <summary>
        /// list of subscribers to notify when the text property of this cell is changed.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Gets cell row index.
        /// </summary>
        public int RowIndex
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets cell Column index.
        /// </summary>
        public int ColumnIndex
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets typed text in cell.
        /// </summary>
        public string Text
        {
            get => this.text;
            set => this.SetTextFieldPropertyChanged(value);
        }

        /// <summary>
        /// Gets evaluated text in cell.
        /// </summary>
        public string Value
        {
            get => this.value;
        }

        /// <summary>
        /// Gets or Sets the background color of the cell.
        /// </summary>
        public uint BGColor
        {
            get => this.bGColor;
            set
            {
                if (this.bGColor == value)
                {
                    return;
                }

                this.bGColor = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Cell.BGColor)));
            }
        }

        /// <summary>
        /// invokes PropertyChanged as if text has been changed to re evalutate.
        /// </summary>
        /// <param name="sender"> cell that has a reference to current cell.  </param>
        /// <param name="eventArgs"> null. </param>
        public void OnReferencedCellPropertyChanged(object? sender, PropertyChangedEventArgs eventArgs)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Cell.Text)));
        }

        /// <summary>
        /// assign the value to field text only if the value assigned is different from the field text.
        ///     notifying any method subscribed to PropertyChanged when field text is assigned to
        ///     the value.
        /// </summary>
        /// <param name="value"> value to assign to field. </param>
        /// <returns> true if the value is assigned to text. </returns>
        private bool SetTextFieldPropertyChanged(string value)
        {
            if (this.text == value)
            {
                return false;
            }

            this.text = value;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Cell.Text)));

            return true;
        }
    }
}
