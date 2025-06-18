using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUIManager : MonoBehaviour
{
    public GameObject slotPrefab;      // 슬롯 프리팹
    public Transform slotParent;       // 슬롯들이 들어갈 부모 오브젝트 (ex: SlotGrid)

    [Header("새로고침 비용 설정")]
    public int refreshBaseCost = 200;
    private int refreshCount = 0;
    private float refreshCostMultiplier = 1.4f;

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

    // 새로고침 버튼 클릭 시 호출되는 함수
    public void RefreshShop()
    {
        int cost = Mathf.RoundToInt(refreshBaseCost * Mathf.Pow(refreshCostMultiplier, refreshCount));

        if (!PlayerData.Instance.SpendGold(cost))
        {
            Debug.LogWarning($"[상점] 골드 부족! 새로고침 비용: {cost}, 보유: {PlayerData.Instance.currentGold}");
            return;
        }

        refreshCount++;
        GenerateShop();
        Debug.Log($"[상점] 새로고침 성공! {cost} 골드 차감됨 (총 {refreshCount}회)");
    }
}
