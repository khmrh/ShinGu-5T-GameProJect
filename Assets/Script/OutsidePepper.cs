using UnityEngine;

public class OutsidePepper : MonoBehaviour
{
    public Sprite pepperSprite;
    public int pepperLevel = 1;  // 기본값 1로 명확히 초기화

    [HideInInspector]
    public PepperManager pepperManager;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnMouseDown()
    {
        if (pepperManager != null && !pepperManager.isRoundActive)
        {
            Debug.Log("라운드 종료로 클릭 불가");
            return;
        }

        if (pepperManager != null)
        {
            pepperManager.HandleOutsidePepperClicked(pepperSprite);  // 스프라이트 전달
            pepperManager.OnOutsidePepperDestroyed(gameObject);      // 리스트에서 제거
            Destroy(gameObject);                                     // 삭제
        }
    }


    public void SetPepper(int level, Sprite sprite)
    {
        pepperLevel = level;
        pepperSprite = sprite;

        // SpriteRenderer에 스프라이트 적용
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
            spriteRenderer.sprite = sprite;
    }
}
