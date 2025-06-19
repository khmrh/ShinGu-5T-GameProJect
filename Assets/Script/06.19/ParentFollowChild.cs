using UnityEngine;

public class ParentFollowChild : MonoBehaviour
{
    public Transform childTransform;  // 따라갈 자식 트랜스폼

    void LateUpdate()
    {
        if (childTransform == null) return;

        // 부모 위치와 회전을 자식과 맞춤
        transform.position = childTransform.position;
        transform.rotation = childTransform.rotation;
    }
}
