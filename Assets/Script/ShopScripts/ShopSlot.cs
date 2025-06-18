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
        descriptionText.text = $"{ability.description}\n�� ���� ȿ��: {ability.GetTotalValue()}";

        int price = ability.GetCurrentPrice();
        priceText.text = ability.IsMaxed() ? "�ִ�ġ" : $"����: {price}";

        Button btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.RemoveAllListeners();

            if (ability.IsMaxed() || price > PlayerData.Instance.currentGold)
            {
                btn.interactable = false;
            }
            else
            {
                btn.interactable = true;
                btn.onClick.AddListener(OnClickPurchase);
            }
        }
    }


    public void OnClickPurchase()
    {
        PassiveManager.Instance.Purchase(ability);
        SetUp(ability); // UI ����
    }
}
