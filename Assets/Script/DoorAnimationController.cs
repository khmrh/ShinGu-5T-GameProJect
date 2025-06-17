using UnityEngine;

public class DoorAnimationController : MonoBehaviour
{
    public Animator doorAnimator; // Animator ������Ʈ�� �Ҵ��� ����
    public bool isDoorOpenState;    // �ܺο��� ������ bool ���� (�ν����Ϳ��� ���� ����)

    void Start()
    {
        // Animator ������Ʈ�� ã�Ƽ� �Ҵ��մϴ�.
        // �� ��ũ��Ʈ�� ���� ���� ������Ʈ�� Animator ������Ʈ�� ���ٸ�, �ڽ� ������Ʈ���� ã�� ���� �ֽ��ϴ�.
        if (doorAnimator == null)
        {
            doorAnimator = GetComponent<Animator>();
            if (doorAnimator == null)
            {
                Debug.LogError("Animator component not found on this GameObject or its children.");
                enabled = false; // Animator�� ������ ��ũ��Ʈ ��Ȱ��ȭ
                return;
            }
        }

        // �ʱ� ���� ����: isDoorOpenState ���� ���� Animator �Ķ���� ����
        doorAnimator.SetBool("IsDoorOpen", isDoorOpenState);
    }

    void Update()
    {
        // isDoorOpenState ������ ����� ������ Animator�� IsDoorOpen �Ķ���͸� ������Ʈ�մϴ�.
        // ���� ���ӿ����� Ư�� �̺�Ʈ(��ư Ŭ�� ��)�� ���� isDoorOpenState�� �����ϰ� �˴ϴ�.
        if (doorAnimator.GetBool("IsDoorOpen") != isDoorOpenState)
        {
            doorAnimator.SetBool("IsDoorOpen", isDoorOpenState);
        }
    }

    // �ܺο��� isDoorOpenState�� ������ �� �ִ� ���� �Լ�
    public void SetDoorOpenState(bool state)
    {
        isDoorOpenState = state;
        if (doorAnimator != null)
        {
            doorAnimator.SetBool("IsDoorOpen", isDoorOpenState);
        }
    }
}