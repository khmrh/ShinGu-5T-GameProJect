using UnityEngine;

public class GameTimerManager : MonoBehaviour
{
    public float gameDuration = 90f;
    public static float remainingTime;
    private bool isGameOver = false;



    public Pepper_Game_UI gameUI;
    public GameResultUI resultUI;
    public ScoreManager scoreManager;  // ScoreManager 직접 연결
    public PepperManager pepperManager; // PepperManager 연결

    void Start()
    {


        remainingTime = gameDuration;
        if (gameUI == null)
            gameUI = FindObjectOfType<Pepper_Game_UI>();

        if (scoreManager == null)
            scoreManager = FindObjectOfType<ScoreManager>();

        if (pepperManager == null)
            pepperManager = FindObjectOfType<PepperManager>();

        // 게임 시작 시 라운드 활성화
        if (pepperManager != null)
        {
            pepperManager.isRoundActive = true;
        }
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

        // 라운드 비활성화 → 페퍼 움직임 및 클릭 제한
        if (pepperManager != null)
        {
            pepperManager.isRoundActive = false;
        }

        if (resultUI != null && scoreManager != null)
        {
            int goalScore = scoreManager.CalculateTargetScore(scoreManager.round);
            int currentScore = scoreManager.currentScore;
            int coin = scoreManager.coin;
            int round = scoreManager.round; //  round 추가

            if (scoreManager.IsGoalReached())
            {
                resultUI.ShowSuccess(goalScore, currentScore, coin, round); //  인자 4개 전달
            }
            else
            {
                resultUI.ShowFail(goalScore, currentScore, coin, round);    //  인자 4개 전달
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

        // 라운드 다시 활성화
        if (pepperManager != null)
        {
            pepperManager.isRoundActive = true;
        }

        Debug.Log("타이머 리셋 완료");
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }


    public static GameTimerManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void AddTime(float time)
    {
        remainingTime += time;
        Debug.Log($" 시간 증가: {time}초 → 남은 시간: {remainingTime}");
    }

    public void SetTime(float newTime)
    {
        remainingTime = Mathf.Max(0f, newTime);
    }
}
