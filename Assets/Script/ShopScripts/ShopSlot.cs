using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour
{
    public Text nameText;
    public Text descriptionText;
    public Text priceText;
    public Image icon;
    private PassiveAbility ability;

    public void SetUp(PassiveAbility ability)
    {
        this.ability = ability;
        nameText.text = ability.name;
        descriptionText.text = ability.description;
        priceText.text = ability.currentPrice.ToString();
        icon.sprite = ability.icon;
    }

    //public void OnClickPurchase()
 //   {
       // if (Player.Instance.gold >= ability.currentPrice)
   //     {
       //     Player.Instance.gold -= ability.currentPrice;
       //     ability.Purchase();
       //     priceText.text = ability.currentPrice.ToString();
     //   }
  //  }
}
 

