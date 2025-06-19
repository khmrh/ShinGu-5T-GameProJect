using UnityEngine;
using TMPro;
using System.Collections;

public class GameStartCountdown : MonoBehaviour
{
    public TMP_Text countdownText;
    public float interval = 1f;

    public GameTimerManager timerManager;
    public PepperManager pepperManager;

    void Start()
    {
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        // 게임 시작 전에 페퍼 멈추기
        if (pepperManager != null)
            pepperManager.isRoundActive = false;

        countdownText.gameObject.SetActive(true);

        for (int i = 3; i >= 1; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(interval);
        }

        countdownText.gameObject.SetActive(false);

        // 카운트다운 끝 → 타이머 시작 + 페퍼 이동 활성화 + 페퍼 스폰 시작
        if (timerManager != null)
            timerManager.ResetTimer();

        if (pepperManager != null)
        {
            pepperManager.isRoundActive = true;
            pepperManager.StartSpawning(); // 여기에 추가!
        }
    }
}
