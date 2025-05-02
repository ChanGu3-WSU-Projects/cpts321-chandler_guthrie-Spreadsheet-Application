// <copyright file="Form1.cs" company="Chandler_Guthrie-WSU_ID:011801740">
// Copyright (c) Chandler_Guthrie-WSU_ID:011801740. All rights reserved.
// </copyright>

using System.ComponentModel;
using SpreadsheetEngine.ACell;
using SpreadsheetEngine.ACell.ASpreadsheet;
using SpreadsheetEngine.AXMLFileProcess;
using SpreadsheetEngine.CommandStructure;

namespace Spreadsheet_Chandler_Guthrie
{
    /// <summary>
    /// windform spreadsheet.
    /// </summary>
    public partial class SpreadsheetForm : Form
    {
        /// <summary>
        /// the spreadsheet made in logic engine.
        /// </summary>
        private Spreadsheet spreadsheet;

        /// <summary>
        /// invoker for command pattern (bassically the host for all commands).
        /// </summary>
        private Invoker invoker;

#pragma warning disable CS8618 // I am setting the spreadsheet and invoker but IDE does not think so
        /// <summary>
        /// Initializes a new instance of the <see cref="SpreadsheetForm"/> class.
        /// </summary>
        public SpreadsheetForm()
        {
            this.InitializeSpreadsheet();
            this.InitializeInvoker();

            this.InitializeComponent();
            this.InitializeDataGrid();
        }
#pragma warning restore CS8618 // I am setting the spreadsheet and invoker but IDE does not think so

        /// <summary>
        /// initalizes the spreadsheet of the form.
        /// </summary>
        private void InitializeSpreadsheet()
        {
            this.spreadsheet = new Spreadsheet(50, 26);
            this.spreadsheet.CellPropertyChanged += this.OnCellTextSet;
            this.spreadsheet.CellPropertyChanged += this.OnCellBGColorSet;
        }

        /// <summary>
        /// initalizes the invoker of the form.
        /// </summary>
        private void InitializeInvoker()
        {
            this.invoker = new Invoker();
            this.invoker.UndoStackChanged += this.OnUndoStackChanged;
            this.invoker.RedoStackChanged += this.OnRedoStackChanged;
        }

