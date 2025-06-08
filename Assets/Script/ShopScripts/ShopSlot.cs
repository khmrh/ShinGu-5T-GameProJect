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
    public Button purchaseButton;

    private PassiveAbility ability;

    public void SetUp(PassiveAbility ability)
    {
        this.ability = ability;

        nameText.text = ability.name;
        descriptionText.text = $"{ability.description}\n▶ 누적 효과: {ability.GetTotalValue()}";
        priceText.text = "가격: 0";

        purchaseButton.onClick.RemoveAllListeners();
        purchaseButton.onClick.AddListener(OnClickPurchase);
    }

    public void OnClickPurchase()
    {
        PassiveManager.Instance.Purchase(ability);
        SetUp(ability);
    }
}
