using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public int currentRound = 1;

    public ScoreManager scoreManager;
    public GameTimerManager gameTimerManager;
    public GameResultUI resultUI;
    public CameraRotationController cameraController;  // ✅ 교체

    private void Start()
    {
        if (scoreManager == null)
            scoreManager = FindObjectOfType<ScoreManager>();

        if (gameTimerManager == null)
            gameTimerManager = FindObjectOfType<GameTimerManager>();

        if (resultUI == null)
            resultUI = FindObjectOfType<GameResultUI>();

        if (cameraController == null)
            cameraController = FindObjectOfType<CameraRotationController>();

        StartRound(currentRound);

        resultUI.HideAll();
    }

    public void StartRound(int round)
    {
        currentRound = round;

        scoreManager?.ResetForNextRound(currentRound);
        gameTimerManager?.ResetTimer();
        resultUI?.HideAll();

        cameraController?.LookDown();  // ✅ 아래로
    }

    public void ProceedToNextRound()
    {
        currentRound++;
        StartRound(currentRound);
    }

    public void OnResultClose()
    {
        resultUI?.HideAll();
        cameraController?.LookUp();    // ✅ 위로
    }
}
