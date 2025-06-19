using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PauseToggleCollider : MonoBehaviour
{
    public PauseManager pauseManager;

    private void OnMouseDown()
    {
        if (pauseManager != null)
        {
            pauseManager.TogglePause();
            Debug.Log("[Debug] PauseToggleCollider Ŭ���� - ��� ����");
        }
        else
        {
            Debug.LogWarning("PauseManager�� �Ҵ���� �ʾҽ��ϴ�!");
        }
    }
}
