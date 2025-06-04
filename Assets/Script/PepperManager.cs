using UnityEngine;

public class PepperManager : MonoBehaviour
{
    [Header("필수 참조")]
    public GridManager gridManager;          // 인스펙터에서 연결
    public GameObject PepperPrefabs;         // 생성할 계급장 프리팹
    public Transform gridContainer;          // 계급장 오브젝트의 부모

    [Header("프리팹 생성 제한")]
    public int maxPepperCount = 50;          // 최대 생성 가능한 수
    private int currentPepperCount = 0;      // 현재까지 생성된 수

    private void Awake()
    {
        if (gridManager == null)
        {
            gridManager = FindObjectOfType<GridManager>();
        }
    }

    /// <summary>
    /// 클릭한 계급장 오브젝트를 복제하여 빈 셀에 생성한다.
    /// </summary>
    /// <param name="original">복제할 원본 계급장</param>
    public void TryCloneAndReplace(DraggablePepper original)
    {
        // 1. 빈 셀 탐색
        GridCell targetCell = gridManager.FindEmptyCell();
        if (targetCell == null)
        {
            Debug.Log("빈 셀이 없습니다. 복제 불가");
            return;
        }

        // 2. 복제 생성
        Vector3 spawnPos = targetCell.transform.position;
        GameObject clone = Instantiate(PepperPrefabs, spawnPos, Quaternion.identity, gridContainer);
        clone.name = "ClonedPepper";

        DraggablePepper cloned = clone.AddComponent<DraggablePepper>();
        cloned.SetPepperLevel(original.pepperLevel);
        targetCell.SetRank(cloned);

        // 3. 원본 제거 (카운트 감소 포함)
        if (original.currentCell != null)
        {
            original.currentCell.currentRank = null;
        }

        Destroy(original.gameObject);
        DecreasePepperCount();  // 개수 관리까지 하면 완벽
    }


    /// <summary>
    /// 외부에서 계급장 제거 시 카운트 감소
    /// </summary>
    public void DecreasePepperCount()
    {
        currentPepperCount = Mathf.Max(0, currentPepperCount - 1);
    }
}

