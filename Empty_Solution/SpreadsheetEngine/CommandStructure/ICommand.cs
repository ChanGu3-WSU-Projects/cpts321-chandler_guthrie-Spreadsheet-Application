// <copyright file="ICommand.cs" company="Chandler_Guthrie-WSU_ID:011801740">
// Copyright (c) Chandler_Guthrie-WSU_ID:011801740. All rights reserved.
// </copyright>

namespace SpreadsheetEngine.CommandStructure
{
    /// <summary>
    /// interface command for all concretecommand types.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Gets the name of the command.
        /// </summary>
        public string CommandName
        {
            get;
        }

        /// <summary>
        /// excutes specific command from reciever.
        /// </summary>
        public void ExecuteAction();

        /// <summary>
        /// undo's command that was executed.
        /// </summary>
        public void UndoAction();
    }
}
