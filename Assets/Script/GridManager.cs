using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("�׸��� ��ġ ����")]
    public int gridWidth = 8;                                        // ���� ĭ ��
    public int gridHeight = 8;                                       // ���� ĭ ��
    public float cellSize = 1.05f;                                   // �� ĭ�� ũ��
    [Header("�׸��� ���� ����")]
    public GameObject cellPrefabs;                                   // ��ĭ ������
    public Transform gridContainer;                                  // �׸��带 ���� �θ� ������Ʈ

    [Header("��� ��ġ ����")]
    public int maxPepperLevel = 8;                                   // �ִ� Pepper ����
    [Header("��� ���� ����")]
    public GameObject PepperPrefabs;                                 // Pepper ������
    public Sprite[] PepperSprites;                                   // �� ������ Pepper �̹���

    [Header("���� �ý���")]
    public ScoreManager scoreManager;                                // ScoreManager ����

    public GridCell[,] grid;

    void Start()
    {
        InitializeGrid();
    }

    void Update()
    {
        // ���� �׽�Ʈ�� *(���� ����)
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

        // DraggablePepper�� ��������Ʈ ���� �Լ� �ʿ�
        rank.SetSprite(sprite);

        emptyCell.SetRank(rank);

        return rank;
    }

    void InitializeGrid()                                           // �׸��� �ʱ�ȭ
    {
        grid = new GridCell[gridWidth, gridHeight];                 // ������ ũ���� 2���� �迭 ����

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                // �� ���� ��ġ ��� (�׸����� �߾��� ��������)
                Vector3 position = new Vector3(
                    x * cellSize - (gridWidth * cellSize / 2) + cellSize / 2,
                    y * cellSize - (gridHeight * cellSize / 2) + cellSize / 2,
                    1f
                );

                // �� ������Ʈ ���� �� GridContainer�� �ڽ����� ����
                GameObject cellObj = Instantiate(cellPrefabs, position, Quaternion.identity, gridContainer);
                // ������ ������Ʈ�� GridCell ������Ʈ �߰�
                GridCell cell = cellObj.AddComponent<GridCell>();
                // GridCell �ʱ�ȭ (��ǥ ���� ����)
                cell.Initialize(x, y);

                grid[x, y] = cell;  // �迭�� ����
            }
        }
    }

    public DraggablePepper CreateRankInCell(GridCell cell, int level)
    {

        if (cell == null || !cell.IsEmpty()) return null;            // ����ִ� ĭ�� �ƴϸ� ���� ����

        level = Mathf.Clamp(level, 1, maxPepperLevel);                // ���� ������ �ִ� ���� ���� ����

        // �ش� ���� �߾� ��ġ�� ����� ��ġ ����
        Vector3 rankPosition = new Vector3(cell.transform.position.x, cell.transform.position.y, 0f);

        // Pepper �������� �̿��Ͽ� ����� ������Ʈ ���� �� GridContainer�� �ڽ����� ����
        GameObject pepperObj = Instantiate(PepperPrefabs, rankPosition, Quaternion.identity, gridContainer);
        pepperObj.name = "Pepper_Lvel" + level;

        // ������ ������Ʈ�� DraggablePepper ������Ʈ �߰�
        DraggablePepper rank = pepperObj.AddComponent<DraggablePepper>();
        // ������� ���� ����
        rank.SetPepperLevel(level);

        // �ش� ���� ����� ���� ����
        cell.SetRank(rank);

        return rank;

    }

    public GridCell FindEmptyCell()                                 // ����ִ� ĭ ã��
    {
        List<GridCell> emptyCells = new List<GridCell>();            // �� ĭ���� ������ ����Ʈ

        for (int x = 0; x < gridWidth; x++)                        // ��� ���� ĭ ��ȸ
        {
            for (int y = 0; y < gridHeight; y++)                   // ��� ���� ĭ ��ȸ
            {
                if (grid[x, y].IsEmpty())                           // �ش� ĭ�� ����ִ� ĭ���� Ȯ��
                {
                    emptyCells.Add(grid[x, y]);
                }
            }
        }

        if (emptyCells.Count == 0)                                  // ��ĭ�� ������ null �� ��ȯ
        {
            return null;
        }

        // ����Ʈ���� �����ϰ� �� ĭ �ϳ� �����Ͽ� ��ȯ
        return emptyCells[Random.Range(0, emptyCells.Count)];

    }

    public bool SpawnNewRank()                                      // �� ����� ����
    {
        GridCell emptyCell = FindEmptyCell();                       // 1. ����ִ� ĭ ã��
        if (emptyCell == null) return false;                        // 2. ����ִ� ĭ�� ������ ���� ����

        // 80% Ȯ���� ���� 1, 20% Ȯ���� ���� 2�� ����� ���� ���� (������ �� �״�� ��Ÿ ����, ���߿� ����)
        int rankLevel = Random.Range(0, 100) < 80 ? 1 : 2;

        CreateRankInCell(emptyCell, rankLevel);                     // 3. �ش� �� ĭ�� ����� ���� �� ����

        return true;
    }

    public GridCell FindClosestCell(Vector3 position)
    {
        // �ش� position�� �����ϴ� ���� �ִ��� Ȯ��
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

        // �����ϴ� ���� ������ ���� ����� ���� ã��
        GridCell closestCell = null;
        float closestDistance = float.MaxValue;

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                // ���� ��ġ�� �� �׸��� ���� �߽� ������ �Ÿ� ���
                float distance = Vector3.Distance(position, grid[x, y].transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestCell = grid[x, y];
                }
            }
        }
        // �ʹ� �ָ� ������ ���� null ��ȯ
        if (closestDistance > cellSize * 2)
        {
            return null;
        }
        return closestCell;
    }

    public void MergeRanks(DraggablePepper draggedRank, DraggablePepper targetRank)
    {
        // ���ο� ���� ���
        int newLevel = targetRank.pepperLevel + 1;

        // �ִ� ���� �ʰ� �� �巡���� ��ũ ���� �� ����
        if (newLevel > maxPepperLevel)
        {
            RemoveRank(draggedRank);
            return;
        }

        //  ���� �߰�
        if (scoreManager != null)
        {
            int points = GetScoreForLevel(newLevel);
            scoreManager.AddScore(points);

            //  ���� �߰� (��: ���� ���� * 4?)
            int coinReward = draggedRank.pepperLevel * 1;
            scoreManager.AddCoin(coinReward);
        }

        //  Ÿ�� ��ũ�� ������ ������Ű�� �巡���� ��ũ ����
        targetRank.SetPepperLevel(newLevel);
        RemoveRank(draggedRank);
    }


    public void RemoveRank(DraggablePepper rank)
    {
        if (rank == null) return;

        // �ش� ��ũ�� ���� ������ ��ũ ���� ���� �� ��ũ ������Ʈ �ı�
        if (rank.currentCell != null)
        {
            rank.currentCell.currentRank = null;
            Destroy(rank.gameObject);
        }
    }

    // ���յ� Pepper�� ������ ���� ������ ��ȯ�ϴ� �Լ�(������ �ϵ� ����� ����)
    private int GetScoreForLevel(int level)
    {
        switch (level)
        {
            case 2: return 50;     // ���� 2�� ���յǸ� 50��
            case 3: return 100;    // ���� 3���� ���յǸ� 100��
            case 4: return 200;    // ���� 4���� ���յǸ� 200��
            case 5: return 400;    // ���� 5���� ���յǸ� 800��
            case 6: return 800;    // ���� 6���� ���յǸ� 800��
            case 7: return 1600;
            case 8: return 3200;
            default: return 0;     // ���ǵ��� ���� ������ ���� ����
        }
    }

    public bool SpawnPepperBySprite(Sprite sprite)
    {
        if (sprite == null) return false;

        DraggablePepper newRank = CreateRankInCellBySprite(sprite);
        if (newRank == null)
        {
            Debug.Log("�� ���� ���� ���� ����");
            return false;
        }

        Debug.Log("�׸��忡 ��������Ʈ ���� ���� �Ϸ�");
        return true;
    }


}