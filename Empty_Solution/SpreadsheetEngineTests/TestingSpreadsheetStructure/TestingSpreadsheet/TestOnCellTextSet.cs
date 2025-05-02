// <copyright file="TestOnCellTextSet.cs" company="Chandler_Guthrie-WSU_ID:011801740">
// Copyright (c) Chandler_Guthrie-WSU_ID:011801740. All rights reserved.
// </copyright>

using System.ComponentModel;
using System.Reflection;
using SpreadsheetEngineTests;

namespace SpreadsheetEngine.ACell.ASpreadsheet.Test.SpreadsheetOnCellTextSet
{
    /// <summary>
    /// testing OnCellTextSet method listener in spreadsheet.
    /// </summary>
    [TestFixture]
    internal class TestOnCellTextSet
    {
        /// <summary>
        /// method currently under testing need to be invoked to be used.
        /// </summary>
        private MethodInfo? methodInfo;

        /// <summary>
        /// the event sent after using method should be the evaluated value of the text.
        /// </summary>
        private string? eventArgValue;

        /// <summary>
        /// class instance where method resisdes.
        /// </summary>
        private Spreadsheet spreadsheet;

        /// <summary>
        /// setup for the tests.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.methodInfo = ReflectionMethods.GetMethod("OnCellTextSet", typeof(Spreadsheet), BindingFlags.Instance);
            this.spreadsheet = new Spreadsheet(10, 10);
            this.spreadsheet.CellPropertyChanged += this.MethodSubscriber;
            this.eventArgValue = null;
        }

        /// <summary>
        /// normal case testing.
        /// </summary>
        /// <param name="testCase"> specific test case. </param>
        /// <param name="testName"> name of specific test/text input for B2. </param>
        /// <param name="expectedString"> the expected string inside of specific cell. </param>
        /// <returns> returns the result of a method. </returns>
        [Test]
        [TestCase(2, "Testing", "Testing", ExpectedResult = true)] // string value
        [TestCase(2, "=A1", "Testing", ExpectedResult = true)] // single cell expression
        [TestCase(2, "=1", "1", ExpectedResult = true)] // single constant expression
        [TestCase(3, null, "!(self reference)", ExpectedResult = true)] // self reference
        [TestCase(4, null, "!(circular reference)", ExpectedResult = true)] // circular reference one to one
        [TestCase(5, null, "!(circular reference)", ExpectedResult = true)] // self reference indirection
        [TestCase(6, null, "!(bad reference)", ExpectedResult = true)] // bad reference from random variable
        [TestCase(7, null, "!(bad reference)", ExpectedResult = true)] // bad reference from cell variable that does not exist
        public bool? TestNormalCase(int testCase, string? testName, string expectedString)
        {
            if (this.methodInfo is not null)
            {
                switch (testCase)
                {
                    case 2:
                        // cell to be tested on or used to test another cell
                        this.spreadsheet.GetCell(0, 0)!.Text = "Testing";

                        // (0,0) == A1, (1, 1) == B2
                        if (this.spreadsheet.GetCell(0, 0) is not null && this.spreadsheet.GetCell(1, 1) is not null)
                        {
                            if (testName?[0] == '=')
                            {
                                // call method twice to evaluate first cell so that seconod can be evaluate based of that cell
                                this.methodInfo.Invoke(this.spreadsheet, new object?[] { this.spreadsheet.GetCell(0, 0), new PropertyChangedEventArgs(nameof(Cell.Text)) });
                                this.spreadsheet.GetCell(1, 1)!.Text = testName;
                                this.eventArgValue = null;
                                this.methodInfo.Invoke(this.spreadsheet, new object?[] { this.spreadsheet.GetCell(1, 1), new PropertyChangedEventArgs(nameof(Cell.Text)) });
                                return this.spreadsheet.GetCell(1, 1)!.Value == expectedString && this.eventArgValue == expectedString;
                            }
                            else
                            {
                                // evaluate single cell for value
                                this.eventArgValue = null;
                                this.methodInfo.Invoke(this.spreadsheet, new object?[] { this.spreadsheet.GetCell(0, 0), new PropertyChangedEventArgs(nameof(Cell.Text)) });
                                return this.spreadsheet.GetCell(0, 0)!.Value == expectedString && this.eventArgValue == expectedString;
                            }
                        }

                        break;
                    case 3:
                        this.spreadsheet.GetCell(0, 0)!.Text = "=A1";
                        return this.spreadsheet.GetCell(0, 0)!.Value == expectedString;
                    case 4:
                        this.spreadsheet.GetCell(0, 0)!.Text = "=B1";
                        this.spreadsheet.GetCell(0, 1)!.Text = "=A1";

                        return this.spreadsheet.GetCell(0, 0)!.Value == expectedString &&
                            this.spreadsheet.GetCell(0, 1)!.Value == expectedString;
                    case 5:
                        this.spreadsheet.GetCell(0, 0)!.Text = "=B1";
                        this.spreadsheet.GetCell(0, 1)!.Text = "=B2";
                        this.spreadsheet.GetCell(1, 1)!.Text = "=A2";
                        this.spreadsheet.GetCell(1, 0)!.Text = "=A1";

                        return this.spreadsheet.GetCell(0, 0)!.Value == expectedString &&
                            this.spreadsheet.GetCell(0, 1)!.Value == expectedString &&
                            this.spreadsheet.GetCell(1, 1)!.Value == expectedString &&
                            this.spreadsheet.GetCell(1, 0)!.Value == expectedString;
                    case 6:
                        this.spreadsheet.GetCell(0, 0)!.Text = "=Hello";
                        return this.spreadsheet.GetCell(0, 0)!.Value == expectedString;
                    case 7:
                        this.spreadsheet.GetCell(0, 0)!.Text = "=Z1234";
                        return this.spreadsheet.GetCell(0, 0)!.Value == expectedString;
                    default:
                        break;
                }
            }

            return null;
        }

