using UnityEngine;



public class GridCell : MonoBehaviour
{
    public int x, y;                  //�׸��忡���� ��ġ(��ǥ)
    public DraggablePepper currentPepper;     //���� ĭ�� �ִ� �����
    public SpriteRenderer cellRenderers;  //ĭ�� �̹��� ������

    public void Initialize(int gridX, int gridY)
    {
        x = gridX;
        y = gridY;
        name = "Cell_" + x + "_" + y;         //�̸� ����
    }


    void Start()
    {

    }


    void Update()
    {

    }
}