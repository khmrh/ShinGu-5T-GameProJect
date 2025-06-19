using UnityEngine;
using TMPro;
using System.Collections;

public class NextRoundTrigger : MonoBehaviour
{
    public CameraAnimationController cameraAnimator;
    public GameRoundManager roundManager;
    public TMP_Text countdownText;
    public float countdownInterval = 1f;

    [Header("ī��Ʈ�ٿ� ���� �� Ȱ��ȭ�� ������Ʈ��")]
    public GameObject[] objectsToActivate;

    private bool isActivated = false;

    private void OnMouseDown()
    {
        Debug.Log("[Debug] NextRoundTrigger ��ư Ŭ�� ����");

        if (isActivated)
        {
            Debug.Log("[Debug] �̹� Ȱ��ȭ�Ǿ� ������");
            return;
        }

        isActivated = true;
        StartCoroutine(HandleNextRoundStart());
    }

    IEnumerator HandleNextRoundStart()
    {
        if (cameraAnimator != null)
            cameraAnimator.LookDown();

        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(true);

            for (int i = 3; i >= 1; i--)
            {
                countdownText.text = i.ToString();
                yield return new WaitForSeconds(countdownInterval);
            }

            countdownText.gameObject.SetActive(false);
        }

        if (roundManager != null)
            roundManager.GoToNextRound();

        // ī��Ʈ�ٿ� �� ���� ���� �� ������Ʈ Ȱ��ȭ
        if (objectsToActivate != null)
        {
            foreach (var obj in objectsToActivate)
            {
                if (obj != null)
                    obj.SetActive(true);
            }
        }

        // �ٽ� Ŭ�� �����ϵ���
        isActivated = false;
    }
}
