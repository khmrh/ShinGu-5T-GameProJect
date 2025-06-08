using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("그리드 수치 조정")]
    public int gridWidth = 8;                                        // 가로 칸 수
    public int gridHeight = 8;                                       // 세로 칸 수
    public float cellSize = 1.05f;                                   // 각 칸의 크기
    [Header("그리드 형태 구성")]
    public GameObject cellPrefabs;                                   // 빈칸 프리팹
    public Transform gridContainer;                                  // 그리드를 담을 부모 오브젝트

    [Header("재료 수치 조정")]
    public int maxPepperLevel = 8;                                   // 최대 Pepper 레벨
    [Header("재료 형태 구성")]
    public GameObject PepperPrefabs;                                 // Pepper 프리팹
    public Sprite[] PepperSprites;                                   // 각 레벨별 Pepper 이미지

    [Header("점수 시스템")]
    public ScoreManager scoreManager;                                // ScoreManager 연결

    public GridCell[,] grid;

    void Start()
    {
        InitializeGrid();
    }

    void Update()
    {
        // 게임 테스트용 *(이후 제거)
        if (Input.GetKeyDown(KeyCode.W))
        {
            SpawnNewRank();
        }
    }

    public DraggablePepper CreateRankInCellBySprite(Sprite sprite)
    {
        GridCell emptyCell = FindEmptyCell();
        if (emptyCell == null) return null;

        Vector3 rankPosition = emptyCell.transform.position;

        GameObject pepperObj = Instantiate(PepperPrefabs, rankPosition, Quaternion.identity, gridContainer);
        pepperObj.name = "Pepper_Sprite";

        DraggablePepper rank = pepperObj.AddComponent<DraggablePepper>();

        // DraggablePepper에 스프라이트 세팅 함수 필요
        rank.SetSprite(sprite);

        emptyCell.SetRank(rank);

        return rank;
    }

    void InitializeGrid()                                           // 그리드 초기화
    {
        grid = new GridCell[gridWidth, gridHeight];                 // 지정된 크기의 2차원 배열 생성

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                // 각 셀의 위치 계산 (그리드의 중앙을 기준으로)
                Vector3 position = new Vector3(
                    x * cellSize - (gridWidth * cellSize / 2) + cellSize / 2,
                    y * cellSize - (gridHeight * cellSize / 2) + cellSize / 2,
                    1f
                );

                // 셀 오브젝트 생성 및 GridContainer의 자식으로 설정
                GameObject cellObj = Instantiate(cellPrefabs, position, Quaternion.identity, gridContainer);
                // 생성된 오브젝트에 GridCell 컴포넌트 추가
                GridCell cell = cellObj.AddComponent<GridCell>();
                // GridCell 초기화 (좌표 정보 저장)
                cell.Initialize(x, y);

                grid[x, y] = cell;  // 배열에 저장
            }
        }
    }

    public DraggablePepper CreateRankInCell(GridCell cell, int level)
    {

        if (cell == null || !cell.IsEmpty()) return null;            // 비어있는 칸이 아니면 생성 실패

        level = Mathf.Clamp(level, 1, maxPepperLevel);                // 레벨 범위를 최대 레벨 내로 제한

        // 해당 셀의 중앙 위치에 계급장 위치 설정
        Vector3 rankPosition = new Vector3(cell.transform.position.x, cell.transform.position.y, 0f);

        // Pepper 프리팹을 이용하여 계급장 오브젝트 생성 및 GridContainer의 자식으로 설정
        GameObject pepperObj = Instantiate(PepperPrefabs, rankPosition, Quaternion.identity, gridContainer);
        pepperObj.name = "Pepper_Lvel" + level;

        // 생성된 오브젝트에 DraggablePepper 컴포넌트 추가
        DraggablePepper rank = pepperObj.AddComponent<DraggablePepper>();
        // 계급장의 레벨 설정
        rank.SetPepperLevel(level);

        // 해당 셀에 계급장 정보 저장
        cell.SetRank(rank);

        return rank;

    }

    public GridCell FindEmptyCell()                                 // 비어있는 칸 찾기
    {
        List<GridCell> emptyCells = new List<GridCell>();            // 빈 칸들을 저장할 리스트

        for (int x = 0; x < gridWidth; x++)                        // 모든 가로 칸 순회
        {
            for (int y = 0; y < gridHeight; y++)                   // 모든 세로 칸 순회
            {
                if (grid[x, y].IsEmpty())                           // 해당 칸이 비어있는 칸인지 확인
                {
                    emptyCells.Add(grid[x, y]);
                }
            }
        }

        if (emptyCells.Count == 0)                                  // 빈칸이 없으면 null 값 반환
        {
            return null;
        }

        // 리스트에서 랜덤하게 빈 칸 하나 선택하여 반환
        return emptyCells[Random.Range(0, emptyCells.Count)];

    }

    public bool SpawnNewRank()                                      // 새 계급장 생성
    {
        GridCell emptyCell = FindEmptyCell();                       // 1. 비어있는 칸 찾기
        if (emptyCell == null) return false;                        // 2. 비어있는 칸이 없으면 생성 실패

        // 80% 확률로 레벨 1, 20% 확률로 레벨 2의 계급장 레벨 결정 (수업에 쓴 그대로 베타 제작, 나중에 변경)
        int rankLevel = Random.Range(0, 100) < 80 ? 1 : 2;

        CreateRankInCell(emptyCell, rankLevel);                     // 3. 해당 빈 칸에 계급장 생성 및 설정

        return true;
    }

    public GridCell FindClosestCell(Vector3 position)
    {
        // 해당 position을 포함하는 셀이 있는지 확인
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (grid[x, y].ContainsPosition(position))
                {
                    return grid[x, y];
                }
            }
        }

        // 포함하는 셀이 없으면 가장 가까운 셀을 찾음
        GridCell closestCell = null;
        float closestDistance = float.MaxValue;

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                // 현재 위치와 각 그리드 셀의 중심 사이의 거리 계산
                float distance = Vector3.Distance(position, grid[x, y].transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestCell = grid[x, y];
                }
            }
        }
        // 너무 멀리 떨어진 셀은 null 반환
        if (closestDistance > cellSize * 2)
        {
            return null;
        }
        return closestCell;
    }

    public void MergeRanks(DraggablePepper draggedRank, DraggablePepper targetRank)
    {
        // 새로운 레벨 계산
        int newLevel = targetRank.pepperLevel + 1;

        // 최대 레벨 초과 시 드래그한 랭크 제거 후 종료
        if (newLevel > maxPepperLevel)
        {
            RemoveRank(draggedRank);
            return;
        }

        //  점수 추가
        if (scoreManager != null)
        {
            int points = GetScoreForLevel(newLevel);
            scoreManager.AddScore(points);

            //  코인 추가 (예: 병합 레벨 * 4?)
            int coinReward = draggedRank.pepperLevel * 1;
            scoreManager.AddCoin(coinReward);
        }

        //  타겟 랭크의 레벨을 증가시키고 드래그한 랭크 제거
        targetRank.SetPepperLevel(newLevel);
        RemoveRank(draggedRank);
    }


    public void RemoveRank(DraggablePepper rank)
    {
        if (rank == null) return;

        // 해당 랭크가 속한 셀에서 랭크 정보 제거 및 랭크 오브젝트 파괴
        if (rank.currentCell != null)
        {
            rank.currentCell.currentRank = null;
            Destroy(rank.gameObject);
        }
    }

    // 병합된 Pepper의 레벨에 따라 점수를 반환하는 함수(좀수는 일딴 적어둔 거임)
    private int GetScoreForLevel(int level)
    {
        switch (level)
        {
            case 2: return 50;     // 레벨 2로 병합되면 50점
            case 3: return 100;    // 레벨 3으로 병합되면 100점
            case 4: return 200;    // 레벨 4으로 병합되면 200점
            case 5: return 400;    // 레벨 5으로 병합되면 800점
            case 6: return 800;    // 레벨 6으로 병합되면 800점
            case 7: return 1600;
            case 8: return 3200;
            default: return 0;     // 정의되지 않은 레벨은 점수 없음
        }
    }

    public bool SpawnPepperBySprite(Sprite sprite)
    {
        if (sprite == null) return false;

        DraggablePepper newRank = CreateRankInCellBySprite(sprite);
        if (newRank == null)
        {
            Debug.Log("빈 셀이 없어 생성 실패");
            return false;
        }

        Debug.Log("그리드에 스프라이트 페퍼 생성 완료");
        return true;
    }


}