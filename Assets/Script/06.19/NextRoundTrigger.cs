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
        Debug.Log("[Debug] NextRoundTrigger 버튼 클릭 감지");

        if (isActivated)
        {
            Debug.Log("[Debug] 이미 활성화되어 무시함");
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

        // 애니메이션, 카운트다운 끝난 후 다시 클릭 가능하도록
        isActivated = false;
    }

}
