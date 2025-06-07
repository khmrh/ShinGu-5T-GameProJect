using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Diagnostics;

[DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public class ShopSlot : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI priceText;
    public Image icon;
    private PassiveAbility ability;

    public void SetUp(PassiveAbility ability)
    {
        this.ability = ability;
        nameText.text = ability.name;
        descriptionText.text = $"{ability.description}\n�� ���� ȿ��: {ability.GetTotalValue()}";
        priceText.text = "����: 0"; // �ӽ� ����
        icon.sprite = ability.icon;
    }

    public void OnClickPurchase()
    {
        PassiveManager.Instance.Purchase(ability);
        SetUp(ability); // UI ����
    }

    private string GetDebuggerDisplay()
    {
        return ToString();
    }
}
