using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 패시브 능력을 중앙에서 관리하고 적용하는 매니저
/// </summary>
public class PassiveManager : MonoBehaviour
{
    public static PassiveManager Instance;
    public List<PassiveAbility> abilities;  // 현재 등록된 패시브 리스트

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    /// <summary>
    /// 특정 패시브 타입의 누적 효과 반환 (valuePerPurchase * timesPurchased)
    /// </summary>
    public float GetBonus(PassiveType type)
    {
        var ability = abilities.Find(a => a.type == type);
        return ability != null ? ability.GetTotalValue() : 0f;
    }

    /// <summary>
    /// 특정 패시브 타입의 구매 횟수 반환
    /// </summary>
    private int GetTimesPurchased(PassiveType type)
    {
        var ability = abilities.Find(a => a.type == type);
        return ability != null ? ability.timesPurchased : 0;
    }

    /// <summary>
    /// 라운드가 시작될 때 적용되는 모든 패시브 효과들
    /// </summary>
    public void ApplyPassivesAtRoundStart()
    {
        ApplyExtraTime();      // 제한 시간 증가 패시브
        ApplyIngredientSlow(); // 재료 속도 둔화 패시브
    }

    /// <summary>
    /// 제한 시간 증가 패시브 효과 적용
    /// </summary>
    private void ApplyExtraTime()
    {
        float extraTime = GetBonus(PassiveType.AddTime);
        GameTimerManager.remainingTime += extraTime;
        Debug.Log($"[패시브] 제한 시간 +{extraTime}초 적용됨");
    }

    /// <summary>
    /// 재료 이동 속도 둔화 패시브 효과 적용
    /// 기본 1.3배 빠른 상태에서, 구매 횟수만큼 10%씩 곱연산으로 감소
    /// </summary>
    private void ApplyIngredientSlow()
    {
        int level = GetTimesPurchased(PassiveType.SlowIngredient);
        float slowMultiplier = Mathf.Pow(0.9f, level);      // 10%씩 곱연산으로 감소
        float baseSpeedMultiplier = 1.3f;                   // 기본적으로 1.3배 빠르게 시작

        foreach (var pepper in FindObjectsOfType<PepperMovement>())
        {
            pepper.SetBaseSpeed(baseSpeedMultiplier);       // 기본 배속 설정
            pepper.ApplySlowMultiplier(slowMultiplier);     // 둔화 적용
        }

        Debug.Log($"[패시브] 재료 속도 = 1.3 x {slowMultiplier:F2} = {(1.3f * slowMultiplier):F2}배");
    }

    /// <summary>
    /// 점수 획득량 증가 (구매당 8%씩 곱연산)
    /// </summary>
    public int ApplyScoreBonus(int baseScore)
    {
        int count = GetTimesPurchased(PassiveType.ScoreBonus);
        float multiplier = Mathf.Pow(1.08f, count);
        return Mathf.RoundToInt(baseScore * multiplier);
    }

    /// <summary>
    /// 골드 획득량 증가 (구매당 10%씩 곱연산)
    /// </summary>
    public int ApplyGoldBonus(int baseCoin)
    {
        int count = GetTimesPurchased(PassiveType.GoldBonus);
        float multiplier = Mathf.Pow(1.10f, count);
        return Mathf.RoundToInt(baseCoin * multiplier);
    }

    /// <summary>
    /// 고등급 재료 등장 확률 계산
    /// 레벨에 따라 다양한 등급 등장 확률을 반환
    /// </summary>
    public Dictionary<int, float> GetSpawnProbabilities()
    {
        int level = GetTimesPurchased(PassiveType.BetterMaterial);
        Dictionary<int, float> spawnChances = new();

        // 이미지 문서 기준 확률표 반영
        switch (level)
        {
            case 1:
                spawnChances[2] = 0.02f;
                break;
            case 2:
                spawnChances[2] = 0.04f;
                spawnChances[3] = 0.01f;
                break;
            case 3:
                spawnChances[2] = 0.06f;
                spawnChances[3] = 0.03f;
                spawnChances[4] = 0.01f;
                break;
            case 4:
                spawnChances[2] = 0.08f;
                spawnChances[3] = 0.05f;
                spawnChances[4] = 0.02f;
                spawnChances[5] = 0.01f;
                break;
            case 5:
                spawnChances[2] = 0.10f;
                spawnChances[3] = 0.07f;
                spawnChances[4] = 0.03f;
                spawnChances[5] = 0.015f;
                spawnChances[6] = 0.01f;
                break;
        }

        return spawnChances;
    }

    /// <summary>
    /// 패시브 능력 구매 처리
    /// </summary>
    public void Purchase(PassiveAbility ability)
    {
        ability.Purchase();
        Debug.Log($"[상점] {ability.name} 구매 완료 → 총 {ability.timesPurchased}회");
    }

    /// <summary>
    /// BetterMaterial 패시브에 따라 재료의 등장 레벨을 결정하는 함수
    /// 확률은 PassiveManager.GetSpawnProbabilities()에서 가져온다.
    /// 확률이 누적되어 가장 먼저 도달한 등급이 최종 결정된다.
    /// 예: {2: 0.1, 3: 0.05}일 때, 랜덤값이 0.07이면 → 2단계 생성
    /// </summary>
    public int DetermineSpawnLevel()
    {
        // 패시브로부터 등장 확률 딕셔너리 가져오기
        var spawnChances = PassiveManager.Instance.GetSpawnProbabilities();

        // 0~1 사이의 랜덤값 생성 (가챠처럼 동작)
        float roll = Random.value;
        float cumulative = 0f;

        // 낮은 등급부터 확률을 누적해가며 검사
        foreach (var kvp in spawnChances.OrderBy(k => k.Key))
        {
            cumulative += kvp.Value;

            // 누적 확률을 넘기면 해당 등급 리턴
            if (roll < cumulative)
                return kvp.Key;
        }

        // 확률 범위에 해당하지 않으면 기본 레벨 1 재료 생성
        return 1;
    }

}
