using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera mainCamera;
    public Camera blenderCamera; // Camera.001
    public Animator animator; // 블렌더에서 임포트한 애니메이션이 붙은 오브젝트

    void Start()
    {
        // 메인 카메라 끄고 블렌더 카메라 켬
        if (mainCamera != null) mainCamera.enabled = false;
        if (blenderCamera != null) blenderCamera.enabled = true;

        // 애니메이션 재생
        if (animator != null)
        {
            animator.Play("AnimationName"); // 애니메이션 클립 이름
        }
    }
}
