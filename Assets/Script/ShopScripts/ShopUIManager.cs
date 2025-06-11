using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUIManager : MonoBehaviour
{
    public GameObject slotPrefab;      // ���� ������
    public Transform slotParent;       // ���Ե��� �� �θ� ������Ʈ (ex: SlotGrid)

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

    //  ���ΰ�ħ ��ư�� ȣ���� �Լ�
    public void RefreshShop()
    {
        GenerateShop();
    }
}
