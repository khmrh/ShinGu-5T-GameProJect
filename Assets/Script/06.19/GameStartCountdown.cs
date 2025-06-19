using UnityEngine;
using TMPro;
using System.Collections;

public class GameStartCountdown : MonoBehaviour
{
    public TMP_Text countdownText;
    public float interval = 1f;

    public GameTimerManager timerManager;
    public PepperManager pepperManager;

    public GameObject pauseButtonObject;      // �Ͻ����� ��ư ������Ʈ (��ư + �ݶ��̴� ����)

    void Start()
    {
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        // 3,2,1 ī��Ʈ�ٿ� ���� ��ư ��Ȱ��ȭ (��ư�� �ݶ��̴� ��� ����)
        if (pauseButtonObject != null)
            pauseButtonObject.SetActive(false);

        if (pepperManager != null)
            pepperManager.isRoundActive = false;

        countdownText.gameObject.SetActive(true);

        Time.timeScale = 0f;  // �ð� ����

        for (int i = 3; i >= 1; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSecondsRealtime(interval);
        }

        countdownText.gameObject.SetActive(false);

        Time.timeScale = 1f;  // �ð� �簳

        // ī��Ʈ�ٿ� ���� �� ��ư �ٽ� Ȱ��ȭ
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

            // pauseButtonObject�� �ڽ� ������Ʈ�� �ִ� ��� 3D �ݶ��̴� Ȱ��ȭ
            Collider[] colliders = pauseButtonObject.GetComponentsInChildren<Collider>(true);
            foreach (var collider in colliders)
            {
                collider.enabled = true;
            }
        }

    }
}