        /// <summary>
        /// Initializes the data grid view by clearing any templated view.
        ///     Then add fixed rows and columns.
        /// </summary>
        private void InitializeDataGrid()
        {
            // Clears any row or columns currently in UI[Design].
            this.dataGridView.Rows.Clear();
            this.dataGridView.Columns.Clear();

            // Creates 26 columns with the name A-Z using Ascii binary
            for (int columnCount = 0; columnCount < 26; columnCount++)
            {
                char uppercaseLetter = (char)('A' + columnCount);
                int currentColumnIndex = this.dataGridView.Columns.Add("column" + uppercaseLetter.ToString(), uppercaseLetter.ToString());
                this.dataGridView.Columns[currentColumnIndex].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            // Creates 50 rows with the name of the number
            for (int rowCount = 0; rowCount < 50; rowCount++)
            {
                this.dataGridView.Rows.Add();
                this.dataGridView.Rows.SharedRow(rowCount).HeaderCell.Value = (rowCount + 1).ToString();
            }

            // (every 5 pixels seems to be a single terminal)
            this.dataGridView.RowHeadersWidth = 48;

            this.undoToolStripMenuItem.Text = "Undo:";
            this.undoToolStripMenuItem.Enabled = false;

            this.redoToolStripMenuItem.Text = "Redo:";
            this.redoToolStripMenuItem.Enabled = false;
        }

        /// <summary>
        /// Started editing a cell.
        /// </summary>
        /// <param name="sender"> the datagridview being edited. </param>
        /// <param name="cellInfo"> info about the current cell being edited. </param>
        /// <exception cref="Exception"> If getting the cell is null it returns an exception. </exception>
        private void OnCellBeginEdit(object sender, DataGridViewCellCancelEventArgs cellInfo)
        {
            DataGridView senderDataGridView = (DataGridView)sender;

            if (this.spreadsheet.GetCell(cellInfo.RowIndex, cellInfo.ColumnIndex) is not null)
            {
                senderDataGridView[cellInfo.ColumnIndex, cellInfo.RowIndex].Value = this.spreadsheet.GetCell(cellInfo.RowIndex, cellInfo.ColumnIndex)!.Text;
            }
            else
            {
                throw new IndexOutOfRangeException("ERROR: Getting Cell");
            }
        }

        /// <summary>
        /// Completed editing a cell.
        /// </summary>
        /// <param name="sender"> the datagridview being edited. </param>
        /// <param name="cellInfo"> info about the current cell being edited. </param>
        /// <exception cref="Exception"> If getting the cell is null it returns an exception. </exception>
        private void OnCellEndEdit(object sender, DataGridViewCellEventArgs cellInfo)
        {
            DataGridView senderDataGridView = (DataGridView)sender;

            if (this.spreadsheet.GetCell(cellInfo.RowIndex, cellInfo.ColumnIndex) is not null)
            {
                if (this.spreadsheet.GetCell(cellInfo.RowIndex, cellInfo.ColumnIndex)!.Text != (string)senderDataGridView[cellInfo.ColumnIndex, cellInfo.RowIndex].Value) // don't create command when cell is not changed.
                {
                    // set the spreadsheet cell to the value of the data grid cell.
                    if (senderDataGridView[cellInfo.ColumnIndex, cellInfo.RowIndex].Value is not null)
                    {
                        this.invoker.SetCommand(new CellTextChangeCommand(ref this.spreadsheet, cellInfo.RowIndex, cellInfo.ColumnIndex, (string)senderDataGridView[cellInfo.ColumnIndex, cellInfo.RowIndex].Value));
                        this.invoker.ExecuteCommand();
                    }
                    else
                    {
                        this.invoker.SetCommand(new CellTextChangeCommand(ref this.spreadsheet, cellInfo.RowIndex, cellInfo.ColumnIndex, string.Empty));
                        this.invoker.ExecuteCommand();
                    }
                }

                // set data grid cell to spreadsheet cell.
                this.dataGridView[cellInfo.ColumnIndex, cellInfo.RowIndex].Value = this.spreadsheet.GetCell(cellInfo.RowIndex, cellInfo.ColumnIndex)!.Value;
            }
            else
            {
                throw new IndexOutOfRangeException("ERROR: Getting Cell");
            }
        }

        /// <summary>
        /// setting cells value based off a change in text from an event.
        /// </summary>
        /// <param name="sender"> the cell sending the message. </param>
        /// <param name="cellInfo"> message context. </param>
        private void OnCellTextSet(object? sender, PropertyChangedEventArgs cellInfo)
        {
            // stop execution when property changed is not the text
            if (cellInfo.PropertyName != nameof(Cell.Text))
            {
                return;
            }

            if (sender is not null)
            {
                Cell cell = (Cell)sender;

                this.dataGridView[cell.ColumnIndex, cell.RowIndex].Value = cell.Value;
            }
        }

        private void OnCellBGColorSet(object? sender, PropertyChangedEventArgs cellInfo)
        {
            // stop execution when property changed is not the text
            if (cellInfo.PropertyName != nameof(Cell.BGColor))
            {
                return;
            }

            if (sender is not null)
            {
                Cell cell = (Cell)sender;

                this.dataGridView[cell.ColumnIndex, cell.RowIndex].Style.BackColor = Color.FromArgb((int)cell.BGColor);
            }
        }

        /// <summary>
        /// Whenever undo stack is changed update the Form undo button.
        /// </summary>
        /// <param name="sender"> the invoker. </param>
        /// <param name="e"> event args. </param>
        private void OnUndoStackChanged(object? sender, EventArgs e)
        {
            if (this.invoker.NameUndoTop is null)
            {
                this.undoToolStripMenuItem.Enabled = false;
                this.undoToolStripMenuItem.Text = "Undo:";
            }
            else
            {
                this.undoToolStripMenuItem.Enabled = true;
                this.undoToolStripMenuItem.Text = "Undo: " + this.invoker.NameUndoTop;
            }
        }

        /// <summary>
        /// Whenever redo stack is changed update the Form redo button.
        /// </summary>
        /// <param name="sender"> the invoker. </param>
        /// <param name="e"> event args. </param>
        private void OnRedoStackChanged(object? sender, EventArgs e)
        {
            if (this.invoker.NameRedoTop is null)
            {
                this.redoToolStripMenuItem.Enabled = false;
                this.redoToolStripMenuItem.Text = "Redo:";
            }
            else
            {
                this.redoToolStripMenuItem.Enabled = true;
                this.redoToolStripMenuItem.Text = "Redo: " + this.invoker.NameRedoTop;
            }
        }

        /// <summary>
        /// (Depricated) Demo For the enigne to UI communication.
        /// </summary>
        /// <param name="sender"> the button object. </param>
        /// <param name="e"> argument from button. </param>
        [Obsolete("Used In Previous Homework")]
        private void OnPerformDemoClick(object sender, EventArgs e)
        {
            // Add 50 somewhat random text.
            for (int indexColumn = 0; indexColumn < 25; indexColumn++)
            {
                for (int indexRow = 0; indexRow < 2; indexRow++)
                {
                    if (indexColumn != 1 && indexColumn != 0)
                    {
                        this.spreadsheet.GetCell(indexRow, indexColumn)!.Text = "Hello World!";
                    }
                    else
                    {
                        this.spreadsheet.GetCell(indexRow + 5, indexColumn + 5)!.Text = "Hello World!";
                    }
                }
            }

            // add text down B column
            for (int addRowBIndex = 0; addRowBIndex < 50; addRowBIndex++)
            {
                this.spreadsheet.GetCell(addRowBIndex, 1)!.Text = $"This is cell B{addRowBIndex + 1}";
            }

            // add text down A column from B Column all the way down
            for (int addRowAIndex = 0; addRowAIndex < 50; addRowAIndex++)
            {
                this.spreadsheet.GetCell(addRowAIndex, 0)!.Text = "=B" + (addRowAIndex + 1).ToString();
            }
        }

        /// <summary>
        /// listener to the undo button event.
        ///     undoes the last most command.
        /// </summary>
        /// <param name="sender"> undo button object. </param>
        /// <param name="e"> arguments. </param>
        private void OnUndoActionClick(object sender, EventArgs e)
        {
            this.invoker.UndoCommand();
        }

        /// <summary>
        /// listener to the redo button event.
        ///     redoes the last most undone command.
        /// </summary>
        /// <param name="sender"> redo button object. </param>
        /// <param name="e"> arguments. </param>
        private void OnRedoActionClick(object sender, EventArgs e)
        {
            this.invoker.RedoCommand();
        }

        /// <summary>
        /// changing color of selected cells.
        /// </summary>
        /// <param name="sender"> button for chaning color. </param>
        /// <param name="e"> arguments. </param>
        private void OnChangeBGColorCellsClick(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();

            if (this.dataGridView.SelectedCells.Count == 1)
            {
                colorDialog.Color = Color.FromArgb((int)this.spreadsheet.GetCell(this.dataGridView.SelectedCells[0].RowIndex, this.dataGridView.SelectedCells[0].ColumnIndex)!.BGColor);
            }

            // show color dialog waiting for the ok result.
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                List<Tuple<int, int, uint>> cellsInfo = new List<Tuple<int, int, uint>>();
                for (int index = 0; index < this.dataGridView.SelectedCells.Count; index++)
                {
                    int rowIndex = this.dataGridView.SelectedCells[index].RowIndex;
                    int columnIndex = this.dataGridView.SelectedCells[index].ColumnIndex;
                    uint colorARGB = this.spreadsheet.GetCell(rowIndex, columnIndex)!.BGColor;

                    cellsInfo.Add(new Tuple<int, int, uint>(rowIndex, columnIndex, colorARGB));
                }

                this.invoker.SetCommand(new CellBGColorChangeCommand(ref this.spreadsheet, cellsInfo, (uint)colorDialog.Color.ToArgb()));
                this.invoker.ExecuteCommand();
            }
        }

