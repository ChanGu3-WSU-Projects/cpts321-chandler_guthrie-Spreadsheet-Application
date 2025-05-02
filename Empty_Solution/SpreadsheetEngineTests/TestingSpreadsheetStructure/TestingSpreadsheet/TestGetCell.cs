// <copyright file="TestGetCell.cs" company="Chandler_Guthrie-WSU_ID:011801740">
// Copyright (c) Chandler_Guthrie-WSU_ID:011801740. All rights reserved.
// </copyright>

namespace SpreadsheetEngine.ACell.ASpreadsheet.Test.SpreadsheetGetCell
{
    /// <summary>
    /// testing the GetCell method within Spreadsheet.
    /// </summary>
    [TestFixture]
    internal class TestGetCell
    {
        /// <summary>
        /// instance of spreadsheet to test its GetCell method.
        /// </summary>
        private Spreadsheet spreadsheetTesting;

        /// <summary>
        /// setup for the tests.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.spreadsheetTesting = new Spreadsheet(4, 6);
        }

        /// <summary>
        /// normal case testing.
        /// </summary>
        /// <param name="dataName"> name of a specific data item. </param>
        /// <returns> string of the value. </returns>
        [Test]
        [TestCase(nameof(Cell.RowIndex), ExpectedResult = "2")]
        [TestCase(nameof(Cell.ColumnIndex), ExpectedResult = "3")]
        [TestCase(nameof(Cell.Text), ExpectedResult = "Text")]
        [TestCase(nameof(Cell.Value), ExpectedResult = "")]
        public string? TestNormalCase(string? dataName = null)
        {
            // depending on name of the data test that datas value up to the expected value.
            if (this.spreadsheetTesting.GetCell(2, 3) != null)
            {
                switch (dataName)
                {
                    case nameof(Cell.RowIndex):
                        return this.spreadsheetTesting.GetCell(2, 3)?.RowIndex.ToString();
                    case nameof(Cell.ColumnIndex):
                        return this.spreadsheetTesting.GetCell(2, 3)?.ColumnIndex.ToString();
                    case nameof(Cell.Text):
                        if (this.spreadsheetTesting.GetCell(2, 3) is not null)
                        {
                            this.spreadsheetTesting.GetCell(2, 3)!.Text = "Text";
                        }

                        return this.spreadsheetTesting.GetCell(2, 3)?.Text.ToString();
                    case nameof(Cell.Value):
                        return this.spreadsheetTesting.GetCell(2, 3)?.Value.ToString();
                    default:
                        break;
                }
            }

            return null;
        }

        /// <summary>
        /// edge case testing.
        /// </summary>
        /// <param name="rowIndex"> the row index of spreadsheet. </param>
        /// <param name="columnIndex"> the column index of spreadsheet. </param>
        /// <param name="testType"> the type of test that is being made. </param>
        /// <returns> true or false depending on expected output. </returns>
        [Test]
        [TestCase(0, 0, "typeof(Cell)", ExpectedResult = true)]
        [TestCase(10, 0, "IndexOverflow", ExpectedResult = true)]
        [TestCase(0, 10, "IndexOverflow", ExpectedResult = true)]
        public bool? TestEdgeCase(int rowIndex, int columnIndex, string? testType = null)
        {
            // does test depending on testType
            if (testType == "typeof(Cell)")
            {
                return this.spreadsheetTesting.GetCell(rowIndex, columnIndex) is Cell; // Is the Type a Cell?
            }
            else if (testType == "IndexOverflow")
            {
                return this.spreadsheetTesting.GetCell(rowIndex, columnIndex) == null; // does the cell exist in spreadsheet at index's?
            }

            return null;
        }

        /// <summary>
        /// exception case testing.
        /// </summary>
        /// <param name="rowIndex"> row index of cell. </param>
        /// <param name="columnIndex"> column index of cell. </param>
        /// <returns> exception type thrown inside try block. </returns>
        [Test]
        [TestCase(-5, 0, ExpectedResult = typeof(IndexOutOfRangeException))]
        [TestCase(0, -5, ExpectedResult = typeof(IndexOutOfRangeException))]
        public Type? TestExceptionCase(int rowIndex, int columnIndex)
        {
            // after creating instance once exception is thrown make sure specific exception for input is being thrown.
            try
            {
                this.spreadsheetTesting.GetCell(rowIndex, columnIndex);
                return null;
            }
            catch (Exception exception)
            {
                return exception.GetType();
            }
        }
    }
}