using UnityEngine;
using TMPro;
using System.Collections;

public class GameStartCountdown : MonoBehaviour
{
    public TMP_Text countdownText;
    public float interval = 1f;

    public GameTimerManager timerManager;
    public PepperManager pepperManager;

    public GameObject pauseButtonObject;      // 일시정지 버튼 오브젝트 (버튼 + 콜라이더 포함)

    void Start()
    {
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        // 3,2,1 카운트다운 동안 버튼 비활성화 (버튼과 콜라이더 모두 꺼짐)
        if (pauseButtonObject != null)
            pauseButtonObject.SetActive(false);

        if (pepperManager != null)
            pepperManager.isRoundActive = false;

        countdownText.gameObject.SetActive(true);

        Time.timeScale = 0f;  // 시간 멈춤

        for (int i = 3; i >= 1; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSecondsRealtime(interval);
        }

        countdownText.gameObject.SetActive(false);

        Time.timeScale = 1f;  // 시간 재개

        // 카운트다운 끝난 후 버튼 다시 활성화
        if (pauseButtonObject != null)
            pauseButtonObject.SetActive(true);

        if (timerManager != null)
            timerManager.ResetTimer();

        if (pepperManager != null)
        {
            pepperManager.isRoundActive = true;
            pepperManager.StartSpawning();
        }

        if (pauseButtonObject != null)
        {
            pauseButtonObject.SetActive(true);

            // pauseButtonObject와 자식 오브젝트에 있는 모든 3D 콜라이더 활성화
            Collider[] colliders = pauseButtonObject.GetComponentsInChildren<Collider>(true);
            foreach (var collider in colliders)
            {
                collider.enabled = true;
            }
        }

    }
}
