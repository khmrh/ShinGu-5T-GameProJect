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
        // ���� ���� ���� ���� ���߱�
        if (pepperManager != null)
            pepperManager.isRoundActive = false;

        countdownText.gameObject.SetActive(true);

        for (int i = 3; i >= 1; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(interval);
        }

        countdownText.gameObject.SetActive(false);

        // ī��Ʈ�ٿ� �� �� Ÿ�̸� ���� + ���� �̵� Ȱ��ȭ + ���� ���� ����
        if (timerManager != null)
            timerManager.ResetTimer();

        if (pepperManager != null)
        {
            pepperManager.isRoundActive = true;
            pepperManager.StartSpawning(); // ���⿡ �߰�!
        }
    }
}
