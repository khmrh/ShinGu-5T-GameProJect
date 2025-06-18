using UnityEngine;

[System.Serializable]
public class PassiveAbility
{
    public PassiveType type;
    public string name;
    public string description;
    public Sprite icon;

    public float valuePerPurchase = 1f;

    public int basePrice = 100;
    public float priceMultiplier = 1.5f;

    public int timesPurchased = 0;
    public int maxPurchaseCount = 5;

    public int GetCurrentPrice()
    {
        return Mathf.RoundToInt(basePrice * Mathf.Pow(priceMultiplier, timesPurchased));
    }

    public void Purchase()
    {
        if (!IsMaxed())
        {
            timesPurchased++;
        }
    }

    public bool IsMaxed()
    {
        return timesPurchased >= maxPurchaseCount;
    }

    public float GetTotalValue()
    {
        return valuePerPurchase * timesPurchased;
    }
}
