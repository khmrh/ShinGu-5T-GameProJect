using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int currentScore = 0;
    public int round = 1;
    public int coin = 0;

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

    public void StartRound(int newRound)
    {
        round = newRound;
        currentScore = 0;
        goalReached = false;
        UpdateUI();
    }

    public void ResetForNextRound(int nextRound)
    {
        round = nextRound;
        currentScore = 0;
        goalReached = false;
        UpdateUI();
    }

    public void AddScore(int amount)
    {
        currentScore += amount;
        CheckGoalReached();
        UpdateUI();
    }

    public void SubtractScore(int amount)
    {
        currentScore = Mathf.Max(0, currentScore - amount);
        CheckGoalReached();
        UpdateUI();
    }

    public void AddCoin(int amount)
    {
        coin += amount;
        UpdateUI();
    }

    private void CheckGoalReached()
    {
        int target = CalculateTargetScore(round);
        if (currentScore >= target)
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
        float baseScore = 13000f;
        float multiplier = Mathf.Pow(1.1f, r - 1);
        int target = Mathf.FloorToInt(baseScore * multiplier);
        return target;
    }

    private void UpdateUI()
    {
        if (gameUI != null)
        {
            gameUI.UpdateScore(currentScore);
            gameUI.UpdateCoin(coin);
            gameUI.UpdateRound(round);
            gameUI.UpdateTargetScore(CalculateTargetScore(round));
        }
    }
}
