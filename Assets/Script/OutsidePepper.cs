using UnityEngine;

public class OutsidePepper : MonoBehaviour
{
    public Sprite pepperSprite;
    public int pepperLevel = 1;  // �⺻�� 1�� ��Ȯ�� �ʱ�ȭ

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
            Debug.Log("���� ����� Ŭ�� �Ұ�");
            return;
        }

        if (pepperManager != null)
        {
            pepperManager.HandleOutsidePepperClicked(pepperSprite);  // ��������Ʈ ����
            pepperManager.OnOutsidePepperDestroyed(gameObject);      // ����Ʈ���� ����
            Destroy(gameObject);                                     // ����
        }
    }


    public void SetPepper(int level, Sprite sprite)
    {
        pepperLevel = level;
        pepperSprite = sprite;

        // SpriteRenderer�� ��������Ʈ ����
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
            spriteRenderer.sprite = sprite;
    }
}
