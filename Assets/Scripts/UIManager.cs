using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject chestPopup;
    public Button startTimerButton, unlockWithGemsButton;
    public TMP_Text popupText, gemText, unlockCostText;

    private ChestSlot currentSlot;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ShowPopup(string message)
    {
        popupText.text = message;
        popupText.gameObject.SetActive(true);
        CancelInvoke(nameof(HidePopup));
        Invoke(nameof(HidePopup), 2f);
    }

    void HidePopup() => popupText.gameObject.SetActive(false);

    public void ShowChestOptions(ChestSlot slot)
    {
        /*currentSlot = slot;
        chestPopup.SetActive(true);
        unlockWithGemsButton.GetComponentInChildren<Text>().text = $"Unlock with {slot.GetGemCost()} Gems";*/
        currentSlot = slot;
        chestPopup.SetActive(true);
        int cost = slot.GetGemCost();
        unlockCostText.text = $"Unlock for {cost} Gems?";
    }

    public void OnStartTimerClicked()
    {
        ChestSystemManager.Instance.ExecuteCommand(new StartTimerCommand(currentSlot));
        chestPopup.SetActive(false);
    }

    public void OnUnlockWithGemsClicked()
    {
        ChestSystemManager.Instance.ExecuteCommand(new UnlockWithGemsCommand(currentSlot));
        chestPopup.SetActive(false);
    }

    public void UpdateGemUI(int amount) => gemText.text = $"Gems: {amount}";
}
