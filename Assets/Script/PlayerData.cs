using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance;

    public int currentGold = 1000;

    private void Awake()
    {
        Instance = this;
    }

    public bool SpendGold(int amount)
    {
        if (currentGold >= amount)
        {
            currentGold -= amount;
            return true;
        }
        return false;
    }

    public void AddGold(int amount)
    {
        currentGold += amount;
    }
}
