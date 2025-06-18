using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUIManager : MonoBehaviour
{
    public GameObject slotPrefab;      // ���� ������
    public Transform slotParent;       // ���Ե��� �� �θ� ������Ʈ (ex: SlotGrid)

    [Header("���ΰ�ħ ��� ����")]
    public int refreshBaseCost = 200;
    private int refreshCount = 0;
    private float refreshCostMultiplier = 1.4f;

    void Start()
    {
        GenerateShop();
    }

    // ���� ���� (�ʱ� 4�� ����)
    void GenerateShop()
    {
        foreach (Transform child in slotParent)
            Destroy(child.gameObject); // ���� ���� ����

        List<PassiveAbility> all = PassiveManager.Instance.abilities;
        List<PassiveAbility> random = new List<PassiveAbility>(all);

        for (int i = 0; i < 4 && random.Count > 0; i++)
        {
            int rand = Random.Range(0, random.Count);
            PassiveAbility chosen = random[rand];

            GameObject slot = Instantiate(slotPrefab, slotParent);
            slot.GetComponent<ShopSlot>().SetUp(chosen);

            random.RemoveAt(rand);
        }
    }

    // ���ΰ�ħ ��ư Ŭ�� �� ȣ��Ǵ� �Լ�
    public void RefreshShop()
    {
        int cost = Mathf.RoundToInt(refreshBaseCost * Mathf.Pow(refreshCostMultiplier, refreshCount));

        if (!PlayerData.Instance.SpendGold(cost))
        {
            Debug.LogWarning($"[����] ��� ����! ���ΰ�ħ ���: {cost}, ����: {PlayerData.Instance.currentGold}");
            return;
        }

        refreshCount++;
        GenerateShop();
        Debug.Log($"[����] ���ΰ�ħ ����! {cost} ��� ������ (�� {refreshCount}ȸ)");
    }
}
