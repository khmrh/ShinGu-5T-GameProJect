using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera mainCamera;
    public Camera blenderCamera; // Camera.001
    public Animator animator; // �������� ����Ʈ�� �ִϸ��̼��� ���� ������Ʈ

    void Start()
    {
        // ���� ī�޶� ���� ���� ī�޶� ��
        if (mainCamera != null) mainCamera.enabled = false;
        if (blenderCamera != null) blenderCamera.enabled = true;

        // �ִϸ��̼� ���
        if (animator != null)
        {
            animator.Play("AnimationName"); // �ִϸ��̼� Ŭ�� �̸�
        }
    }
}
