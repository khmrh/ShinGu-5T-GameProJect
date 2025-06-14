using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUIManager : MonoBehaviour
{
    public GameObject slotPrefab;      // 슬롯 프리팹
    public Transform slotParent;       // 슬롯들이 들어갈 부모 오브젝트 (ex: SlotGrid)

    void Start()
    {
        GenerateShop();
    }

    // 슬롯 생성 (초기 4개 랜덤)
    void GenerateShop()
    {
        foreach (Transform child in slotParent)
            Destroy(child.gameObject); // 기존 슬롯 제거

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

    //  새로고침 버튼이 호출할 함수
    public void RefreshShop()
    {
        GenerateShop();
    }
}
