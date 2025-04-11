using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum ChestState { Empty, Locked, Unlocking, ReadyToCollect }

public class ChestSlot : MonoBehaviour
{
    public ChestData chestData;
    public ChestState state = ChestState.Empty;
    public float unlockTimer;
    public float currentTimeLeft;
    public bool HasChest => state != ChestState.Empty;

    private void Update()
    {
        if (state == ChestState.Unlocking)
        {
            currentTimeLeft -= Time.deltaTime;
            if (currentTimeLeft <= 0f)
                SetToCollectable();
        }
    }

    public void OnChestClicked()
    {
        if (state == ChestState.Locked)
            UIManager.Instance.ShowChestOptions(this);
        else if (state == ChestState.ReadyToCollect)
            ChestSystemManager.Instance.ExecuteCommand(new CollectChestCommand(this));
    }

    public void StartUnlocking()
    {
        if (ChestSystemManager.Instance.chestSlots.Exists(c => c.state == ChestState.Unlocking))
        {
            UIManager.Instance.ShowPopup("Only one chest can unlock at a time!");
            return;
        }

        state = ChestState.Unlocking;
        currentTimeLeft = unlockTimer;
    }

    public void UnlockWithGems()
    {
        state = ChestState.ReadyToCollect;
    }

    public void CollectChest()
    {
        state = ChestState.Empty;
        chestData = null;
        foreach (Transform child in transform)
            Destroy(child.gameObject);
    }

    public void RevertCollect(ChestData previousData)
    {
        chestData = previousData;
        state = ChestState.ReadyToCollect;
    }

    public void SetChest(ChestSlot chestPrefabSlot)
    {
        this.chestData = chestPrefabSlot.chestData;
        this.unlockTimer = chestData.unlockTime;
        this.state = ChestState.Locked;
    }

    public void SetToCollectable() => state = ChestState.ReadyToCollect;

    public void ResetToLocked()
    {
        state = ChestState.Locked;
        currentTimeLeft = unlockTimer;
    }

    public int GetGemCost() => Mathf.CeilToInt(currentTimeLeft / 600f); // 600s = 10 mins
}

