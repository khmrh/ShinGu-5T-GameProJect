using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopSlot : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI priceText;

    private PassiveAbility ability;

    public void SetUp(PassiveAbility ability)
    {
        this.ability = ability;

        nameText.text = ability.name;
        descriptionText.text = $"{ability.description}\n▶ 누적 효과: {ability.GetTotalValue()}";
        priceText.text = "가격: 0";

        // 슬롯 전체에 붙은 Button 컴포넌트에 클릭 이벤트 연결
        var btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(OnClickPurchase);
        }
        else
        {
            Debug.LogWarning("ShopSlot에 Button 컴포넌트가 없습니다!");
        }
    }

    public void OnClickPurchase()
    {
        PassiveManager.Instance.Purchase(ability);
        SetUp(ability); // UI 갱신
    }
}
