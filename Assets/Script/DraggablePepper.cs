using UnityEngine;

public class DraggablePepper : MonoBehaviour
{
    public int pepperLevel = 1;                                        //계급장 레벨
    public float dragSpeed = 25f;                                    //선형 보간 이동 속도

    public bool isDragging = false;                                  //현재 드래그 중인지
    public bool isReturning = false;                                // 현재 돌아가는 중인지
    public bool isMovingToCell = false;                             // 현재 셀로 이동 중인지
    public GridCell targetMoveCell;                                 // 이동 목표 셀
    public Vector3 originalPosition;                                 //원래 위치
    public GridCell currentCell;                                     //현재 위치한 칸

    public Camera mainCamera;                                        //메인 카메라
    public Vector3 dragOffset;                                       //드래그 시 오프셋 (보정값)
    public SpriteRenderer spriteRenderer;                            //계급장 이미지 렌더러
    public GridManager gridManager;                                   //게임 메니저
    public Collider2D pepperCollider;                                // 콜라이더 참조

    private void Awake()
    {
        mainCamera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
        gridManager = FindObjectOfType<GridManager>();
        pepperCollider = GetComponent<Collider2D>();
        if (pepperCollider == null)
        {
            Debug.Log("이 오브젝트에 Collider2D 없음.");
        }
    }

    void Start()
    {
        originalPosition = transform.position;
    }

    void Update()
    {

        if (isDragging)
        {
            transform.position = GetMouseWorldPosition() + dragOffset;
        }
        else if (isReturning)
        {
            pepperCollider.enabled = false; // 이동 중 클릭 방지
            transform.position = Vector3.Lerp(transform.position, originalPosition, dragSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, originalPosition) < 0.05f) // 충분히 가까워지면 멈춤
            {
                transform.position = originalPosition; // 최종 위치 정확히 설정
                isReturning = false;
                pepperCollider.enabled = true; // 돌아오면 클릭 활성화
            }
        }
        else if (isMovingToCell && targetMoveCell != null)
        {
            pepperCollider.enabled = false; // 이동 중 클릭 방지
            transform.position = Vector3.Lerp(transform.position, targetMoveCell.transform.position, Time.deltaTime * dragSpeed);

            if (Vector3.Distance(transform.position, targetMoveCell.transform.position) < 0.1f) // 충분히 가까워지면 멈춤
            {
                transform.position = targetMoveCell.transform.position; // 최종 위치 정확히 설정
                transform.position = new Vector3(transform.position.x, transform.position.y, 0f); // Z좌표 변경
                isMovingToCell = false;
                targetMoveCell = null;
                pepperCollider.enabled = true; // 돌아오면 클릭 활성화
            }
        }
    }
    private void OnMouseDown()
    {
        FindObjectOfType<PepperManager>().TryClonePepper(this);
    }
    private void OnMouseUp()
    {
        if (!isDragging) return;
        StopDragging();
    }
 


    void StartDragging()
    {
        isDragging = true;
        dragOffset = transform.position - GetMouseWorldPosition();
        spriteRenderer.sortingOrder = 10;
    }

    void StopDragging()
    {
        isDragging = false;
        spriteRenderer.sortingOrder = 5;
        GridCell targetCell = gridManager.FindClosestCell(transform.position);
        if (targetCell != null)
        {
            if (targetCell.currentRank == null)
            {
                MoveToCell(targetCell);
            }
            else if (targetCell.currentRank != this && targetCell.currentRank.pepperLevel == pepperLevel)
            {
                MergeWithCell(targetCell);
            }
            else
            {
                ReturnToOriginalPosition();
            }
        }
        else
        {
            ReturnToOriginalPosition();
        }
    }

    public void MoveToCell(GridCell targetCell)    //특정 칸으로 이동
    {
        if (currentCell != null)
        {
            currentCell.currentRank = null; //기존 칸에서 제거
        }

        isMovingToCell = true;
        targetMoveCell = targetCell;
        targetCell.currentRank = this;
        currentCell = targetCell;
        originalPosition = new Vector3(targetCell.transform.position.x, targetCell.transform.position.y, 0f);
    }

    public void ReturnToOriginalPosition() //원래 위치로 돌아가는 함수
    {
        isReturning = true;
    }

    public void MergeWithCell(GridCell targetCell)
    {
        if (currentCell != null)
        {
            currentCell.currentRank = null; //기존 칸에서 제거
        }

        gridManager.MergeRanks(this, targetCell.currentRank);
    }

    public Vector3 GetMouseWorldPosition()                                        //마우스 월드 좌표 구하기
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -mainCamera.transform.position.z;
        return mainCamera.ScreenToWorldPoint(mousePos);
    }

    public void SetPepperLevel(int level)
    {
        pepperLevel = level;

        if (gridManager != null && gridManager.PepperSprites.Length > level - 1)
        {
            spriteRenderer.sprite = gridManager.PepperSprites[level - 1];    //레벨에 맞는 스프라이트로 변경
        }
    }

  

}