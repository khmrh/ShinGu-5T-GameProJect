using UnityEngine;

public class GameTimerManager : MonoBehaviour
{
    public float gameDuration = 180f;
    private float remainingTime;

    public Pepper_Game_UI gameUI;
    private bool isGameOver = false;

    public GameResultUI resultUI;
    public ScoreManager scoreManager; // ✅ 점수 확인용

    void Start()
    {
        remainingTime = gameDuration;

        if (gameUI == null)
            gameUI = FindObjectOfType<Pepper_Game_UI>();

        if (resultUI == null)
            resultUI = FindObjectOfType<GameResultUI>();

        if (scoreManager == null)
            scoreManager = FindObjectOfType<ScoreManager>();
    }

    void Update()
    {
        if (isGameOver) return;

        remainingTime -= Time.deltaTime;
        remainingTime = Mathf.Max(0, remainingTime);

        gameUI.UpdateTimer(remainingTime);

        if (remainingTime <= 0f)
        {
            HandleGameOver();
        }
    }

    void HandleGameOver()
    {
        isGameOver = true;
        Debug.Log("게임 종료");

        if (resultUI == null)
        {
            Debug.LogWarning("resultUI 연결 안 됨!");
            return;
        }

        if (scoreManager != null && scoreManager.IsGoalReached())
        {
            resultUI.ShowSuccess(); // ✅ 시간은 끝났고, 점수는 달성했으니 성공
        }
        else
        {
            resultUI.ShowFail();    // ✅ 점수 못 채웠으면 실패
        }
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }
}
