using UnityEngine;

public class roundmanager : MonoBehaviour
{
    public int currentRound = 1;

    public ScoreManager scoreManager;
    public GameTimerManager gameTimerManager;
    public GameResultUI resultUI;
    public CameraAnimationController cameraController;  // 🔹 추가

    private void Start()
    {
        // 필요한 컴포넌트 자동 할당
        if (scoreManager == null)
            scoreManager = FindObjectOfType<ScoreManager>();

        if (gameTimerManager == null)
            gameTimerManager = FindObjectOfType<GameTimerManager>();

        if (resultUI == null)
            resultUI = FindObjectOfType<GameResultUI>();

        if (cameraController == null)
            cameraController = FindObjectOfType<CameraAnimationController>();  // 🔹 자동 할당

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

        if (cameraController != null)
            cameraController.LookDown(); // ⬇ 라운드 시작 시 아래 보기
    }

    // 다음 라운드로 진행 (버튼 클릭 시 호출)
    public void ProceedToNextRound()
    {
        currentRound++;
        StartRound(currentRound);
        Debug.Log($"라운드 {currentRound} 시작");
    }

    // 결과창 닫기 버튼이 이걸 호출
    public void OnResultClose()
    {
        if (resultUI != null)
            resultUI.HideAll();

        if (cameraController != null)
            cameraController.LookUp();  // ⬆ 결과창 닫을 때 위 보기
    }
}
