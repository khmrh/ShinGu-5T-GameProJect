using UnityEngine;

public class GameTimerManager : MonoBehaviour
{
    public float gameDuration = 180f;
    private float remainingTime;
    private bool isGameOver = false;

    public Pepper_Game_UI gameUI;
    public GameResultUI resultUI;
    public ScoreManager scoreManager;  // ScoreManager 직접 연결

    void Start()
    {
        remainingTime = gameDuration;
        if (gameUI == null)
            gameUI = FindObjectOfType<Pepper_Game_UI>();

        if (scoreManager == null)
            scoreManager = FindObjectOfType<ScoreManager>();
    }

    void Update()
    {
        if (isGameOver) return;

        remainingTime -= Time.deltaTime;
        remainingTime = Mathf.Max(0, remainingTime);
        gameUI?.UpdateTimer(remainingTime);

        if (remainingTime <= 0f)
        {
            HandleGameOver();
        }
    }

    void HandleGameOver()
    {
        isGameOver = true;
        Debug.Log("게임 종료");

        if (resultUI != null && scoreManager != null)
        {
            int goalScore = scoreManager.CalculateTargetScore(scoreManager.round);
            int currentScore = scoreManager.currentScore;
            int coin = scoreManager.coin;
            int round = scoreManager.round; // ✅ round 추가

            if (scoreManager.IsGoalReached())
            {
                resultUI.ShowSuccess(goalScore, currentScore, coin, round); // ✅ 인자 4개 전달
            }
            else
            {
                resultUI.ShowFail(goalScore, currentScore, coin, round);    // ✅ 인자 4개 전달
            }
        }
        else
        {
            Debug.LogWarning("resultUI 혹은 scoreManager가 연결되지 않음");
        }
    }


    public void ResetTimer()
    {
        remainingTime = gameDuration;
        isGameOver = false;
        gameUI?.UpdateTimer(remainingTime);
        Debug.Log("타이머 리셋 완료");
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }
}
