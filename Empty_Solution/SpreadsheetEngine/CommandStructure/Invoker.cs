// <copyright file="Invoker.cs" company="Chandler_Guthrie-WSU_ID:011801740">
// Copyright (c) Chandler_Guthrie-WSU_ID:011801740. All rights reserved.
// </copyright>

namespace SpreadsheetEngine.CommandStructure
{
    /// <summary>
    /// invokes actions of concreate commands inherited from ICommand.
    /// </summary>
    public class Invoker
    {
        /// <summary>
        /// the current command of in the invoker.
        /// </summary>
        private ICommand? currentCommand = null;

        /// <summary>
        /// undo stack contains any commands that were undone.
        /// </summary>
        private Stack<ICommand> undoStack = new Stack<ICommand>();

        /// <summary>
        /// redo stack allows command to be redone if it was the last undone.
        /// </summary>
        private Stack<ICommand> redoStack = new Stack<ICommand>();

        /// <summary>
        /// broadcasts to listeners whenever the undo stack is changed.
        /// </summary>
        public event EventHandler? UndoStackChanged;

        /// <summary>
        /// broadcasts to listeners whenver the redo stack is changed.
        /// </summary>
        public event EventHandler? RedoStackChanged;

        /// <summary>
        /// Gets the name of the command on top of the undoStack.
        /// </summary>
        public string? NameUndoTop
        {
            get
            {
                ICommand? topUndoCommand;
                if (this.undoStack.TryPeek(out topUndoCommand))
                {
                    return topUndoCommand.CommandName;
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the name of the command on top of the undoStack.
        /// </summary>
        public string? NameRedoTop
        {
            get
            {
                ICommand? topRedoCommand;
                if (this.redoStack.TryPeek(out topRedoCommand))
                {
                    return topRedoCommand.CommandName;
                }

                return null;
            }
        }

        /// <summary>
        /// set current command to be executed inside of invoker.
        /// </summary>
        /// <param name="newCommand"> command wanting to be invoked. </param>
        public void SetCommand(ICommand newCommand)
        {
            if (newCommand is null)
            {
                throw new ArgumentNullException("Command Cannot Be Null");
            }

            this.currentCommand = newCommand;
        }

        /// <summary>
        /// executes current commmand.
        /// </summary>
        public void ExecuteCommand()
        {
            if (this.currentCommand is not null)
            {
                this.redoStack.Clear();
                this.undoStack.Push(this.currentCommand);
                this.currentCommand?.ExecuteAction();
                this.RedoStackChanged?.Invoke(this, new EventArgs());
                this.UndoStackChanged?.Invoke(this, new EventArgs());
            }
            else
            {
                throw new NullReferenceException("Command Is Null");
            }
        }

        /// <summary>
        /// undos the most previous command.
        /// </summary>
        public void UndoCommand()
        {
            ICommand? undoCommand;
            if (this.undoStack.TryPop(out undoCommand))
            {
                this.redoStack.Push(undoCommand);
                undoCommand.UndoAction();
                this.RedoStackChanged?.Invoke(this, new EventArgs());
                this.UndoStackChanged?.Invoke(this, new EventArgs());
            }
        }

        /// <summary>
        /// redos the most previous undo command.
        /// </summary>
        public void RedoCommand()
        {
            ICommand? redoCommand;
            if (this.redoStack.TryPop(out redoCommand))
            {
                this.undoStack.Push(redoCommand);
                redoCommand.ExecuteAction();
                this.RedoStackChanged?.Invoke(this, new EventArgs());
                this.UndoStackChanged?.Invoke(this, new EventArgs());
            }
        }
    }
}
