using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int currentScore = 0;
    public int round = 1;

    public Pepper_Game_UI gameUI;
    public GameResultUI resultUI;

    private bool goalReached = false; // ✅ 목표 점수 달성 여부만 저장

    void Start()
    {
        if (gameUI == null)
            gameUI = FindObjectOfType<Pepper_Game_UI>();

        if (resultUI == null)
            resultUI = FindObjectOfType<GameResultUI>();

        UpdateScoreUI();
    }

    public void AddScore(int amount)
    {
        currentScore += amount;
        UpdateScoreUI();

        // ✅ 목표 점수 달성했는지 저장만
        int targetScore = CalculateTargetScore(round);
        if (currentScore >= targetScore)
        {
            goalReached = true;
        }
    }

    public bool IsGoalReached()
    {
        return goalReached;
    }

    public int CalculateTargetScore(int r)
    {
        int target = Mathf.FloorToInt((5000f * Mathf.Pow(1.1f, r - 1)) / 100f) * 100;
        return target;
    }

    private void UpdateScoreUI()
    {
        if (gameUI != null)
        {
            gameUI.UpdateScore(currentScore);
            gameUI.UpdateTargetScore(CalculateTargetScore(round));
        }
    }
}
