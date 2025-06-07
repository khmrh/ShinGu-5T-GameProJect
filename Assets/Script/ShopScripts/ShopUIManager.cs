using System.Collections.Generic;
using UnityEngine;

public class ShopUIManager : MonoBehaviour
{
    public GameObject slotPrefab;
    public Transform slotParent;

    void Start()
    {
        GenerateShop();
    }

    void GenerateShop()
    {
        foreach (Transform child in slotParent)
            Destroy(child.gameObject);

        List<PassiveAbility> all = PassiveManager.Instance.abilities;
        List<PassiveAbility> random = new List<PassiveAbility>(all);

        for (int i = 0; i < 4; i++)
        {
            int rand = Random.Range(0, random.Count);
            PassiveAbility chosen = random[rand];
            GameObject slot = Instantiate(slotPrefab, slotParent);
            slot.GetComponent<ShopSlot>().SetUp(chosen);
            random.RemoveAt(rand);
        }
    }
}
