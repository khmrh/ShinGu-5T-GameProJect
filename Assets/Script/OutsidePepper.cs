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
            // 레벨과 스프라이트 같이 전달하도록 함수 새로 만들거나 수정
            pepperManager.HandleOutsidePepperClicked(pepperLevel, pepperSprite);
            pepperManager.OnOutsidePepperDestroyed(gameObject);
            Destroy(gameObject);
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
