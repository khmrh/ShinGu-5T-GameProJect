using UnityEngine;

public class ParentFollowChild : MonoBehaviour
{
    public Transform childTransform;  // ���� �ڽ� Ʈ������

    void LateUpdate()
    {
        if (childTransform == null) return;

        // �θ� ��ġ�� ȸ���� �ڽİ� ����
        transform.position = childTransform.position;
        transform.rotation = childTransform.rotation;
    }
}
