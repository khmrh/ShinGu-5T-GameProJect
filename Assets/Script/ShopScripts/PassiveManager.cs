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
        //제한 시간 증가 함수
    }

    private void ApplyIngredientSlow()
    {
        //날라댕기는 날파리들 둔화
    }

   //public int ApplyScoreBonus(int baseScore)
   // {
        //종료시 스코어 증가 함수
   // }

   // public int ApplyGoldBonus(int score)
   // {
        //종료시 골드 증가 함수
   // }

  //  public bool ShouldSpawnHigherGradeMaterial()
   // {
        //기본 생성 등급 증가 함수
    //}
}

