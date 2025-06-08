using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int currentScore = 0;
    public int currentCoin = 0; // 코인 변수 추가
    public int round = 1;

    public Pepper_Game_UI gameUI;
    public GameResultUI resultUI;

    private bool goalReached = false;

    void Start()
    {
        if (gameUI == null)
            gameUI = FindObjectOfType<Pepper_Game_UI>();

        if (resultUI == null)
            resultUI = FindObjectOfType<GameResultUI>();

        UpdateUI();
    }

    public void AddScore(int amount)
    {
        currentScore += amount;
        UpdateUI();

        int targetScore = CalculateTargetScore(round);
        if (currentScore >= targetScore)
        {
            goalReached = true;
        }
    }

    public void AddCoin(int amount)
    {
        currentCoin += amount;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (gameUI != null)
        {
            gameUI.UpdateScore(currentScore);
            gameUI.UpdateCoin(currentCoin);
            gameUI.UpdateTargetScore(CalculateTargetScore(round));
        }
    }

    public bool IsGoalReached() => goalReached;

    public int CalculateTargetScore(int r)
    {
        int target = Mathf.FloorToInt((15000f * Mathf.Pow(1.1f, r - 1)) / 100f) * 100;
        return target;
    }
}
