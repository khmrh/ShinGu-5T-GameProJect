using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("�׸��� ��ġ ����")]
    public int gridWidth = 8;                           //���� ĭ ��
    public int gridHeight = 8;                          //���� ĭ ��
    public float cellSize = 1.05f;                      //�� ĭ�� ũ��
    [Header("�׸��� ���� ����")]
    public GameObject cellPrefabs;                      //��ĭ ������
    public Transform gridContainer;                     //�׸��带 ���� �θ� ������Ʈ 

    [Header("��� ��ġ ����")]
    public int maxPepperLevel = 7;                       //�ִ� Pepper ���� 
    [Header("��� ���� ����")]
    public GameObject PepperPrefabs;                      //Pepper ������
    public Sprite[] PepperSprites;                        //�� ������ Pepper �̹���


    public GridCell[,] grid;

    void Start()
    {
        InitializeGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitializeGrid()
    {
        grid = new GridCell[gridWidth, gridHeight];

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector2 position = new Vector2
                (
                    x * cellSize - (gridWidth * cellSize / 2) + cellSize / 2,
                    y * cellSize - (gridHeight * cellSize / 2) + cellSize / 2
                );

                GameObject cellObj = Instantiate(cellPrefabs, position, Quaternion.identity, gridContainer);
                GridCell cell = cellObj.AddComponent<GridCell>();
                cell.Initialize(x, y);
                grid[x, y] = cell;
            }
        }
    }

}
