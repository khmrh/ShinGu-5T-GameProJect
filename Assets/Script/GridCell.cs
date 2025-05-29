using UnityEngine;



public class GridCell : MonoBehaviour
{
    public int x, y;                  //그리드에서의 위치(좌표)
    public DraggablePepper currentPepper;     //현재 칸에 있는 계급장
    public SpriteRenderer cellRenderers;  //칸의 이미지 렌더러

    public void Initialize(int gridX, int gridY)
    {
        x = gridX;
        y = gridY;
        name = "Cell_" + x + "_" + y;         //이름 설정
    }


    void Start()
    {

    }


    void Update()
    {

    }
}