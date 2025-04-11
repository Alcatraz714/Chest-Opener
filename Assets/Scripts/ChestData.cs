using UnityEngine;

[CreateAssetMenu(fileName = "ChestData", menuName = "Chest/New Chest")]
public class ChestData : ScriptableObject
{
    public string chestName;
    public float unlockTime; // in seconds
    public string[] rewards;
}
