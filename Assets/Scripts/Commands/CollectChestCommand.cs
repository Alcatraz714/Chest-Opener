using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectChestCommand : ICommand
{
    private ChestSlot chestSlot;
    private ChestData chestData;

    public CollectChestCommand(ChestSlot slot)
    {
        chestSlot = slot;
        chestData = slot.chestData;
    }

    public void Execute()
    {
        chestSlot.CollectChest();
        UIManager.Instance.ShowPopup("You received: " + string.Join(", ", chestData.rewards));
    }

    public void Undo() => chestSlot.RevertCollect(chestData);
}

