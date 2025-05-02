// <copyright file="TestSetTextFieldPropertyChanged.cs" company="Chandler_Guthrie-WSU_ID:011801740">
// Copyright (c) Chandler_Guthrie-WSU_ID:011801740. All rights reserved.
// </copyright>

using System.ComponentModel;
using System.Reflection;
using SpreadsheetEngineTests;
using SpreadsheetEngineTests.AMockAbstractCell;

namespace SpreadsheetEngine.ACell.Test.CellSetTextFieldPropertyChanged
{
    /// <summary>
    /// template for testing.
    /// </summary>
    [TestFixture]
    internal class TestSetTextFieldPropertyChanged
    {
        /// <summary>
        /// instance of Cell to test on.
        /// </summary>
        private Cell cell;

        /// <summary>
        /// event argument value for current test.
        /// </summary>
        private string? eventArgPropertyName;

        /// <summary>
        /// method data currently being tested on.
        /// </summary>
        private MethodInfo? methodInfo;

        /// <summary>
        /// setup for the tests.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.methodInfo = ReflectionMethods.GetMethod("SetTextFieldPropertyChanged", typeof(Cell), BindingFlags.Instance);
            this.cell = new MockAbstractCell(0, 1);
            this.cell.PropertyChanged += this.MethodSubscriber; // used to check broadcast for this method.
            this.eventArgPropertyName = null;
        }

        /// <summary>
        /// Normal case testing.
        /// </summary>
        /// <param name="textToAssign"> text assigning to cell. </param>
        /// <returns> returns the result of a method. </returns>
        [Test]
        [TestCase("AssignOnce", ExpectedResult = true)] // should contain name of the cell property
        [TestCase("AssignTwice", ExpectedResult = true)] // should not contain name of cell property
        public bool? TestNormalCase(string textToAssign)
        {
            // depending on textToAssign, returns if text was assigned correctly and also if event has been broadcasted when calling method.
            if (textToAssign == "AssignOnce")
            {
                this.methodInfo?.Invoke(this.cell, new object?[] { textToAssign });

                return this.cell.Text == "AssignOnce" && this.eventArgPropertyName == nameof(Cell.Text);
            }
            else if (textToAssign == "AssignTwice")
            {
                this.methodInfo?.Invoke(this.cell, new object?[] { textToAssign });
                this.eventArgPropertyName = null;
                this.methodInfo?.Invoke(this.cell, new object?[] { textToAssign });
                return this.cell.Text == "AssignTwice" && this.eventArgPropertyName == null;
            }

            return null;
        }

        /// <summary>
        /// edge case testing.
        /// </summary>
        /// <param name="textToAssign"> text assigning to cell. </param>
        /// <returns> returns the result of a method. </returns>
        [Test]
        [TestCase("AssignOnce", ExpectedResult = true)]
        [TestCase("AssignTwice", ExpectedResult = false)]
        public bool? TestEdgeCase(string textToAssign)
        {
            // depending on textToAssign, returns true when text is assigned otherwise false;
            if (textToAssign == "AssignOnce")
            {
                return (bool?)this.methodInfo?.Invoke(this.cell, new object?[] { textToAssign });
            }
            else if (textToAssign == "AssignTwice")
            {
                // calling twice for method to return false as the text is the same.
                this.methodInfo?.Invoke(this.cell, new object?[] { textToAssign });
                this.eventArgPropertyName = null;

                return (bool?)this.methodInfo?.Invoke(this.cell, new object?[] { textToAssign });
            }

            return null;
        }

        /// <summary>
        /// exception case testing.
        /// </summary>
        /// <returns> exception type thrown inside try block. </returns>
        [Test]
        [TestCase(ExpectedResult = typeof(OutOfMemoryException))]
        public Type? TestExceptionCase()
        {
            // return the original exception type if thrown when invoking method.
            try
            {
                this.methodInfo?.Invoke(this.cell, new object?[] { new string('0', int.MaxValue) });
                return null;
            }
            catch (Exception exception)
            {
                // reflection exception incapsulates original exception, returns the starting throw call type.
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
            this.eventArgPropertyName = eventArg.PropertyName?.ToString();
        }
    }
}
