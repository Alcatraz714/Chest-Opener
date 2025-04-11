using UnityEngine;

public class GemManager : MonoBehaviour
{
    public static GemManager Instance;
    public int gemCount = 20;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public bool UseGems(int cost)
    {
        if (gemCount >= cost)
        {
            gemCount -= cost;
            UIManager.Instance.UpdateGemUI(gemCount);
            return true;
        }
        return false;
    }

    public void AddGems(int amount)
    {
        gemCount += amount;
        UIManager.Instance.UpdateGemUI(gemCount);
    }
}
