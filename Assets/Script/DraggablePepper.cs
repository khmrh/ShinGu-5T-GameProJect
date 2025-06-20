﻿using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DraggablePepper : MonoBehaviour
{
    public int pepperLevel = 1;                                        // 계급장 레벨
    public float dragSpeed = 25f;                                      // 선형 보간 이동 속도

    public bool isDragging = false;                                    // 현재 드래그 중인지
    public bool isReturning = false;                                   // 현재 돌아가는 중인지
    public bool isMovingToCell = false;                                // 현재 셀로 이동 중인지
    public GridCell targetMoveCell;                                    // 이동 목표 셀
    public Vector3 originalPosition;                                   // 원래 위치
    public GridCell currentCell;                                       // 현재 위치한 칸

    public Camera mainCamera;                                          // 메인 카메라
    public Vector3 dragOffset;                                         // 드래그 시 오프셋 (보정값)
    public SpriteRenderer spriteRenderer;                              // 계급장 이미지 렌더러
    public GridManager gridManager;                                    // 게임 매니저
    public Collider2D pepperCollider;                                  // 콜라이더 참조

    public PepperManager pepperManager;                                // PepperManager 연결

    public bool isInteractable = true;                                 // 상호작용 가능 여부

    private void Awake()
    {
        mainCamera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
        gridManager = FindObjectOfType<GridManager>();
        pepperCollider = GetComponent<Collider2D>();

        if (pepperCollider == null)
            Debug.LogWarning("이 오브젝트에 Collider2D 없음.");

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetPepperLevel(int level)
    {
        pepperLevel = level;

        if (gridManager != null && gridManager.PepperSprites.Length > level - 1)
        {
            spriteRenderer.sprite = gridManager.PepperSprites[level - 1];    // 레벨에 맞는 스프라이트로 변경
        }
    }

    // 특정 스프라이트로 변경하는 메서드
    public void SetSprite(Sprite sprite)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = sprite;
        }
        else
        {
            Debug.LogWarning("SpriteRenderer가 없습니다.");
        }
    }

    void Start()
    {
        // 최초 생성 위치 저장 (Z 고정)
        originalPosition = new Vector3(transform.position.x, transform.position.y, 0.9f);
        transform.position = originalPosition; // Z값 고정
    }

    void Update()
    {
        // 라운드가 종료된 경우 드래그 상태를 강제로 해제하고 업데이트 루프를 빠져나감
        if (pepperManager != null && !pepperManager.isRoundActive)
        {
            if (isDragging)
            {
                // 드래그 상태 강제 해제 (기존 방식)
                isDragging = false;
                isReturning = true;
                spriteRenderer.sortingOrder = 5;
                pepperCollider.enabled = true;
            }
            return; // 이후 로직 실행하지 않음
        }

        // 드래그 중일 때는 마우스 위치에 페퍼 위치를 따라가도록 함
        if (isDragging)
        {
            transform.position = GetMouseWorldPosition() + dragOffset;
        }
        // 원래 위치로 돌아가는 중일 때 보간 이동 처리
        else if (isReturning)
        {
            pepperCollider.enabled = false; // 이동 중 클릭 방지
            transform.position = Vector3.Lerp(transform.position, originalPosition, dragSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, originalPosition) < 0.05f)
            {
                // 정확한 위치로 스냅, Z값 포함
                transform.position = new Vector3(originalPosition.x, originalPosition.y, 0.9f);
                isReturning = false;
                pepperCollider.enabled = true;
            }
        }
        // 셀로 이동 중일 때 보간 이동 처리
        else if (isMovingToCell && targetMoveCell != null)
        {
            pepperCollider.enabled = false;
            transform.position = Vector3.Lerp(transform.position, targetMoveCell.transform.position, Time.deltaTime * dragSpeed);

            if (Vector3.Distance(transform.position, targetMoveCell.transform.position) < 0.1f)
            {
                // 최종 위치 고정 시에도 Z = 0.9f 적용
                transform.position = new Vector3(targetMoveCell.transform.position.x, targetMoveCell.transform.position.y, 0.9f);
                isMovingToCell = false;
                targetMoveCell = null;
                pepperCollider.enabled = true;
            }
        }
    }

    private void OnMouseDown()
    {
        // 상호작용 불가하거나 라운드가 끝난 경우 클릭 무시
        if (!isInteractable || (pepperManager != null && !pepperManager.isRoundActive))
            return;

        // 부모가 그리드 컨테이너가 아니면 무시 (필요시 수정 가능)
        if (transform.parent != gridManager.gridContainer.transform)
            return;

        // 현재 셀이 없으면 오브젝트 파괴 (예외 처리)
        if (currentCell == null)
        {
            Destroy(gameObject);
        }
        else
        {
            StartDragging();
        }
    }

    private void OnMouseUp()
    {
        // 라운드가 끝났을 때 드래그 상태 해제 처리
        if (pepperManager != null && !pepperManager.isRoundActive)
        {
            if (isDragging)
            {
                isDragging = false;
                isReturning = true;
                spriteRenderer.sortingOrder = 5;
                pepperCollider.enabled = true;
            }
            return;
        }

        if (!isDragging)
            return;

        StopDragging();
    }

    void StartDragging()
    {
        isDragging = true;
        dragOffset = transform.position - GetMouseWorldPosition();
        spriteRenderer.sortingOrder = 10; // 드래그 중 우선순위 올리기
    }

    void StopDragging()
    {
        if (!isInteractable || (pepperManager != null && !pepperManager.isRoundActive))
            return;

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

    public void MoveToCell(GridCell targetCell)
    {
        if (currentCell != null)
            currentCell.currentRank = null;

        isMovingToCell = true;
        targetMoveCell = targetCell;
        targetCell.currentRank = this;
        currentCell = targetCell;

        // 복귀 위치에도 Z 고정
        originalPosition = new Vector3(targetCell.transform.position.x, targetCell.transform.position.y, 0.9f);
    }

    public void ReturnToOriginalPosition()
    {
        isReturning = true;
    }

    public void MergeWithCell(GridCell targetCell)
    {
        if (!isInteractable || (pepperManager != null && !pepperManager.isRoundActive))
            return;

        if (currentCell != null)
            currentCell.currentRank = null;

        gridManager.MergeRanks(this, targetCell.currentRank);
    }

    public Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -mainCamera.transform.position.z;
        return mainCamera.ScreenToWorldPoint(mousePos);
    }

    /// <summary>
    /// 라운드 종료 시 즉시 원래 자리로 되돌리는 함수
    /// </summary>
    public void ForceReturnToOriginalPosition()
    {
        isDragging = false;
        isReturning = false;
        isMovingToCell = false;
        pepperCollider.enabled = true;
        spriteRenderer.sortingOrder = 5;
        transform.position = new Vector3(originalPosition.x, originalPosition.y, 0.9f);
    }
}
