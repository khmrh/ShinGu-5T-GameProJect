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


    public GridCell[,] grid;

    void Start()
    {
        InitializeGrid();
        SpawnNewRank();
        SpawnNewRank();
    }

    void Update()
    {
        // Update ���� ���� ����
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

                grid[x, y] = cell;                                   // �迭�� ����
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

    private GridCell FindEmptyCell()                                 // ����ִ� ĭ ã��
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
        // 1�������� �ش� position�� �����ϴ� ���� �ִ��� Ȯ��
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
        // ���� ����� ���ų� ������ �ٸ��� ���� ��ġ�� �ǵ����� ����
        if (draggedRank == null || targetRank == null || draggedRank.rankLevel != targetRank.rankLevel)
        {
            if (draggedRank != null) draggedRank.ReturnToOriginalPosition();
            return;
        }
        // ���ο� ���� ���
        int newLevel = targetRank.rankLevel + 1;
        // �ִ� ���� �ʰ� �� �巡���� ��ũ ���� �� ����
        if (newLevel > maxPepperLevel)
        {
            RemoveRank(draggedRank);
            return;
        }

        // Ÿ�� ��ũ�� ������ ������Ű�� �巡���� ��ũ ����
        targetRank.SetPepperLevel(newLevel);
        RemoveRank(draggedRank);
        // 60% Ȯ���� �� ��ũ ����
        if (Random.Range(0, 100) < 60)
        {
            SpawnNewRank();
        }
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
}