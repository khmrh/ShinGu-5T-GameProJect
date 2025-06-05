using UnityEngine;
using System.Collections;


public class PepperManager : MonoBehaviour
{
    public GameObject pepperPrefab;
    public int maxPepperCount = 20;
    public float fixedY = 1f; // y축 고정

    private int currentPepperCount = 0;

    public GridManager gridManager;

    // 4개 중심 좌표 (x, z)
    private Vector3[] spawnCenters = new Vector3[]
    {
        new Vector3(-10f, 0f, 10f),
        new Vector3(-10f, 0f, -10f),
        new Vector3(10f, 0f, 10f),
        new Vector3(10f, 0f, -10f)
    };

    private float spawnRange = 10f; // 중심 좌표에서 ±10 범위 (즉 20x20 영역)

    private void Start()
    {
        StartCoroutine(SpawnRoutine());

        if (gridManager == null)
        {
            gridManager = FindObjectOfType<GridManager>();
        }


    }

    private IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(3f); // 게임 시작 후 3초 대기

        while (currentPepperCount < maxPepperCount)
        {
            for (int i = 0; i < spawnCenters.Length; i++)
            {
                if (currentPepperCount >= maxPepperCount)
                    break;

                Vector3 spawnPos = GetRandomPositionInArea(spawnCenters[i]);
                SpawnPepperAtPosition(spawnPos);
            }
            yield return new WaitForSeconds(1f); // 1초마다 4개씩 소환
        }

        // 이후는 필요하면 다시 1초에 1개씩 생성하는 로직 추가 가능
    }

    private Vector3 GetRandomPositionInArea(Vector3 center)
    {
        float randomX = Random.Range(-spawnRange, spawnRange);
        float randomZ = Random.Range(-spawnRange, spawnRange);
        return new Vector3(center.x + randomX, fixedY, center.z + randomZ);
    }

    private void SpawnPepperAtPosition(Vector3 position)
    {
        Instantiate(pepperPrefab, position, Quaternion.identity);
        currentPepperCount++;
    }
    public void TryClonePepper(DraggablePepper original)
    {
        // 예: 빈 칸 찾기
        GridCell emptyCell = gridManager.FindEmptyCell();
        if (emptyCell == null)
        {
            Debug.Log("빈 칸이 없습니다!");
            return;
        }

        // 복제 생성
        DraggablePepper clone = gridManager.CreateRankInCell(emptyCell, original.pepperLevel);

        // 원본 삭제
        gridManager.RemoveRank(original);
    }
}

