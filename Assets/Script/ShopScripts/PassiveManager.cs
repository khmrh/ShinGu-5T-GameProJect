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
    /// ���� ���� �� ����Ǵ� �ɷµ�
    /// </summary>
    public void ApplyPassivesAtRoundStart()
    {
        ApplyExtraTime();
        ApplyIngredientSlow();
    }

    /// <summary>
    /// �ɷ� 1. ���� �ð� ����
    /// </summary>
    private void ApplyExtraTime()
    {
        // TODO: ���� �ð� ���� ����
    }

    /// <summary>
    /// �ɷ� 2. ��� �̵� �ӵ� ��ȭ
    /// </summary>
    private void ApplyIngredientSlow()
    {
        // TODO: ��� �̵� �ӵ� ���� ����
    }

    /// <summary>
    /// �ɷ� 3. ���� ȹ�淮 ����
    /// </summary>
    public int ApplyScoreBonus(int baseScore)
    {
        // TODO: ���� ���� ����
        return baseScore;
    }

    /// <summary>
    /// �ɷ� 4. ��� ȹ�淮 ����
    /// </summary>
    public int ApplyGoldBonus(int score)
    {
        // TODO: ��� ���� ����
        return Mathf.RoundToInt(score * 0.1f); // �⺻ ȯ�� ����
    }

    /// <summary>
    /// �ɷ� 5. ���� ��� ���� Ȯ�� ����
    /// </summary>
    public bool ShouldSpawnHigherGradeMaterial()
    {
        // TODO: ���� ��� Ȯ�� ���
        return false;
    }

    /// <summary>
    /// �ɷ� ���� �� ó��
    /// </summary>
    public void Purchase(PassiveAbility ability)
    {
        ability.Purchase();
        Debug.Log($"[����] {ability.name} ���� �Ϸ� �� �� {ability.timesPurchased}ȸ");
    }
}
