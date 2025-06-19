using UnityEngine;

public class roundmanager : MonoBehaviour
{
    public int currentRound = 1;

    public ScoreManager scoreManager;
    public GameTimerManager gameTimerManager;
    public GameResultUI resultUI;
    public CameraAnimationController cameraController;  // 카메라 애니메이션 컨트롤러

    [Header("관리할 오브젝트들")]
    public GameObject[] objectsToDisableOnRoundStart;  // 라운드 시작 시 비활성화할 오브젝트들

    public GameObject pauseButton; // 일시정지 버튼

    private bool objectsDisabled = false;  // 오브젝트 활성화 상태 토글 저장 변수

    private void Start()
    {
        // 컴포넌트 자동 할당
        if (scoreManager == null)
            scoreManager = FindObjectOfType<ScoreManager>();

        if (gameTimerManager == null)
            gameTimerManager = FindObjectOfType<GameTimerManager>();

        if (resultUI == null)
            resultUI = FindObjectOfType<GameResultUI>();

        if (cameraController == null)
            cameraController = FindObjectOfType<CameraAnimationController>();

        // 현재 라운드 시작
        StartRound(currentRound);

        // 결과 UI 숨기기
        resultUI.HideAll();
    }

    private void Update()
    {
        // 제한 시간이 0 이하이거나 게임 종료 상태면
        if (gameTimerManager != null)
        {
            if (GameTimerManager.remainingTime <= 0f || gameTimerManager.IsGameOver())
            {
                // 일시정지 버튼 비활성화
                if (pauseButton != null && pauseButton.activeSelf)
                    pauseButton.SetActive(false);

                // 지정된 오브젝트 비활성화
                SetObjectsActive(false);
            }
            else
            {
                // 제한 시간이 남아있으면 일시정지 버튼 활성화 유지
                if (pauseButton != null && !pauseButton.activeSelf)
                    pauseButton.SetActive(true);

                // 오브젝트가 꺼져있지 않은 경우 활성화 유지
                if (objectsDisabled == false)
                    SetObjectsActive(true);
            }
        }
    }

    // 새로운 라운드 시작
    public void StartRound(int round)
    {
        currentRound = round;

        // 점수 초기화
        if (scoreManager != null)
            scoreManager.ResetForNextRound(currentRound);

        // 타이머 초기화
        if (gameTimerManager != null)
            gameTimerManager.ResetTimer();

        // 결과 UI 숨김
        if (resultUI != null)
            resultUI.HideAll();

        // 카메라 아래 보기로 전환
        if (cameraController != null)
            cameraController.LookDown();

        // 지정된 오브젝트 활성화 및 상태 초기화
        SetObjectsActive(true);
        objectsDisabled = false;

        // 일시정지 버튼 활성화
        if (pauseButton != null)
            pauseButton.SetActive(true);
    }

    // 다음 라운드로 진행
    public void ProceedToNextRound()
    {
        currentRound++;
        StartRound(currentRound);
        Debug.Log($"라운드 {currentRound} 시작");
    }

    // 결과창 닫기 호출 시 처리
    public void OnResultClose()
    {
        // 결과 UI 숨기기
        if (resultUI != null)
            resultUI.HideAll();

        // 카메라 위 보기로 전환
        if (cameraController != null)
            cameraController.LookUp();

        // 일시정지 버튼 활성화
        if (pauseButton != null)
            pauseButton.SetActive(true);

        // 지정된 오브젝트 활성화
        SetObjectsActive(true);
    }

    // 결과창 보일 때 호출 - 버튼 및 오브젝트 비활성화
    public void OnShowResult()
    {
        if (pauseButton != null)
            pauseButton.SetActive(false);

        SetObjectsActive(false);
    }

    // 오브젝트 활성화 토글 처리 함수
    public void ToggleObjectsActive()
    {
        objectsDisabled = !objectsDisabled;

        if (objectsToDisableOnRoundStart == null) return;

        foreach (var obj in objectsToDisableOnRoundStart)
        {
            if (obj != null)
                obj.SetActive(!objectsDisabled); // 토글 상태에 따라 활성/비활성 설정
        }

        Debug.Log($"오브젝트 활성화 상태: {!objectsDisabled}");
    }

    // 지정한 오브젝트들을 일괄 활성화/비활성화 처리
    public void SetObjectsActive(bool active)
    {
        if (objectsToDisableOnRoundStart == null) return;

        foreach (var obj in objectsToDisableOnRoundStart)
        {
            if (obj != null)
                obj.SetActive(active);
        }
    }
}
