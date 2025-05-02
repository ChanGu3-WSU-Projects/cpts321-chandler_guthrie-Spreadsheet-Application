// <copyright file="TestLoad.cs" company="Chandler_Guthrie-WSU_ID:011801740">
// Copyright (c) Chandler_Guthrie-WSU_ID:011801740. All rights reserved.
// </copyright>

using System.Text;
using SpreadsheetEngine.ACell.ASpreadsheet;

namespace SpreadsheetEngine.AXMLFileProcess.Test.Load
{
    /// <summary>
    /// testing Load within XMLFileProccess.
    /// </summary>
    [TestFixture]
    internal class TestLoad
    {
        /// <summary>
        /// instance of spreadsheet to test in normal case.
        /// </summary>
        private Spreadsheet testSpreadsheet;

        /// <summary>
        /// string loaded into a spreadsheet for normal case.
        /// </summary>
        private string stringTestSpreadsheetNormal = "<root>\r\n  <spreadsheet>\r\n    <cell>\r\n      <row>0</row>\r\n      <column>0</column>\r\n      <text>5</text>\r\n      <bgcolor>4294944000</bgcolor>\r\n    </cell>\r\n    <cell>\r\n      <row>1</row>\r\n      <column>0</column>\r\n      <text>=A1+5</text>\r\n      <bgcolor>4294944000</bgcolor>\r\n    </cell>\r\n    <cell>\r\n      <row>1</row>\r\n      <column>1</column>\r\n      <text>Bell Cranel</text>\r\n      <bgcolor>4294967295</bgcolor>\r\n    </cell>\r\n    <cell>\r\n      <row>2</row>\r\n      <column>2</column>\r\n      <text></text>\r\n      <bgcolor>4294944000</bgcolor>\r\n    </cell>\r\n  </spreadsheet>\r\n</root>";

        /// <summary>
        /// string loaded into a spreadsheet for edge case no cell elements.
        /// </summary>
        private string stringTestSpreadsheetEdge0 = "<root>\r\n  <spreadsheet />\r\n</root>";

        /// <summary>
        /// string loaded into a spreadsheet for edge case the element of a a cell element that does not exist in loading.
        /// </summary>
        private string stringTestSpreadsheetEdge1 = "<root>\r\n  <spreadsheet>\r\n    <cell unusedattr=\"abc\">\r\n      <row>0</row>\r\n      <THISDOESNOTBELONG>SHROOMIE</THISDOESNOTBELONG>\r\n      <column>0</column>\r\n      <text>A BIG TEST</text>\r\n      <bgcolor></bgcolor>\r\n    </cell>\r\n  </spreadsheet>\r\n</root>";

        /// <summary>
        /// string loaded into a spreadsheet for exceptions.
        /// </summary>
        private string stringTestSpreadsheetException0 = "<root>\r\n  <spreadsheet />\r\n</root>";

        /// <summary>
        /// string loaded into a spreadsheet for exception with no root element.
        /// </summary>
        private string stringTestSpreadsheetException1 = string.Empty;

        /// <summary>
        /// string loaded into a spreadsheet for edge case the element of a a cell element that does not exist in loading.
        /// </summary>
        private string stringTestSpreadsheetException2 = "<root>\r\n  <spreadsheet>\r\n    <cell>\r\n      <row>0</row>\r\n      <ABROKENTAG>EIMOORHS</GATNEKORBA>\r\n      <column>0</column>\r\n      <text>A BIG TEST</text>\r\n      <bgcolor></bgcolor>\r\n    </cell>\r\n  </spreadsheet>\r\n</root>";

        /// <summary>
        /// setup for the tests.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            // normal testing
            this.testSpreadsheet = new Spreadsheet(5, 5);

            // edge testing
            this.testSpreadsheet = new Spreadsheet(5, 5);
        }

        /// <summary>
        /// normal case testing.
        /// </summary>
        /// <param name="testType"> type of test being run. </param>
        /// <returns> true when result it correct. </returns>
        [Test]
        [TestCase(0, ExpectedResult = true)]
        public bool? TestNormalCase(int testType)
        {
            MemoryStream memoryStream = new MemoryStream();
            byte[] bytesDocumentXML = Encoding.ASCII.GetBytes(this.stringTestSpreadsheetNormal);
            memoryStream.Write(bytesDocumentXML, 0, bytesDocumentXML.Length);
            memoryStream.Capacity = (int)memoryStream.Length;
            memoryStream.Position = 0;

            switch (testType)
            {
                case 0:
                    XMLFileProcess.Load(memoryStream, ref this.testSpreadsheet);
                    return this.testSpreadsheet.GetCell(0, 0)!.Value == "5" && this.testSpreadsheet.GetCell(0, 0)!.BGColor == 0xFFFFA500 &&
                           this.testSpreadsheet.GetCell(1, 0)!.Value == "10" && this.testSpreadsheet.GetCell(1, 0)!.BGColor == 0xFFFFA500 &&
                           this.testSpreadsheet.GetCell(1, 1)!.Text == "Bell Cranel" && this.testSpreadsheet.GetCell(2, 2)!.BGColor == 0xFFFFA500;
                default:
                    return null;
            }
        }

