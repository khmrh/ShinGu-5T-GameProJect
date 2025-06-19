using UnityEngine;
using TMPro;
using System.Collections;

public class NextRoundTrigger : MonoBehaviour
{
    public CameraAnimationController cameraAnimator;
    public GameRoundManager roundManager;
    public TMP_Text countdownText;
    public float countdownInterval = 1f;

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

        // �ִϸ��̼�, ī��Ʈ�ٿ� ���� �� �ٽ� Ŭ�� �����ϵ���
        isActivated = false;
    }

}
