using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PepperManager : MonoBehaviour
{
    [Header("설정")]
    public GameObject pepperPrefab;              // 페퍼 프리팹
    public Transform spawnArea;                  // 밖 페퍼가 생성될 범위 오브젝트
    public float spawnOffsetZ = -3f;             // Z축 위치 보정 (앞쪽으로)

    public GridManager gridManager;              // 그리드 매니저 연결

    public int maxOutsidePepperCount = 20;       // 필드에 유지할 최대 밖 페퍼 수

    private List<GameObject> spawnedOutsidePeppers = new List<GameObject>();  // 현재 존재하는 밖 페퍼들 리스트

    public GameObject spawnerObject;

    [HideInInspector]
    public bool isRoundActive = true;  // 라운드가 진행 중인지 상태

    


    void Start()
    {
        StartCoroutine(ContinuousOutsidePepperSpawner());  // 계속 페퍼 관리 루틴 시작
    }

    // 1초에 1개씩 밖 페퍼를 생성하며, 최대 개수 유지
    private IEnumerator ContinuousOutsidePepperSpawner()
    {
        yield return new WaitForSeconds(1f);  // 초기 대기

        while (true)
        {
            if (isRoundActive && spawnedOutsidePeppers.Count < maxOutsidePepperCount)
            {
                SpawnOneOutsidePepper();  // 1개 생성
            }

            yield return new WaitForSeconds(1f);  // 1초마다 반복
        }
    }

    // 밖 페퍼 1개 생성
    private void SpawnOneOutsidePepper()
    {
        Vector3 spawnPos = GetSpawnPosition(spawnArea);

        GameObject pepper = Instantiate(pepperPrefab, spawnPos, Quaternion.identity);

        PepperMovement pm = pepper.GetComponent<PepperMovement>();
        pm.SetSpawnAreaFromObject(spawnerObject);
        pm.pepperManager = this;  // PepperMovement에 pepperManager 연결 (아래에서 추가 예정)

        OutsidePepper outside = pepper.GetComponent<OutsidePepper>();
        if (outside == null)
        {
            outside = pepper.AddComponent<OutsidePepper>();
        }

        outside.pepperManager = this;

        // 항상 1레벨로 고정
        outside.SetPepper(1, gridManager.PepperSprites[0]);

        spawnedOutsidePeppers.Add(pepper);
    }

    // 밖 페퍼가 사라질 때 호출 → 리스트에서 제거
    public void OnOutsidePepperDestroyed(GameObject outsidePepper)
    {
        if (spawnedOutsidePeppers.Contains(outsidePepper))
        {
            spawnedOutsidePeppers.Remove(outsidePepper);
        }

        Debug.Log("Outside Pepper Destroyed: " + outsidePepper.name);
    }

    // 밖 페퍼 클릭 시 그리드에 생성 요청 (라운드 진행중일 때만)
    public void HandleOutsidePepperClicked(Sprite sprite)
    {
        if (!isRoundActive)
        {
            Debug.Log("라운드 종료, 밖 페퍼 클릭 무시");
            return;
        }

        bool success = gridManager.SpawnPepperBySprite(sprite);

        if (!success)
        {
            Debug.LogWarning("그리드에 페퍼 생성 실패");
        }
        else
        {
            Debug.Log("그리드에 페퍼 생성 성공");
        }
    }

    // 빈 셀에 특정 레벨 페퍼 생성 (직접 호출용)
    public void SpawnPepperInGrid(int level)
    {
        if (gridManager == null)
        {
            Debug.LogError("GridManager가 할당되지 않았습니다.");
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

            newPepper.pepperManager = this; // 🔥 여기 반드시 추가!
        }
    }


    // 밖 페퍼 생성 위치 결정 (랜덤한 X, Y, Z는 앞쪽)
    private Vector3 GetSpawnPosition(Transform area)
    {
        Vector3 center = area.position;
        Vector3 scale = area.localScale;

        float randomX = Random.Range(center.x - scale.x / 2f, center.x + scale.x / 2f);
        float randomY = Random.Range(center.y - scale.y / 2f, center.y + scale.y / 2f);
        float z = center.z + spawnOffsetZ;

        return new Vector3(randomX, randomY, z);
    }

    public void OnRoundEnd()
    {
        isRoundActive = false;

        // 밖 페퍼 정지 및 클릭 차단
        foreach (var pepper in spawnedOutsidePeppers)
        {
            if (pepper != null)
            {
                var pm = pepper.GetComponent<PepperMovement>();
                if (pm != null)
                    pm.enabled = false;

                var collider = pepper.GetComponent<Collider2D>();
                if (collider != null)
                    collider.enabled = false;
            }
        }

        // 그리드 안 페퍼 클릭 차단
        DisableGridPepperInteraction();
    }



    public void DisableGridPepperInteraction()
    {
        for (int x = 0; x < gridManager.gridWidth; x++)
        {
            for (int y = 0; y < gridManager.gridHeight; y++)
            {
                var cell = gridManager.grid[x, y];
                if (cell != null && cell.currentRank != null)
                {
                    cell.currentRank.isInteractable = false;
                }
            }
        }
    }

  
}
