using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public int currentRound = 1;
    public ScoreManager scoreManager;
    public GameTimerManager gameTimerManager;
    public GameResultUI resultUI;



    private void Start()
    {
        // 필요한 컴포넌트 자동 할당
        if (scoreManager == null)
            scoreManager = FindObjectOfType<ScoreManager>();

        if (gameTimerManager == null)
            gameTimerManager = FindObjectOfType<GameTimerManager>();

        if (resultUI == null)
            resultUI = FindObjectOfType<GameResultUI>();

        StartRound(currentRound);

        resultUI.HideAll();
    }


    // 새로운 라운드 시작
    public void StartRound(int round)
    {
        currentRound = round;

        if (scoreManager != null)
            scoreManager.ResetForNextRound(currentRound);

        if (gameTimerManager != null)
            gameTimerManager.ResetTimer();

        if (resultUI != null)
            resultUI.HideAll();
     
    }

    // 다음 라운드로 진행 (버튼 클릭 시 호출)
    public void ProceedToNextRound()
    {
        currentRound++;
        StartRound(currentRound);
    }
}
