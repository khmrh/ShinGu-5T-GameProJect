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

    public float GetBonus(PassiveType type)
    {
        var ability = abilities.Find(a => a.type == type);
        return ability != null ? ability.GetTotalValue() : 0f;
    }

    /// <summary>
    /// 라운드 시작 시 적용되는 능력들
    /// </summary>
    public void ApplyPassivesAtRoundStart()
    {
        ApplyExtraTime();
        ApplyIngredientSlow();
    }

    /// <summary>
    /// 능력 1. 제한 시간 증가
    /// </summary>
    private void ApplyExtraTime()
    {
        // TODO: 제한 시간 증가 적용
    }

    /// <summary>
    /// 능력 2. 재료 이동 속도 둔화
    /// </summary>
    private void ApplyIngredientSlow()
    {
        // TODO: 재료 이동 속도 감소 적용
    }

    /// <summary>
    /// 능력 3. 점수 획득량 증가
    /// </summary>
    public int ApplyScoreBonus(int baseScore)
    {
        // TODO: 점수 증가 적용
        return baseScore;
    }

    /// <summary>
    /// 능력 4. 골드 획득량 증가
    /// </summary>
    public int ApplyGoldBonus(int score)
    {
        // TODO: 골드 증가 적용
        return Mathf.RoundToInt(score * 0.1f); // 기본 환산 비율
    }

    /// <summary>
    /// 능력 5. 고등급 재료 등장 확률 증가
    /// </summary>
    public bool ShouldSpawnHigherGradeMaterial()
    {
        // TODO: 고등급 재료 확률 계산
        return false;
    }

    /// <summary>
    /// 능력 구매 시 처리
    /// </summary>
    public void Purchase(PassiveAbility ability)
    {
        ability.Purchase();
        Debug.Log($"[상점] {ability.name} 구매 완료 → 총 {ability.timesPurchased}회");
    }
}
