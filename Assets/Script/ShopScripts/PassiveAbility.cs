using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PassiveAbility
{
    public PassiveTypes type;
    public string name;
    public string description;
    public Sprite icon;

    public float valuePerPurchase;
    public int basePrice;
    public int currentPrice;
    public int timesPurchased;

    public void Purchase()
    {
        timesPurchased++;
        currentPrice = Mathf.RoundToInt(basePrice * Mathf.Pow(1.5f, timesPurchased));
    }

    public float GetTotalValue()
    {
        return valuePerPurchase * timesPurchased;
    }
}

