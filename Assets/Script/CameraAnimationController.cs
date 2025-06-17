using UnityEngine;

public class CameraAnimationController : MonoBehaviour
{
    public Animator cameraAnimator; // Animator ������Ʈ�� �Ҵ��� ����
    public bool isCameraUpState;    // �ܺο��� ������ bool ���� (�ν����Ϳ��� ���� ����)

    void Start()
    {
        // Animator ������Ʈ�� ã�Ƽ� �Ҵ��մϴ�.
        // �� ��ũ��Ʈ�� ���� ���� ������Ʈ�� Animator ������Ʈ�� ���ٸ�, �ڽ� ������Ʈ���� ã�� ���� �ֽ��ϴ�.
        if (cameraAnimator == null)
        {
            cameraAnimator = GetComponent<Animator>();
            if (cameraAnimator == null)
            {
                Debug.LogError("Animator component not found on this GameObject or its children.");
                enabled = false; // Animator�� ������ ��ũ��Ʈ ��Ȱ��ȭ
                return;
            }
        }

        // �ʱ� ���� ����: isCameraUpState ���� ���� Animator �Ķ���� ����
        cameraAnimator.SetBool("IsCameraUp", isCameraUpState);
    }

    void Update()
    {
        // isCameraUpState ������ ����� ������ Animator�� IsCameraUp �Ķ���͸� ������Ʈ�մϴ�.
        // ���� ���ӿ����� Ư�� �̺�Ʈ(��ư Ŭ�� ��)�� ���� isCameraUpState�� �����ϰ� �˴ϴ�.
        if (cameraAnimator.GetBool("IsCameraUp") != isCameraUpState)
        {
            cameraAnimator.SetBool("IsCameraUp", isCameraUpState);
        }
    }

    // �ܺο��� isCameraUpState�� ������ �� �ִ� ���� �Լ�
    public void SetCameraUpState(bool state)
    {
        isCameraUpState = state;
        if (cameraAnimator != null)
        {
            cameraAnimator.SetBool("IsCameraUp", isCameraUpState);
        }
    }
}