using UnityEngine;
using System.Collections;

public class DoorOpenButton : MonoBehaviour
{
    public Animator doorAnimator;
    public AudioSource openSound;
    public GameObject targetPanel;

    private void OnMouseDown()
    {
        // 🔊 열기 사운드
        if (openSound != null && !openSound.isPlaying)
            openSound.Play();

        // 🔁 애니메이션 트리거 또는 bool
        if (doorAnimator != null)
            doorAnimator.SetBool("IsDoorOpen", true);

        // ⏱️ 0.5초 후 패널 열기
        StartCoroutine(ShowPanelAfterDelay(0.5f));
    }

    IEnumerator ShowPanelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (targetPanel != null)
            targetPanel.SetActive(true);
    }
}
