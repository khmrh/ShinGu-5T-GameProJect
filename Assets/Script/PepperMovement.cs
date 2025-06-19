using UnityEngine;

public class PepperMovement : MonoBehaviour
{
    private float baseSpeed = 2f;               // 변하지 않는 기본 속도
    private float baseSpeedMultiplier = 1f;     // 기본 속도 배율 (ex: 1.3f)
    private float slowMultiplier = 1f;          // 둔화 배율

    public float speed => baseSpeed * baseSpeedMultiplier * slowMultiplier; // 최종 속도

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
        baseSpeedMultiplier = 1.3f;             // 기본 이동 속도 배율 설정
        slowMultiplier = 1f;                    // 둔화 배율 초기화
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

    // 기본 속도 배율 설정
    public void SetBaseSpeed(float baseMultiplier)
    {
        baseSpeedMultiplier = baseMultiplier;
        Debug.Log($"SetBaseSpeed 호출: baseSpeedMultiplier = {baseSpeedMultiplier}, 최종 speed = {speed}");
    }

    // 둔화 배율 설정 (누적이 아닌 직접 설정)
    public void ApplySlowMultiplier(float newSlowMultiplier)
    {
        slowMultiplier = newSlowMultiplier;
        Debug.Log($"ApplySlowMultiplier 호출: slowMultiplier = {slowMultiplier}, 최종 speed = {speed}");
    }
}
