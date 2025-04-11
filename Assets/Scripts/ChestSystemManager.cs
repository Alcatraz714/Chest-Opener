using UnityEngine;
using System.Collections.Generic;

public class ChestSystemManager : MonoBehaviour
{
    public static ChestSystemManager Instance;
    public List<ChestSlot> chestSlots = new();
    public GameObject[] chestPrefabs;

    private Stack<ICommand> commandHistory = new();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ExecuteCommand(ICommand command)
    {
        command.Execute();
        commandHistory.Push(command);
    }

    public void UndoLastCommand()
    {
        ICommand undo = new UndoCommand(commandHistory);
        undo.Execute();
    }

    public void TrySpawnChest()
    {
        foreach (var slot in chestSlots)
        {
            if (!slot.HasChest)
            {
                int index = Random.Range(0, chestPrefabs.Length);
                GameObject chestObj = Instantiate(chestPrefabs[index], slot.transform);
                slot.SetChest(chestObj.GetComponent<ChestSlot>());
                return;
            }
        }
        UIManager.Instance.ShowPopup("All slots are full!");
    }
}
