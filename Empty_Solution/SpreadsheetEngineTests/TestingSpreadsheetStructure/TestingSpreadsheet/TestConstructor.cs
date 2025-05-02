// <copyright file="TestConstructor.cs" company="Chandler_Guthrie-WSU_ID:011801740">
// Copyright (c) Chandler_Guthrie-WSU_ID:011801740. All rights reserved.
// </copyright>

namespace SpreadsheetEngine.ACell.ASpreadsheet.Test.SpreadsheetConstructor
{
    /// <summary>
    /// testing the contstructor for spreadsheet class.
    /// </summary>
    [TestFixture]
    internal class TestConstructor
    {
        /// <summary>
        /// spreadsheet instance used with normal testing.
        /// </summary>
        private Spreadsheet spreadsheetNormalTest;

        /// <summary>
        /// spreadsheet instance used with exception Type testing.
        /// </summary>
        private Spreadsheet spreadsheetTypeTest;

        /// <summary>
        /// setup for the tests.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.spreadsheetNormalTest = new Spreadsheet(5, 5);
            this.spreadsheetTypeTest = new Spreadsheet(1, 1);
        }

        /// <summary>
        /// normal case testing.
        /// </summary>
        /// <param name="name"> someinput. </param>
        /// <returns> returns the result of a method. </returns>
        [Test]
        [TestCase(nameof(Spreadsheet.RowCount), ExpectedResult = "5")]
        [TestCase(nameof(Spreadsheet.ColumnCount), ExpectedResult = "5")]
        public string? TestNormalCase(string? name = null)
        {
            // Values are set correctly
            switch (name)
            {
                case nameof(Spreadsheet.RowCount):
                    return this.spreadsheetNormalTest.RowCount.ToString();
                case nameof(Spreadsheet.ColumnCount):
                    return this.spreadsheetNormalTest.ColumnCount.ToString();
            }

            return null;
        }

        /// <summary>
        /// edge case testing.
        ///    not a lot really here since no returns from the constructor.
        /// </summary>
        /// <param name="classType"> the type of class the instance is. </param>
        /// <returns> true if instance is of specific class. </returns>
        [Test]
        [TestCase(typeof(Spreadsheet), ExpectedResult = true)]
        public bool TestEdgeCase(Type classType)
        {
            return this.spreadsheetTypeTest.GetType() == classType;
        }

        /// <summary>
        /// exception case testing.
        /// </summary>
        /// <param name="numberOfRows"> rows inside spreadsheet. </param>
        /// <param name="numberOfColumns"> column inside spreadsheet. </param>
        /// <returns> exception type thrown inside try block. </returns>
        [Test]
        [TestCase(5, 0, ExpectedResult = typeof(ArgumentException))]
        [TestCase(0, 5, ExpectedResult = typeof(ArgumentException))]
        public Type? TestExceptionCase(int numberOfRows, int numberOfColumns)
        {
            // after creating instance once exception is thrown make sure specific exception for input is being thrown.
            try
            {
                Spreadsheet spreadsheet = new Spreadsheet(numberOfRows, numberOfColumns);
                return null;
            }
            catch (Exception exception)
            {
                return exception.GetType();
            }
        }
    }
}
