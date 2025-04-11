using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndoCommand : ICommand
{
    private Stack<ICommand> commandHistory;

    public UndoCommand(Stack<ICommand> history) => commandHistory = history;

    public void Execute()
    {
        if (commandHistory.Count > 0)
        {
            var lastCommand = commandHistory.Pop();
            lastCommand.Undo();
        }
        else
            UIManager.Instance.ShowPopup("Nothing to undo!");
    }

    public void Undo() { }
}
