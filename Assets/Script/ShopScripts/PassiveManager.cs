using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveManager : MonoBehaviour
{
    public static PassiveManager Instance;
    public List<PassiveAbility> abilities;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public float GetBonus(PassiveTypes type)
    {
        var ability = abilities.Find(a => a.type == type);
        return ability != null ? ability.GetTotalValue() : 0f;
    }

    public void ApplyPassivesAtRoundStart()
    {
        ApplyExtraTime();
        ApplyIngredientSlow();
    }

    private void ApplyExtraTime()
    {
        //���� �ð� ���� �Լ�
    }

    private void ApplyIngredientSlow()
    {
        //������� ���ĸ��� ��ȭ
    }

   //public int ApplyScoreBonus(int baseScore)
   // {
        //����� ���ھ� ���� �Լ�
   // }

   // public int ApplyGoldBonus(int score)
   // {
        //����� ��� ���� �Լ�
   // }

  //  public bool ShouldSpawnHigherGradeMaterial()
   // {
        //�⺻ ���� ��� ���� �Լ�
    //}
}

