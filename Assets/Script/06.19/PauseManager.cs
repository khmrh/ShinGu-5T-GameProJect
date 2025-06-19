using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public CameraAnimationController cameraAnimator;  // ī�޶� �ִϸ�����
    public GameObject pauseButton;                      // �Ͻ����� ���¿��� ���� ��ư
    public GameObject[] objectsToDisable;               // �Ͻ����� �� ���� ������Ʈ��

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
            cameraAnimator.LookUp();   // ī�޶� �� ����

        if (pauseButton != null)
            pauseButton.SetActive(true);

        SetObjectsActive(false);

        Debug.Log("[Debug] ���� �Ͻ�����");
    }

    void ResumeGame()
    {
        Time.timeScale = 1f;

        if (cameraAnimator != null)
            cameraAnimator.LookDown(); // ī�޶� �Ʒ� ����

        if (pauseButton != null)
            pauseButton.SetActive(true);

        SetObjectsActive(true);

        Debug.Log("[Debug] ���� �簳");
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