        /// <summary>
        /// loads a new file.
        /// </summary>
        /// <param name="sender"> load button. </param>
        /// <param name="e"> arguments from button. </param>
        private void OnFileLoadClick(object sender, EventArgs e)
        {
            Stream stream;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "xml files (*.xml)|*.xml";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // reset form
                    this.InitializeDataGrid();

                    // reset spreadsheet
                    this.InitializeSpreadsheet();

                    // reset undo/redo
                    this.undoToolStripMenuItem.Text = "Undo:";
                    this.undoToolStripMenuItem.Enabled = false;
                    this.redoToolStripMenuItem.Text = "Redo:";
                    this.redoToolStripMenuItem.Enabled = false;
                    this.InitializeInvoker();

                    stream = openFileDialog.OpenFile();

                    XMLFileProcess.Load(stream, ref this.spreadsheet);
                }
            }
        }

        /// <summary>
        /// saves current file.
        /// </summary>
        /// <param name="sender"> save button. </param>
        /// <param name="e"> arguments from button. </param>
        private void OnFileSaveClick(object sender, EventArgs e)
        {
            this.SaveNewFile();
        }

        private void SaveNewFile()
        {
            Stream stream;
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "xml files (*.xml)|*.xml";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                if ((stream = saveFileDialog.OpenFile()) != null)
                {
                    XMLFileProcess.Save(stream, this.spreadsheet);
                    stream.Close();
                }
            }
        }
    }
}