        /// <summary>
        /// edge case testing.
        /// </summary>
        /// <param name="testType"> type of test being run. </param>
        /// <returns> true when result is correct. </returns>
        [Test]
        [TestCase(0, ExpectedResult = true)]
        [TestCase(1, ExpectedResult = true)]
        public bool? TestEdgeCase(int testType)
        {
            MemoryStream memoryStream = new MemoryStream();
            byte[] bytesDocumentXML;

            switch (testType)
            {
                case 0:
                    bytesDocumentXML = Encoding.ASCII.GetBytes(this.stringTestSpreadsheetEdge0);
                    memoryStream.Write(bytesDocumentXML, 0, bytesDocumentXML.Length);
                    memoryStream.Capacity = (int)memoryStream.Length;
                    memoryStream.Position = 0;
                    XMLFileProcess.Load(memoryStream, ref this.testSpreadsheet);
                    for (int rowindex = 0; rowindex < this.testSpreadsheet.RowCount; rowindex++)
                    {
                        for (int columnindex = 0; columnindex < this.testSpreadsheet.ColumnCount; columnindex++)
                        {
                            if (this.testSpreadsheet.GetCell(rowindex, columnindex)!.Value != string.Empty && this.testSpreadsheet.GetCell(rowindex, columnindex)!.BGColor != 0xFFFFFFFF)
                            {
                                return false;
                            }
                        }
                    }

                    return true;
                case 1:
                    bytesDocumentXML = Encoding.ASCII.GetBytes(this.stringTestSpreadsheetEdge1);
                    memoryStream.Write(bytesDocumentXML, 0, bytesDocumentXML.Length);
                    memoryStream.Capacity = (int)memoryStream.Length;
                    memoryStream.Position = 0;
                    XMLFileProcess.Load(memoryStream, ref this.testSpreadsheet);
                    for (int rowindex = 0; rowindex < this.testSpreadsheet.RowCount; rowindex++)
                    {
                        for (int columnindex = 0; columnindex < this.testSpreadsheet.ColumnCount; columnindex++)
                        {
                            if (this.testSpreadsheet.GetCell(rowindex, columnindex)!.Value == "A BIG TEST" && this.testSpreadsheet.GetCell(rowindex, columnindex)!.BGColor != 0xFFFFFFFF)
                            {
                                return false;
                            }
                        }
                    }

                    return true;
                default:
                    return null;
            }
        }

        /// <summary>
        /// exception case testing.
        /// </summary>
        /// <param name="testType"> type of test being run. </param>
        /// <returns> exception type thrown inside try block. </returns>
        [Test]
        [TestCase(0, ExpectedResult = typeof(ArgumentNullException))] // stream is null
        [TestCase(1, ExpectedResult = typeof(ArgumentException))] // XML does not contain a root element of any name.
        [TestCase(2, ExpectedResult = typeof(ArgumentException))] // XML has element that does not match itself
        public Type? TestExceptionCase(int testType)
        {
            MemoryStream memoryStream = new MemoryStream();
            byte[] bytesDocumentXML;

            // after creating instance once exception is thrown make sure specific exception for input is being thrown.
            try
            {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                switch (testType)
                {
                    case 0:
                        bytesDocumentXML = Encoding.ASCII.GetBytes(this.stringTestSpreadsheetException0);
                        memoryStream.Write(bytesDocumentXML, 0, bytesDocumentXML.Length);
                        memoryStream.Capacity = (int)memoryStream.Length;
                        memoryStream.Position = 0;
                        XMLFileProcess.Load(null, ref this.testSpreadsheet);
                        break;
                    case 1:
                        bytesDocumentXML = Encoding.ASCII.GetBytes(this.stringTestSpreadsheetException1);
                        memoryStream.Write(bytesDocumentXML, 0, bytesDocumentXML.Length);
                        memoryStream.Capacity = (int)memoryStream.Length;
                        memoryStream.Position = 0;
                        XMLFileProcess.Load(memoryStream, ref this.testSpreadsheet);
                        break;
                    case 2:
                        bytesDocumentXML = Encoding.ASCII.GetBytes(this.stringTestSpreadsheetException2);
                        memoryStream.Write(bytesDocumentXML, 0, bytesDocumentXML.Length);
                        memoryStream.Capacity = (int)memoryStream.Length;
                        memoryStream.Position = 0;
                        XMLFileProcess.Load(memoryStream, ref this.testSpreadsheet);
                        break;
                    default:
                        return null;
                }
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

                return null;
            }
            catch (Exception exception)
            {
                return exception.GetBaseException().GetType();
            }
        }
    }
}
