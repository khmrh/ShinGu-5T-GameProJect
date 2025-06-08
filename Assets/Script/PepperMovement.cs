using UnityEngine;

public class PepperMovement : MonoBehaviour
{
    public float speed = 2f;
    private Vector2 direction;

    // 스포너 영역 좌표 직접 지정
    public float minX = -11.5f;
    public float maxX = -6.7f;
    public float minY = -3.5f;
    public float maxY =  3.3f;

    // PepperManager 참조
    public PepperManager pepperManager;

    // 이동 활성화 여부
    private bool isActive = true;

    private void Start()
    {
        direction = Random.insideUnitCircle.normalized;
    }

    private void Update()
    {
        if (pepperManager != null && !pepperManager.isRoundActive)
            return;

        if (!isActive) return; // 상태에 따라 이동 차단

        Vector3 pos = transform.position;
        Vector3 move = (Vector3)(direction * speed * Time.deltaTime);

        Vector3 nextPos = pos + move;

        if (nextPos.x < minX || nextPos.x > maxX)
        {
            direction.x *= -1;
            nextPos.x = Mathf.Clamp(nextPos.x, minX, maxX);
        }

        if (nextPos.y < minY || nextPos.y > maxY)
        {
            direction.y *= -1;
            nextPos.y = Mathf.Clamp(nextPos.y, minY, maxY);
        }

        transform.position = nextPos;
    }

    public void SetSpawnAreaFromObject(GameObject spawnerObject)
    {
        Collider2D col = spawnerObject.GetComponent<Collider2D>();
        if (col != null)
        {
            Bounds bounds = col.bounds;
            minX = bounds.min.x;
            maxX = bounds.max.x;
            minY = bounds.min.y;
            maxY = bounds.max.y;
        }
        else
        {
            minX = -11.5f;
            maxX = -6.7f;
            minY = -3.5f;
            maxY =  3.3f;
        }
    }

    // 이동 활성화/비활성화 제어 함수
    public void SetActive(bool active)
    {
        isActive = active;
    }
}
