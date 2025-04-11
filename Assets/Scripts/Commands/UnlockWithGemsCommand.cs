using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UnlockWithGemsCommand : ICommand
{
    private ChestSlot chestSlot;
    private int gemCost;

    public UnlockWithGemsCommand(ChestSlot slot)
    {
        chestSlot = slot;
        gemCost = slot.GetGemCost();
    }

    public void Execute()
    {
        if (GemManager.Instance.UseGems(gemCost))
            chestSlot.UnlockWithGems();
        else
            UIManager.Instance.ShowPopup("Not enough gems!");
    }

    public void Undo()
    {
        GemManager.Instance.AddGems(gemCost);
        chestSlot.ResetToLocked();
    }
}
