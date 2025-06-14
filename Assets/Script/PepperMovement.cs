using UnityEngine;

public class PepperMovement : MonoBehaviour
{
    private float baseSpeed = 2f;               // 기본 이동 속도 (변하지 않음)
    public float speed = 2f;                    // 실제 적용 속도 (패시브 등에 따라 달라짐)
    private Vector2 direction;                  // 이동 방향

    public float minX = -11.5f;
    public float maxX = -6.7f;
    public float minY = -3.5f;
    public float maxY = 3.3f;

    public PepperManager pepperManager;
    private bool isActive = true;               // 이동 활성화 여부

    private void Start()
    {
        direction = Random.insideUnitCircle.normalized;
        speed = baseSpeed * 1.3f;               // 기본 이동 속도에 1.3배 배율 적용 (더 빠르게 시작)
    }

    private void Update()
    {
        if (pepperManager != null && !pepperManager.isRoundActive)
            return;

        if (!isActive) return;

        Vector3 pos = transform.position;
        Vector3 move = (Vector3)(direction * speed * Time.deltaTime);
        Vector3 nextPos = pos + move;

        // 영역 밖으로 나가면 반사
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
            maxY = 3.3f;
        }
    }

    public void SetActive(bool active)
    {
        isActive = active;
    }

    // 기본 속도에 1.3배와 같은 배율을 곱해 새로운 기준 설정
    public void SetBaseSpeed(float baseMultiplier)
    {
        speed = baseSpeed * baseMultiplier;
    }

    // 패시브에 의해 속도에 둔화 배율 적용 (누적 가능)
    public void ApplySlowMultiplier(float slowMultiplier)
    {
        speed *= slowMultiplier;
    }
}
