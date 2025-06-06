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
        if (pepperManager != null)
        {
            pepperManager.HandleOutsidePepperClicked(pepperLevel, pepperSprite); // level도 같이 전달
            Destroy(gameObject); // 외부 페퍼는 클릭 후 제거
        }
    }

    public void SetPepper(int level, Sprite sprite)
    {
        pepperLevel = level;
        pepperSprite = sprite;

        // 그리고 SpriteRenderer에도 반영
        var sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.sprite = sprite;
    }
}
