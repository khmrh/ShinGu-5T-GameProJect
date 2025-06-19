using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public CameraAnimationController cameraAnimator;  // 카메라 애니메이터
    public GameObject pauseButton;                      // 일시정지 상태에서 보일 버튼
    public GameObject[] objectsToDisable;               // 일시정지 시 숨길 오브젝트들

    private bool isPaused = false;

    void Start()
    {
        if (pauseButton != null)
            pauseButton.SetActive(true);
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
            PauseGame();
        else
            ResumeGame();
    }

    void PauseGame()
    {
        Time.timeScale = 0f;

        if (cameraAnimator != null)
            cameraAnimator.LookUp();   // 카메라 위 보기

        if (pauseButton != null)
            pauseButton.SetActive(true);

        SetObjectsActive(false);

        Debug.Log("[Debug] 게임 일시정지");
    }

    void ResumeGame()
    {
        Time.timeScale = 1f;

        if (cameraAnimator != null)
            cameraAnimator.LookDown(); // 카메라 아래 보기

        if (pauseButton != null)
            pauseButton.SetActive(true);

        SetObjectsActive(true);

        Debug.Log("[Debug] 게임 재개");
    }

    void SetObjectsActive(bool active)
    {
        if (objectsToDisable == null) return;

        foreach (var obj in objectsToDisable)
        {
            if (obj != null)
                obj.SetActive(active);
        }
    }
}
