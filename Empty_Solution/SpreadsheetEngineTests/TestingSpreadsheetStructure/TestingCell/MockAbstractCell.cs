// <copyright file="MockAbstractCell.cs" company="Chandler_Guthrie-WSU_ID:011801740">
// Copyright (c) Chandler_Guthrie-WSU_ID:011801740. All rights reserved.
// </copyright>

using SpreadsheetEngine.ACell;

namespace SpreadsheetEngineTests.AMockAbstractCell
{
    /// <summary>
    /// inherits from abstract Cell without concrete implementation.
    ///     use: Testing the abstract class Cell methods/constructor.
    /// </summary>
    internal class MockAbstractCell : Cell
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MockAbstractCell"/> class.
        /// </summary>
        /// <param name="rowIndex"> row index of cell. </param>
        /// <param name="columnIndex"> column index of cell. </param>
        public MockAbstractCell(int rowIndex, int columnIndex)
            : base(rowIndex, columnIndex)
        {
        }
    }
}
