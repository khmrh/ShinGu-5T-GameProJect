using UnityEngine;

public class PepperManager : MonoBehaviour
{
    [Header("필수 참조")]
    public GridManager gridManager;          // 인스펙터에서 연결
    public GameObject PepperPrefabs;         // 생성할 계급장 프리팹
    public Transform gridContainer;          // 계급장 오브젝트의 부모

    private void Awake()
    {
        // 혹시 연결 안 되어 있다면 자동 할당 (보조용)
        if (gridManager == null)
        {
            gridManager = FindObjectOfType<GridManager>();
        }
    }

    /// <summary>
    /// 클릭한 계급장 오브젝트를 복제하여 빈 셀에 생성한다.
    /// </summary>
    /// <param name="original">복제할 원본 계급장</param>
    public void TryClonePepper(DraggablePepper original)
    {
        // 1. 빈 셀 찾기
        GridCell targetCell = gridManager.FindEmptyCell(); // ✅ 인스턴스를 통해 접근
        if (targetCell == null)
        {
            Debug.Log("빈 셀이 없습니다. 복제 실패");
            return;
        }

        // 2. 프리팹으로 오브젝트 생성
        Vector3 spawnPos = targetCell.transform.position;
        GameObject clone = Instantiate(PepperPrefabs, spawnPos, Quaternion.identity, gridContainer);
        clone.name = "Cloned_Pepper";

        // 3. 스크립트 추가 및 레벨 설정
        DraggablePepper clonedRank = clone.AddComponent<DraggablePepper>();
        clonedRank.SetPepperLevel(original.pepperLevel);  // 원본과 동일한 레벨로 복제

        // 4. 셀에 배치
        targetCell.SetRank(clonedRank);
    }
}
