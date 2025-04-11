public class StartTimerCommand : ICommand
{
    private ChestSlot chestSlot;

    public StartTimerCommand(ChestSlot slot) => chestSlot = slot;

    public void Execute() => chestSlot.StartUnlocking();

    public void Undo() => chestSlot.ResetToLocked();
}
