using UnityEngine;

public class DoorButton : MonoBehaviour
{
    public Animator doorAnimator;         // Animator 연결
    public AudioSource clickSound;        // 클릭 사운드 (선택)
    private bool isDoorOpen = false;      // 현재 문이 열린 상태

    private void OnMouseDown()
    {
        // 🔊 클릭 사운드 재생
        if (clickSound != null && !clickSound.isPlaying)
        {
            clickSound.Play();
        }

        // 🔁 애니메이션 Bool 파라미터로 전환
        if (doorAnimator != null)
        {
            isDoorOpen = !isDoorOpen;
            doorAnimator.SetBool("IsDoorOpen", isDoorOpen);
        }
    }
}
