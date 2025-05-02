// <copyright file="TestGetColumnIndex.cs" company="Chandler_Guthrie-WSU_ID:011801740">
// Copyright (c) Chandler_Guthrie-WSU_ID:011801740. All rights reserved.
// </copyright>

using System.Reflection;
using SpreadsheetEngineTests;

namespace SpreadsheetEngine.ACell.ASpreadsheet.Test.SpreadsheetGetColumnIndex
{
    /// <summary>
    /// testing GetColumnIndex of a Name.
    /// </summary>
    [TestFixture]
    internal class TestGetColumnIndex
    {
        private MethodInfo? methodInfo;

        /// <summary>
        /// setup for the tests.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.methodInfo = ReflectionMethods.GetMethod("GetColumnIndex", typeof(Spreadsheet), BindingFlags.Static);
        }

        /// <summary>
        /// normal case testing.
        /// </summary>
        /// <param name="cellColumnName"> column name portion of the cell name. </param>
        /// <returns> column index based on the column name. </returns>
        [Test]
        [TestCase("A", ExpectedResult = 0)] // starting index
        [TestCase("AA", ExpectedResult = 26)] // next iteration starting
        [TestCase("ZZ", ExpectedResult = 701)] // endning of next iteration
        public int? TestNormalCase(string cellColumnName)
        {
            return this.methodInfo?.Invoke(null, new object?[] { cellColumnName }) as int?;
        }

        /// <summary>
        /// edge case testing.
        /// </summary>
        /// <param name="cellColumnName"> column name portion of the cell name. </param>
        /// <returns> column index based on the column name. </returns>
        [Test]
        [TestCase("ABCDE", ExpectedResult = 1040)] // random column row.
        public int? TestEdgeCase(string cellColumnName)
        {
            return this.methodInfo?.Invoke(null, new object?[] { cellColumnName }) as int?;
        }

        /// <summary>
        /// exception case testing.
        /// </summary>
        /// <param name="cellColumnName"> column name portion of the cell name. </param>
        /// <returns> column index based on the column name. </returns>
        [Test]
        [TestCase("f", ExpectedResult = typeof(FormatException))] // character other than A-Z
        [TestCase("1", ExpectedResult = typeof(FormatException))] // character that is not A-Z
        public Type? TestExceptionCase(string cellColumnName)
        {
            // after creating instance once exception is thrown make sure specific exception for input is being thrown.
            try
            {
                this.methodInfo?.Invoke(null, new object?[] { cellColumnName });
                return null;
            }
            catch (Exception exception)
            {
                return exception.GetBaseException().GetType();
            }
        }
    }
}
