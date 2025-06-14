using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// �нú� �ɷ��� �߾ӿ��� �����ϰ� �����ϴ� �Ŵ���
/// </summary>
public class PassiveManager : MonoBehaviour
{
    public static PassiveManager Instance;
    public List<PassiveAbility> abilities;  // ���� ��ϵ� �нú� ����Ʈ

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    /// <summary>
    /// Ư�� �нú� Ÿ���� ���� ȿ�� ��ȯ (valuePerPurchase * timesPurchased)
    /// </summary>
    public float GetBonus(PassiveType type)
    {
        var ability = abilities.Find(a => a.type == type);
        return ability != null ? ability.GetTotalValue() : 0f;
    }

    /// <summary>
    /// Ư�� �нú� Ÿ���� ���� Ƚ�� ��ȯ
    /// </summary>
    private int GetTimesPurchased(PassiveType type)
    {
        var ability = abilities.Find(a => a.type == type);
        return ability != null ? ability.timesPurchased : 0;
    }

    /// <summary>
    /// ���尡 ���۵� �� ����Ǵ� ��� �нú� ȿ����
    /// </summary>
    public void ApplyPassivesAtRoundStart()
    {
        ApplyExtraTime();      // ���� �ð� ���� �нú�
        ApplyIngredientSlow(); // ��� �ӵ� ��ȭ �нú�
    }

    /// <summary>
    /// ���� �ð� ���� �нú� ȿ�� ����
    /// </summary>
    private void ApplyExtraTime()
    {
        float extraTime = GetBonus(PassiveType.AddTime);
        GameTimerManager.remainingTime += extraTime;
        Debug.Log($"[�нú�] ���� �ð� +{extraTime}�� �����");
    }

    /// <summary>
    /// ��� �̵� �ӵ� ��ȭ �нú� ȿ�� ����
    /// �⺻ 1.3�� ���� ���¿���, ���� Ƚ����ŭ 10%�� ���������� ����
    /// </summary>
    private void ApplyIngredientSlow()
    {
        int level = GetTimesPurchased(PassiveType.SlowIngredient);
        float slowMultiplier = Mathf.Pow(0.9f, level);      // 10%�� ���������� ����
        float baseSpeedMultiplier = 1.3f;                   // �⺻������ 1.3�� ������ ����

        foreach (var pepper in FindObjectsOfType<PepperMovement>())
        {
            pepper.SetBaseSpeed(baseSpeedMultiplier);       // �⺻ ��� ����
            pepper.ApplySlowMultiplier(slowMultiplier);     // ��ȭ ����
        }

        Debug.Log($"[�нú�] ��� �ӵ� = 1.3 x {slowMultiplier:F2} = {(1.3f * slowMultiplier):F2}��");
    }

    /// <summary>
    /// ���� ȹ�淮 ���� (���Ŵ� 8%�� ������)
    /// </summary>
    public int ApplyScoreBonus(int baseScore)
    {
        int count = GetTimesPurchased(PassiveType.ScoreBonus);
        float multiplier = Mathf.Pow(1.08f, count);
        return Mathf.RoundToInt(baseScore * multiplier);
    }

    /// <summary>
    /// ��� ȹ�淮 ���� (���Ŵ� 10%�� ������)
    /// </summary>
    public int ApplyGoldBonus(int baseCoin)
    {
        int count = GetTimesPurchased(PassiveType.GoldBonus);
        float multiplier = Mathf.Pow(1.10f, count);
        return Mathf.RoundToInt(baseCoin * multiplier);
    }

    /// <summary>
    /// ���� ��� ���� Ȯ�� ���
    /// ������ ���� �پ��� ��� ���� Ȯ���� ��ȯ
    /// </summary>
    public Dictionary<int, float> GetSpawnProbabilities()
    {
        int level = GetTimesPurchased(PassiveType.BetterMaterial);
        Dictionary<int, float> spawnChances = new();

        // �̹��� ���� ���� Ȯ��ǥ �ݿ�
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
    /// �нú� �ɷ� ���� ó��
    /// </summary>
    public void Purchase(PassiveAbility ability)
    {
        ability.Purchase();
        Debug.Log($"[����] {ability.name} ���� �Ϸ� �� �� {ability.timesPurchased}ȸ");
    }

    /// <summary>
    /// BetterMaterial �нú꿡 ���� ����� ���� ������ �����ϴ� �Լ�
    /// Ȯ���� PassiveManager.GetSpawnProbabilities()���� �����´�.
    /// Ȯ���� �����Ǿ� ���� ���� ������ ����� ���� �����ȴ�.
    /// ��: {2: 0.1, 3: 0.05}�� ��, �������� 0.07�̸� �� 2�ܰ� ����
    /// </summary>
    public int DetermineSpawnLevel()
    {
        // �нú�κ��� ���� Ȯ�� ��ųʸ� ��������
        var spawnChances = PassiveManager.Instance.GetSpawnProbabilities();

        // 0~1 ������ ������ ���� (��íó�� ����)
        float roll = Random.value;
        float cumulative = 0f;

        // ���� ��޺��� Ȯ���� �����ذ��� �˻�
        foreach (var kvp in spawnChances.OrderBy(k => k.Key))
        {
            cumulative += kvp.Value;

            // ���� Ȯ���� �ѱ�� �ش� ��� ����
            if (roll < cumulative)
                return kvp.Key;
        }

        // Ȯ�� ������ �ش����� ������ �⺻ ���� 1 ��� ����
        return 1;
    }

}
