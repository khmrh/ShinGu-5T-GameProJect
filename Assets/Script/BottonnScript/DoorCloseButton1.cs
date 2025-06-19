using UnityEngine;

public class DoorCloseButton1 : MonoBehaviour
{
    public Animator doorAnimator;
    public AudioSource closeSound;
    public GameObject targetPanel;

    private void OnMouseDown()
    {
        // 🔊 닫기 사운드
        if (closeSound != null && !closeSound.isPlaying)
            closeSound.Play();

        // 🔁 애니메이터에서 닫는 동작
        if (doorAnimator != null)
            doorAnimator.SetBool("IsDoorOpen", false);

        // ❌ 패널 닫기
        if (targetPanel != null)
            targetPanel.SetActive(false);
    }
}
