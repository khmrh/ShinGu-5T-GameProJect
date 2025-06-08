using UnityEngine;

public class GameRoundManager : MonoBehaviour
{
    public GameTimerManager timerManager;
    public ScoreManager scoreManager;
    public GameResultUI resultUI;

    void Start()
    {
        if (timerManager == null) timerManager = FindObjectOfType<GameTimerManager>();
        if (scoreManager == null) scoreManager = FindObjectOfType<ScoreManager>();
        if (resultUI == null) resultUI = FindObjectOfType<GameResultUI>();
    }

    public void GoToNextRound()
    {
        int nextRound = scoreManager.round + 1;

        scoreManager.ResetForNextRound(nextRound);
        timerManager.ResetTimer();
        resultUI.HideAll();

        Debug.Log($"라운드 {nextRound} 시작!");
    }
}
