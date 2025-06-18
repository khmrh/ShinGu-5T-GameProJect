using UnityEngine;

public class CameraAnimationController : MonoBehaviour
{
    public Animator cameraAnimator;              // 카메라 애니메이터
    [SerializeField] private string paramName = "IsCameraUp";  // Animator bool 파라미터 이름
    private bool isCameraUpState = false;        // 현재 상태 기록

    void Start()
    {
        if (cameraAnimator == null)
        {
            cameraAnimator = GetComponent<Animator>();
            if (cameraAnimator == null)
            {
                Debug.LogError("Animator component not found.");
                enabled = false;
                return;
            }
        }

        // 초기 상태 적용
        ApplyState(isCameraUpState);
    }

    /// <summary>
    /// 카메라가 위를 보도록 설정
    /// </summary>
    public void LookUp()
    {
        SetCameraState(true);
    }

    /// <summary>
    /// 카메라가 아래를 보도록 설정
    /// </summary>
    public void LookDown()
    {
        SetCameraState(false);
    }

    /// <summary>
    /// 카메라 상태를 외부에서 수동으로 설정
    /// </summary>
    public void SetCameraState(bool lookUp)
    {
        isCameraUpState = lookUp;
        ApplyState(isCameraUpState);
    }

    /// <summary>
    /// Animator 파라미터 적용
    /// </summary>
    private void ApplyState(bool up)
    {
        if (cameraAnimator != null)
        {
            cameraAnimator.SetBool(paramName, up);
        }
    }
}
