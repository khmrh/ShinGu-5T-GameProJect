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
            Debug.Log("[Debug] PauseToggleCollider 클릭됨 - 토글 실행");
        }
        else
        {
            Debug.LogWarning("PauseManager가 할당되지 않았습니다!");
        }
    }
}
