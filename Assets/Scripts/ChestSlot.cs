using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System;

public enum ChestState { Empty, Locked, Unlocking, ReadyToCollect }

public class ChestSlot : MonoBehaviour
{
    public ChestData chestData;
    public ChestState state = ChestState.Empty;
    public float unlockTimer;
    public float currentTimeLeft;
    public bool HasChest => state != ChestState.Empty;

    [Header("UI References")]
    public TextMeshProUGUI timerText; // ⏱ Text UI to show time left

    private void Update()
    {
        if (state == ChestState.Unlocking)
        {
            currentTimeLeft -= Time.deltaTime;

            // Update the timer text while unlocking
            if (timerText != null)
            {
                timerText.gameObject.SetActive(true);
                timerText.text = FormatTime(currentTimeLeft);
            }

            if (currentTimeLeft <= 0f)
            {
                SetToCollectable();
            }
        }
        else if (state == ChestState.ReadyToCollect)
        {
            if (timerText != null)
            {
                timerText.text = "Tap to Collect";
                timerText.gameObject.SetActive(true);
            }
        }
        else
        {
            if (timerText != null)
                timerText.gameObject.SetActive(false);
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
        currentTimeLeft = 0f;

        if (timerText != null)
        {
            timerText.text = "Tap to Collect";
            timerText.gameObject.SetActive(true);
        }
    }

    public void CollectChest()
    {
        if (state != ChestState.ReadyToCollect) return;

        state = ChestState.Empty;
        chestData = null;
        if (timerText != null)
            timerText.gameObject.SetActive(false);

        foreach (Transform child in transform)
            Destroy(child.gameObject);
    }

    public void RevertCollect(ChestData previousData)
    {
        chestData = previousData;
        state = ChestState.ReadyToCollect;

        if (timerText != null)
        {
            timerText.text = "Tap to Collect";
            timerText.gameObject.SetActive(true);
        }
    }

    public void SetChest(ChestSlot chestPrefabSlot)
    {
        this.chestData = chestPrefabSlot.chestData;
        this.unlockTimer = chestData.unlockTime;
        this.state = ChestState.Locked;
        if (timerText != null)
            timerText.gameObject.SetActive(false);
    }

    public void SetToCollectable()
    {
        state = ChestState.ReadyToCollect;
        currentTimeLeft = 0f;
        if (timerText != null)
        {
            timerText.text = "Tap to Collect";
            timerText.gameObject.SetActive(true);
        }
    }

    public void ResetToLocked()
    {
        state = ChestState.Locked;
        currentTimeLeft = unlockTimer;
        if (timerText != null)
            timerText.gameObject.SetActive(false);
    }

    public int GetGemCost() => Mathf.CeilToInt(currentTimeLeft / 600f); // 600s = 10 mins

    private string FormatTime(float timeInSeconds)
    {
        timeInSeconds = Mathf.Max(timeInSeconds, 0f);
        TimeSpan time = TimeSpan.FromSeconds(timeInSeconds);
        return $"{time.Hours:D2}:{time.Minutes:D2}:{time.Seconds:D2}";
    }
}
