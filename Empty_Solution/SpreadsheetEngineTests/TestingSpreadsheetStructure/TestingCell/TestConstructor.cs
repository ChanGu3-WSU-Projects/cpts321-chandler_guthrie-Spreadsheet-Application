// <copyright file="TestConstructor.cs" company="Chandler_Guthrie-WSU_ID:011801740">
// Copyright (c) Chandler_Guthrie-WSU_ID:011801740. All rights reserved.
// </copyright>

using SpreadsheetEngineTests.AMockAbstractCell;

namespace SpreadsheetEngine.ACell.Test.CellConstructor
{
    /// <summary>
    /// tesing the abstract class Cell by having another class
    ///     inherit without any implementation.
    /// </summary>
    [TestFixture]
    internal class TestConstructor
    {
        /// <summary>
        /// Cell with assigned row and column at zero.
        /// </summary>
        private Cell cellZeroTest;

        /// <summary>
        /// Cell testing for its type.
        /// </summary>
        private Cell cellTypeTest;

        /// <summary>
        /// setup for the tests.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.cellZeroTest = new MockAbstractCell(2, 4);
            this.cellTypeTest = new MockAbstractCell(0, 0);
        }

        /// <summary>
        /// normal case testing.
        /// </summary>
        /// <param name="dataName"> specific variable. </param>
        /// <returns> string of a specific value. </returns>
        [Test]
        [TestCase(nameof(Cell.RowIndex), ExpectedResult = "2")]
        [TestCase(nameof(Cell.ColumnIndex), ExpectedResult = "4")]
        [TestCase(nameof(Cell.Text), ExpectedResult = "")]
        [TestCase(nameof(Cell.Value), ExpectedResult = "")]
        public string? TestNormalCase(string? dataName = null)
        {
            // returns data from class when instanitated based on its name in the class.
            switch (dataName)
            {
                case nameof(Cell.RowIndex):
                    return this.cellZeroTest.RowIndex.ToString();
                case nameof(Cell.ColumnIndex):
                    return this.cellZeroTest.ColumnIndex.ToString();
                case nameof(Cell.Text):
                    return this.cellZeroTest.Text;
                case nameof(Cell.Value):
                    return this.cellZeroTest.Value;
            }

            return null;
        }

        /// <summary>
        /// edge case testing.
        ///    not a lot really here since its the constructor.
        /// </summary>
        [Test]
        public void TestEdgeCase()
        {
            Assert.That(this.cellTypeTest, Is.InstanceOf<Cell>());
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
                Cell newCell = new MockAbstractCell(rowIndex, columnIndex);
            }
            catch (Exception exception)
            {
                return exception.GetType();
            }

            return null;
        }
    }
}
