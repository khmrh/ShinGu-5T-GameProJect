using UnityEngine;

public class DoorAnimationController : MonoBehaviour
{
    public Animator doorAnimator; // Animator 컴포넌트를 할당할 변수
    public bool isDoorOpenState;    // 외부에서 제어할 bool 변수 (인스펙터에서 조작 가능)

    void Start()
    {
        // Animator 컴포넌트를 찾아서 할당합니다.
        // 이 스크립트가 붙은 게임 오브젝트에 Animator 컴포넌트가 없다면, 자식 오브젝트에서 찾을 수도 있습니다.
        if (doorAnimator == null)
        {
            doorAnimator = GetComponent<Animator>();
            if (doorAnimator == null)
            {
                Debug.LogError("Animator component not found on this GameObject or its children.");
                enabled = false; // Animator가 없으면 스크립트 비활성화
                return;
            }
        }

        // 초기 상태 설정: isDoorOpenState 값에 따라 Animator 파라미터 설정
        doorAnimator.SetBool("IsDoorOpen", isDoorOpenState);
    }

    void Update()
    {
        // isDoorOpenState 변수가 변경될 때마다 Animator의 IsDoorOpen 파라미터를 업데이트합니다.
        // 실제 게임에서는 특정 이벤트(버튼 클릭 등)에 따라 isDoorOpenState를 변경하게 됩니다.
        if (doorAnimator.GetBool("IsDoorOpen") != isDoorOpenState)
        {
            doorAnimator.SetBool("IsDoorOpen", isDoorOpenState);
        }
    }

    // 외부에서 isDoorOpenState를 설정할 수 있는 공개 함수
    public void SetDoorOpenState(bool state)
    {
        isDoorOpenState = state;
        if (doorAnimator != null)
        {
            doorAnimator.SetBool("IsDoorOpen", isDoorOpenState);
        }
    }
}