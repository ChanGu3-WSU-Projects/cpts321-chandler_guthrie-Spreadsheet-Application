// <copyright file="XMLFileProcess.cs" company="Chandler_Guthrie-WSU_ID:011801740">
// Copyright (c) Chandler_Guthrie-WSU_ID:011801740. All rights reserved.
// </copyright>

using System.Reflection;
using System.Text;
using System.Xml.Linq;
using SpreadsheetEngine.ACell;
using SpreadsheetEngine.ACell.ASpreadsheet;

namespace SpreadsheetEngine.AXMLFileProcess
{
    /// <summary>
    /// Save/Load Functionality for the application.
    /// </summary>
    public static class XMLFileProcess
    {
        /// <summary>
        /// loads all info from a XML file.
        /// </summary>
        /// <param name="stream"> stream to read from. </param>
        /// <param name="spreadsheet"> currently loaded spreadsheet. </param>
        public static void Load(Stream stream, ref Spreadsheet spreadsheet)
        {
            if (stream is null)
            {
                throw new ArgumentNullException("stream cannot be null");
            }

            XDocument documentXML;

            try
            {
                documentXML = XDocument.Load(stream);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("[Error] Loading Stream Into XDocument Failed.\nReason: " + ex.GetBaseException().Message);
            }

            XElement? xNameSpreadsheet = documentXML.Element(XName.Get("root"))?.Element(XName.Get("spreadsheet"));
            if (xNameSpreadsheet is not null)
            {
                LoadSpreadsheet(xNameSpreadsheet, ref spreadsheet);
            }
        }

        /// <summary>
        /// saves all application info into a XDocument.
        /// </summary>
        /// <param name="stream"> stream to write to. </param>
        /// <param name="spreadsheet"> currently loaded spreadsheet. </param>
        public static void Save(Stream stream, Spreadsheet spreadsheet)
        {
            if (stream is null)
            {
                throw new ArgumentNullException("stream cannot be null");
            }

            XElement spreadsheetXML = SaveSpreadsheet(spreadsheet);

            XDocument documentXML = new XDocument(
                new XElement(
                    "root",
                    spreadsheetXML));

            byte[] bytesDocumentXML = Encoding.ASCII.GetBytes(documentXML.ToString());

            stream.Write(bytesDocumentXML, 0, bytesDocumentXML.Length);
            stream.Flush();
        }

        /// <summary>
        /// loads the spreadsheet data specifically.
        /// </summary>
        /// <param name="elementSpreadsheet"> the spreadsheet element. </param>
        /// <param name="spreadsheet"> currently loaded spreadsheet. </param>
        private static void LoadSpreadsheet(XElement elementSpreadsheet, ref Spreadsheet spreadsheet)
        {
            if (spreadsheet is null)
            {
                throw new ArgumentNullException("Spreadsheet cannot be null");
            }

            // using reflection to get field info
            FieldInfo? spreadsheetCellsInfo = typeof(Spreadsheet).GetField("spreadsheetCells", BindingFlags.NonPublic | BindingFlags.Instance);

            if (spreadsheetCellsInfo is not null)
            {
                // pulled the cells from spreadsheet through this info about the field
                List<List<Cell>>? spreadsheetCells = spreadsheetCellsInfo.GetValue(spreadsheet) as List<List<Cell>>;

                if (spreadsheetCells is not null)
                {
                    List<XElement> elementCells = elementSpreadsheet.Elements().Where((spreadsheetElement) => spreadsheetElement.Name == XName.Get("cell")).ToList();
                    foreach (XElement elementCell in elementCells)
                    {
                        int rowIndex = -1;
                        int columnIndex = -1;
                        // the try catch is just to ignore any problem that occurs and reset the values of the cell back to normal since there was a problem in the XML formatting.
                        try
                        {
                            rowIndex = Convert.ToInt32(elementCell.Elements().First((cellElement) => cellElement.Name == XName.Get("row")).Value);
                            columnIndex = Convert.ToInt32(elementCell.Elements().First((cellElement) => cellElement.Name == XName.Get("column")).Value);

                            string text;
                            if ((text = elementCell.Elements().First((cellElement) => cellElement.Name == XName.Get("text")).Value).Trim() != string.Empty)
                            {
                                spreadsheetCells[rowIndex][columnIndex].Text = text;
                            }

                            string bgcolor;
                            if ((bgcolor = elementCell.Elements().First((cellElement) => cellElement.Name == XName.Get("bgcolor")).Value).Trim() != string.Empty)
                            {
                                spreadsheetCells[rowIndex][columnIndex].BGColor = Convert.ToUInt32(bgcolor);
                            }
                        }
                        catch (Exception)
                        {
                            if (rowIndex != -1 && columnIndex != -1)
                            {
                                spreadsheetCells[rowIndex][columnIndex].Text = string.Empty;
                                spreadsheetCells[rowIndex][columnIndex].BGColor = 0xFFFFFFFF;
                            }
                        }
                    }
                }
                else
                {
                    throw new ArgumentNullException("Error: Could Not GetValue of spreadsheetCells from instance of spreadsheet");
                }
            }
            else
            {
                throw new ArgumentNullException("Error: Could Not Retrieve FieldInfo for spreadsheetCells from Spreadsheet");
            }
        }

        /// <summary>
        /// saves the spreadsheet data specifically.
        /// </summary>
        /// <param name="spreadsheet"> currently loaded spreadsheet. </param>
        /// <returns> returns the XML element for the spreadsheet. </returns>
        private static XElement SaveSpreadsheet(Spreadsheet spreadsheet)
        {
            if (spreadsheet is null)
            {
                throw new ArgumentNullException("Spreadsheet cannot be null");
            }

            // using reflection to get field info
            FieldInfo? spreadsheetCellsInfo = typeof(Spreadsheet).GetField("spreadsheetCells", BindingFlags.NonPublic | BindingFlags.Instance);

            if (spreadsheetCellsInfo is not null)
            {
                // pulled the cells from spreadsheet through this info about the field
                List<List<Cell>>? spreadsheetCells = spreadsheetCellsInfo.GetValue(spreadsheet) as List<List<Cell>>;

                if (spreadsheetCells is not null)
                {
                    // retrieve only the cell that have been modified
                    List<Cell> modifiedCells = new List<Cell>();
                    foreach (List<Cell> row in spreadsheetCells)
                    {
                        modifiedCells.AddRange(row.Where((cell) => (cell.Text != string.Empty || cell.BGColor != 0xFFFFFFFF)).ToList());
                    }

                    // Create list of elements made from modified cells
                    List<XElement> modifiedCellElements = new List<XElement>();
                    foreach (Cell modifiedCell in modifiedCells)
                    {
                        modifiedCellElements.Add(
                            new XElement(
                                "cell",
                                new XElement("row", modifiedCell.RowIndex),
                                new XElement("column", modifiedCell.ColumnIndex),
                                new XElement("text", modifiedCell.Text),
                                new XElement("bgcolor", modifiedCell.BGColor)));
                    }

                    // root element for the spreadsheet data
                    return new XElement(
                        "spreadsheet",
                        from element in modifiedCellElements select element);
                }
                else
                {
                    throw new ArgumentNullException("Error: Could Not GetValue of spreadsheetCells from instance of spreadsheet");
                }
            }
            else
            {
                throw new ArgumentNullException("Error: Could Not Retrieve FieldInfo for spreadsheetCells from Spreadsheet");
            }
        }
    }
}
