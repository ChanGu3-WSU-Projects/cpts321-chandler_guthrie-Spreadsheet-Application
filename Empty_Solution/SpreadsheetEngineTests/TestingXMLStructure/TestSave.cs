// <copyright file="TestSave.cs" company="Chandler_Guthrie-WSU_ID:011801740">
// Copyright (c) Chandler_Guthrie-WSU_ID:011801740. All rights reserved.
// </copyright>

using System.Text;
using SpreadsheetEngine.ACell.ASpreadsheet;

namespace SpreadsheetEngine.AXMLFileProcess.Test.Save
{
    /// <summary>
    /// testing Save within XMLFileProccess.
    /// </summary>
    [TestFixture]
    internal class TestSave
    {
        /// <summary>
        /// instance of spreadsheet to test in normal case.
        /// </summary>
        private Spreadsheet testSpreadsheetNormal;

        /// <summary>
        /// string coming from just a spreadsheet.
        /// </summary>
        private string stringTestSpreadsheetNormal = "<root>\r\n  <spreadsheet>\r\n    <cell>\r\n      <row>0</row>\r\n      <column>0</column>\r\n      <text>5</text>\r\n      <bgcolor>4294944000</bgcolor>\r\n    </cell>\r\n    <cell>\r\n      <row>1</row>\r\n      <column>0</column>\r\n      <text>=A1+5</text>\r\n      <bgcolor>4294944000</bgcolor>\r\n    </cell>\r\n    <cell>\r\n      <row>1</row>\r\n      <column>1</column>\r\n      <text>Bell Cranel</text>\r\n      <bgcolor>4294967295</bgcolor>\r\n    </cell>\r\n    <cell>\r\n      <row>2</row>\r\n      <column>2</column>\r\n      <text></text>\r\n      <bgcolor>4294944000</bgcolor>\r\n    </cell>\r\n  </spreadsheet>\r\n</root>";

        /// <summary>
        /// instance of spreadsheet to test in edge case.
        /// </summary>
        private Spreadsheet testSpreadsheetEdge;

        /// <summary>
        /// string coming from just a spreadsheet.
        /// </summary>
        private string stringTestSpreadsheetEdge = "<root>\r\n  <spreadsheet />\r\n</root>";

        /// <summary>
        /// setup for the tests.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            // normal testing
            this.testSpreadsheetNormal = new Spreadsheet(5, 5);

            // both text and color changed
            this.testSpreadsheetNormal.GetCell(0, 0)!.Text = "5";
            this.testSpreadsheetNormal.GetCell(0, 0)!.BGColor = 0xFFFFA500;
            this.testSpreadsheetNormal.GetCell(1, 0)!.Text = "=A1+5";
            this.testSpreadsheetNormal.GetCell(1, 0)!.BGColor = 0xFFFFA500;

            // only text changed
            this.testSpreadsheetNormal.GetCell(1, 1)!.Text = "Bell Cranel";

            // only color changed
            this.testSpreadsheetNormal.GetCell(2, 2)!.BGColor = 0xFFFFA500;

            // edge testing
            this.testSpreadsheetEdge = new Spreadsheet(5, 5);
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

            switch (testType)
            {
                case 0:
                    XMLFileProcess.Save(memoryStream, this.testSpreadsheetNormal);
                    memoryStream.Capacity = (int)memoryStream.Length;
                    string stringXML = Encoding.ASCII.GetString(memoryStream.GetBuffer());
                    return stringXML == this.stringTestSpreadsheetNormal;
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
        public bool? TestEdgeCase(int testType)
        {
            MemoryStream memoryStream = new MemoryStream();

            switch (testType)
            {
                case 0:
                    XMLFileProcess.Save(memoryStream, this.testSpreadsheetEdge);
                    memoryStream.Capacity = (int)memoryStream.Length;
                    string stringXML = Encoding.ASCII.GetString(memoryStream.GetBuffer());
                    return stringXML == this.stringTestSpreadsheetEdge;
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
        [TestCase(0, ExpectedResult = typeof(ArgumentNullException))] // spreadsheet is null
        [TestCase(1, ExpectedResult = typeof(ArgumentNullException))] // stream is null
        public Type? TestExceptionCase(int testType)
        {
            MemoryStream memoryStream = new MemoryStream();

            // after creating instance once exception is thrown make sure specific exception for input is being thrown.
            try
            {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                switch (testType)
                {
                    case 0:
                        XMLFileProcess.Save(memoryStream, null);
                        break;
                    case 1:
                        XMLFileProcess.Save(null, this.testSpreadsheetEdge);
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