using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("그리드 수치 조정")]
    public int gridWidth = 8;                           //가로 칸 수
    public int gridHeight = 8;                          //세로 칸 수
    public float cellSize = 1.05f;                      //각 칸의 크기
    [Header("그리드 형태 구성")]
    public GameObject cellPrefabs;                      //빈칸 프리팹
    public Transform gridContainer;                     //그리드를 담을 부모 오브젝트 

    [Header("재료 수치 조정")]
    public int maxPepperLevel = 7;                       //최대 Pepper 레벨 
    [Header("재료 형태 구성")]
    public GameObject PepperPrefabs;                      //Pepper 프리팹
    public Sprite[] PepperSprites;                        //각 레벨별 Pepper 이미지


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
