using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PepperManager : MonoBehaviour
{
    [Header("설정")]
    public GameObject pepperPrefab;              // 페퍼 프리팹
    public Transform spawnArea;                   // 단일 큐브 오브젝트 (밖 페퍼 소환 범위)
    public float spawnOffsetZ = -3f;              // 큐브 앞쪽 Z 위치 오프셋 (-3)

    public GridManager gridManager;               // 그리드 매니저 (그리드 내 랜덤 위치 소환용)

    public int maxOutsidePepperCount = 20;       // 밖 페퍼 최대 수

    private List<GameObject> spawnedOutsidePeppers = new List<GameObject>();

    void Start()
    {
        StartCoroutine(SpawnOutsidePeppersRoutine());
    }

    // 그리드 내 빈 칸에 특정 레벨 페퍼 소환
    public void SpawnPepperInGrid(int level)
    {
        if (gridManager == null)
        {
            Debug.LogError("GridManager가 할당되어 있지 않습니다.");
            return;
        }

        GridCell emptyCell = gridManager.FindEmptyCell();

        if (emptyCell == null)
        {
            Debug.LogWarning("빈 셀이 없습니다.");
            return;
        }

        DraggablePepper newPepper = gridManager.CreateRankInCell(emptyCell, level);

        if (newPepper != null)
        {
            newPepper.transform.SetParent(emptyCell.transform);
            newPepper.transform.localPosition = Vector3.zero;
            newPepper.pepperManager = this;
        }
        else
        {
            Debug.LogWarning("그리드에 페퍼 생성 실패");
        }
    }

    // 밖 페퍼 소환 루틴
    private IEnumerator SpawnOutsidePeppersRoutine()
    {
        yield return new WaitForSeconds(3f);  // 시작 후 3초 대기

        while (true)
        {
            while (spawnedOutsidePeppers.Count < maxOutsidePepperCount)
            {
                int spawnCount = Mathf.Min(4, maxOutsidePepperCount - spawnedOutsidePeppers.Count);

                for (int i = 0; i < spawnCount; i++)
                {
                    Vector3 spawnPos = GetSpawnPosition(spawnArea);

                    GameObject pepper = Instantiate(pepperPrefab, spawnPos, Quaternion.identity);

                    // OutsidePepper 컴포넌트 추가 및 설정
                    OutsidePepper outside = pepper.GetComponent<OutsidePepper>();
                    if (outside == null)
                    {
                        outside = pepper.AddComponent<OutsidePepper>();
                    }
                    outside.pepperManager = this;

                    // 기본 레벨 설정 (여기서 레벨 랜덤 대신 1로 고정하거나 필요에 따라 수정 가능)
                    outside.SetPepper(1, gridManager.PepperSprites[0]);

                    spawnedOutsidePeppers.Add(pepper);
                }

                yield return new WaitForSeconds(1f); // 1초마다 최대 4개씩 소환
            }

            yield return null;
        }
    }

    // spawnArea 범위 내 X,Y 랜덤, Z는 spawnArea.z + spawnOffsetZ (즉 앞쪽)
    private Vector3 GetSpawnPosition(Transform area)
    {
        Vector3 center = area.position;
        Vector3 scale = area.localScale;

        float randomX = Random.Range(center.x - scale.x / 2f, center.x + scale.x / 2f);
        float randomY = Random.Range(center.y - scale.y / 2f, center.y + scale.y / 2f);
        float z = center.z + spawnOffsetZ;  // 큐브 앞쪽 Z 위치

        return new Vector3(randomX, randomY, z);
    }

    // 밖 페퍼가 파괴(또는 클릭 등)되었을 때 호출하는 함수
    public void OnOutsidePepperDestroyed(GameObject outsidePepper)
    {
        // 밖 페퍼 리스트에서 제거
        if (spawnedOutsidePeppers.Contains(outsidePepper))
        {
            spawnedOutsidePeppers.Remove(outsidePepper);
        }

        Debug.Log("Outside Pepper Destroyed: " + outsidePepper.name);

        // 예: 밖 페퍼가 파괴되면 그리드에 새로운 페퍼 생성 요청
        SpawnPepperInGridBySprite(outsidePepper.GetComponent<SpriteRenderer>().sprite);

        // 밖 페퍼 재생성 코루틴 호출
        StartCoroutine(RespawnOutsideAfterDelay());
    }

    // 예시로 스프라이트 기준으로 그리드에 생성하는 함수 (GridManager 연동)
    private void SpawnPepperInGridBySprite(Sprite sprite)
    {
        if (gridManager == null) return;

        bool success = gridManager.SpawnPepperBySprite(sprite);
        if (!success)
        {
            Debug.LogWarning("그리드에 페퍼 생성 실패");
        }
    }

    public void HandleOutsidePepperClicked(GameObject outsidePepperObj, Sprite sprite)
    {
        Debug.Log("HandleOutsidePepperClicked 호출됨");

        if (spawnedOutsidePeppers.Contains(outsidePepperObj))
        {
            spawnedOutsidePeppers.Remove(outsidePepperObj);
            Destroy(outsidePepperObj);

            gridManager.CreateRankInCellBySprite(sprite);

            StartCoroutine(RespawnOutsideAfterDelay());
        }
        else
        {
            Debug.LogWarning("outsidePepperObj가 spawnedOutsidePeppers에 없습니다.");
        }
    }

    public void HandleOutsidePepperClicked(Sprite clickedSprite)
    {
        bool success = gridManager.SpawnPepperBySprite(clickedSprite);
        if (!success)
        {
            Debug.LogWarning("그리드에 페퍼 생성 실패");
        }
        else
        {
            Debug.Log("페퍼 그리드 생성 성공");
        }
    }

    // 밖 페퍼 재생성 코루틴 (딜레이 후 밖 페퍼 소환)
    private IEnumerator RespawnOutsideAfterDelay(float delay = 2f)
    {
        yield return new WaitForSeconds(delay);

        while (spawnedOutsidePeppers.Count < maxOutsidePepperCount)
        {
            Vector3 spawnPos = GetSpawnPosition(spawnArea);

            GameObject pepper = Instantiate(pepperPrefab, spawnPos, Quaternion.identity);

            OutsidePepper outside = pepper.GetComponent<OutsidePepper>();
            if (outside == null)
            {
                outside = pepper.AddComponent<OutsidePepper>();
            }
            outside.pepperManager = this;

            // 여기서도 반드시 gridManager.PepperSprites[0]로 접근해야 함
            outside.SetPepper(1, gridManager.PepperSprites[0]);

            spawnedOutsidePeppers.Add(pepper);

            // 여러 개 동시에 소환을 원하면 반복문 추가 가능

            yield return null;
        }
    }

    public void HandleOutsidePepperClicked(int level, Sprite sprite)
    {
        if (gridManager == null)
        {
            Debug.LogWarning("GridManager가 할당되지 않았습니다.");
            return;
        }

        // level에 맞는 스프라이트를 gridManager에서 가져옴
        Sprite correctSprite = gridManager.PepperSprites[level - 1];

        // outside pepper를 그리드에 생성
        bool created = gridManager.SpawnPepperBySprite(correctSprite);

        if (!created)
        {
            Debug.Log("그리드에 페퍼 생성 실패");
        }
    }
}