        /// <summary>
        /// edge case testing.
        /// </summary>
        /// <param name="textAssigned"> text being assigned to cell text. </param>
        /// <param name="expectedString"> the expected string inside of specific cell. </param>
        /// <returns> returns the result of a method. </returns>
        [Test]
        [TestCase("=A1+1", "6", ExpectedResult = true)] // expression with cell name
        [TestCase("=1+1", "2", ExpectedResult = true)] // expression with no cell name
        [TestCase("1+1", "1+1", ExpectedResult = true)] // expression without = starting
        public bool? TestEdgeCase(string textAssigned, string expectedString)
        {
            if (this.spreadsheet.GetCell(0, 0) is not null && this.spreadsheet.GetCell(1, 1) is not null)
            {
                // cell used to test another cell
                this.spreadsheet.GetCell(0, 0)!.Text = "5";

                // call method twice to evaluate first cell so that seconod can be evaluate based of that cell
                if (this.methodInfo is not null)
                {
                    this.methodInfo.Invoke(this.spreadsheet, new object?[] { this.spreadsheet.GetCell(0, 0), new PropertyChangedEventArgs(nameof(Cell.Text)) });
                    this.spreadsheet.GetCell(1, 1)!.Text = textAssigned;
                    this.eventArgValue = null;
                    this.methodInfo.Invoke(this.spreadsheet, new object?[] { this.spreadsheet.GetCell(1, 1), new PropertyChangedEventArgs(nameof(Cell.Text)) });
                    return this.spreadsheet.GetCell(1, 1)!.Value == expectedString && this.eventArgValue == expectedString;
                }
            }

            return null;
        }

        /// <summary>
        /// exception case testing.
        /// </summary>
        /// <returns> exception type during testing. </returns>
        [Test]
        [TestCase(ExpectedResult = typeof(ArgumentNullException))] // sender of the function is null
        public Type? TestExceptionCase()
        {
            // after creating instance once exception is thrown make sure specific exception for input is being thrown.
            try
            {
                if (this.methodInfo is not null)
                {
                    this.methodInfo.Invoke(this.spreadsheet, new object?[] { this.spreadsheet.GetCell(20, 20), new PropertyChangedEventArgs(nameof(Cell.Text)) });
                }

                return null;
            }
            catch (Exception exception)
            {
                return exception.GetBaseException().GetType();
            }
        }

        /// <summary>
        /// assigns eventArgValue value based off eventaArg from broadcaster.
        /// </summary>
        /// <param name="sender"> the broadcaster object. </param>
        /// <param name="eventArg"> the arguments sent from broadcaster. </param>
        private void MethodSubscriber(object? sender, PropertyChangedEventArgs eventArg)
        {
            if (sender is not null)
            {
                this.eventArgValue = ((Cell)sender).Value;
            }
        }
    }
}
