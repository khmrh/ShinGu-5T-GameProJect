using UnityEngine;

public class CameraAnimationController : MonoBehaviour
{
    public Animator cameraAnimator;              // ī�޶� �ִϸ�����
    [SerializeField] private string paramName = "IsCameraUp";  // Animator bool �Ķ���� �̸�
    private bool isCameraUpState = false;        // ���� ���� ���

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

        // �ʱ� ���� ����
        ApplyState(isCameraUpState);
    }

    /// <summary>
    /// ī�޶� ���� ������ ����
    /// </summary>
    public void LookUp()
    {
        SetCameraState(true);
    }

    /// <summary>
    /// ī�޶� �Ʒ��� ������ ����
    /// </summary>
    public void LookDown()
    {
        SetCameraState(false);
    }

    /// <summary>
    /// ī�޶� ���¸� �ܺο��� �������� ����
    /// </summary>
    public void SetCameraState(bool lookUp)
    {
        isCameraUpState = lookUp;
        ApplyState(isCameraUpState);
    }

    /// <summary>
    /// Animator �Ķ���� ����
    /// </summary>
    private void ApplyState(bool up)
    {
        if (cameraAnimator != null)
        {
            cameraAnimator.SetBool(paramName, up);
        }
    }
}
