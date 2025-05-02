// <copyright file="TestCommandStructure.cs" company="Chandler_Guthrie-WSU_ID:011801740">
// Copyright (c) Chandler_Guthrie-WSU_ID:011801740. All rights reserved.
// </copyright>

using SpreadsheetEngine.ACell;
using SpreadsheetEngine.ACell.ASpreadsheet;

namespace SpreadsheetEngine.CommandStructure.Test.CommandStructure
{
    /// <summary>
    /// testing the command structure.
    /// </summary>
    internal class TestCommandStructure
    {
        private Spreadsheet spreadsheet;

        private Invoker invoker;

        /// <summary>
        /// setup for the tests.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.spreadsheet = new Spreadsheet(10, 10);
            this.invoker = new Invoker();
        }

        /// <summary>
        /// normal case testing.
        /// </summary>
        /// <param name="commandType"> type of command being tested. </param>
        /// <param name="isUndoTest"> testing for undo command. </param>
        /// <param name="isRedoTest"> testing for redo command. </param>
        /// <returns> true when data inside spreadsheet is set properly from invoker. </returns>
        [Test]
        [TestCase(typeof(CellTextChangeCommand), false, false, ExpectedResult = true)] // normal text change execution
        [TestCase(typeof(CellTextChangeCommand), true, false, ExpectedResult = true)] // text undo after execution
        [TestCase(typeof(CellTextChangeCommand), false, true, ExpectedResult = true)] // text redo after undo execution
        [TestCase(typeof(CellBGColorChangeCommand), false, false, ExpectedResult = true)] // normal BGcolor change execution
        [TestCase(typeof(CellBGColorChangeCommand), true, false, ExpectedResult = true)] // BGColor undo after execution
        [TestCase(typeof(CellBGColorChangeCommand), false, true, ExpectedResult = true)] // BGColor redo after undo execution
        public bool? TestNormalCase(Type? commandType, bool isUndoTest, bool isRedoTest)
        {
            if (commandType == typeof(CellTextChangeCommand))
            {
                this.invoker.SetCommand(new CellTextChangeCommand(ref this.spreadsheet, 0, 0, "Test"));
                this.invoker.ExecuteCommand();

                Cell currentCell = this.spreadsheet.GetCell(0, 0)!;

                if (isUndoTest == false && isRedoTest == false)
                {
                    if (currentCell.Text != "Test" || currentCell.Value != "Test")
                    {
                        return false;
                    }
                }
                else if (isUndoTest)
                {
                    this.invoker.UndoCommand();

                    if (currentCell.Text != string.Empty || currentCell.Value != string.Empty)
                    {
                        return false;
                    }
                }
                else if (isRedoTest)
                {
                    this.invoker.UndoCommand();
                    this.invoker.RedoCommand();

                    if (currentCell.Text != "Test" || currentCell.Value != "Test")
                    {
                        return false;
                    }
                }

                return true;
            }
            else if (commandType == typeof(CellBGColorChangeCommand))
            {
                List<Tuple<int, int, uint>> cellInfo = new List<Tuple<int, int, uint>>();
                cellInfo.Add(new Tuple<int, int, uint>(0, 0, 0xFFFFFFFF));

                this.invoker.SetCommand(new CellBGColorChangeCommand(ref this.spreadsheet, cellInfo, 0xFF00FF00));
                this.invoker.ExecuteCommand();

                Cell currentCell = this.spreadsheet.GetCell(0, 0)!;

                if (isUndoTest == false && isRedoTest == false)
                {
                    if (currentCell.BGColor != 0xFF00FF00)
                    {
                        return false;
                    }
                }
                else if (isUndoTest)
                {
                    this.invoker.UndoCommand();

                    if (currentCell.BGColor != 0xFFFFFFFF)
                    {
                        return false;
                    }
                }
                else if (isRedoTest)
                {
                    this.invoker.UndoCommand();
                    this.invoker.RedoCommand();

                    if (currentCell.BGColor != 0xFF00FF00)
                    {
                        return false;
                    }
                }

                return true;
            }

            return null;
        }

        /// <summary>
        /// edge case testing.
        /// </summary>
        /// <param name="testType"> test type. </param>
        /// <returns> true when result from invoker correct. </returns>
        [Test]
        [TestCase(0, ExpectedResult = true)] // executing same command twice.
        [TestCase(1, ExpectedResult = true)] // executing unod and redo with no commands does nothing does not throw exception
        public bool? TestEdgeCase(int testType)
        {
            if (testType == 0)
            {
                this.invoker.SetCommand(new CellTextChangeCommand(ref this.spreadsheet, 0, 0, "Test"));
                this.invoker.ExecuteCommand();

                Cell currentCell = this.spreadsheet.GetCell(0, 0)!;

                if (currentCell.Text != "Test" || currentCell.Value != "Test")
                {
                    return false;
                }

                this.invoker.ExecuteCommand();

                if (currentCell.Text != "Test" || currentCell.Value != "Test")
                {
                    return false;
                }

                return true;
            }
            else if (testType == 1)
            {
                this.invoker.UndoCommand();
                this.invoker.RedoCommand();

                return true;
            }

            return null;
        }

        /// <summary>
        /// exception case testing.
        /// </summary>
        /// <returns> exception type thrown inside try block. </returns>
        [Test]
        [TestCase(ExpectedResult = typeof(NullReferenceException))] // using the execute command when currentCommand is null
        public Type? TestExceptionCase()
        {
            // after creating instance once exception is thrown make sure specific exception for input is being thrown.
            try
            {
                this.invoker.ExecuteCommand();
                return null;
            }
            catch (Exception exception)
            {
                return exception.GetBaseException().GetType();
            }
        }
    }
}
