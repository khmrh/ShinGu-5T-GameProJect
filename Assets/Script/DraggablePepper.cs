using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DraggablePepper : MonoBehaviour
{
    public int pepperLevel = 1;                                        //계급장 레벨
    public float dragSpeed = 100f;                                    //드래그 시 이동 속도
    public float snapBackSpeed = 10f;                                //원위치로 돌아가는 속도
    public float moveToCellSpeed = 20f;                               // 셀 이동 속도

    public bool isDragging = false;                                  //현재 드래그 중인지
    public bool isReturning = false;                                // 현재 돌아가는 중인지
    public bool isMovingToCell = false;                             // 현재 셀로 이동 중인지
    public GridCell targetMoveCell;                                 // 이동 목표 셀
    public float moveProgress = 0f;                                 // 이동 진행도
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
            Vector3 targetPosition = GetMouseWorldPosition() + dragOffset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, dragSpeed * Time.deltaTime);
        }
        else if (isReturning)
        {
            pepperCollider.enabled = false; // 이동 중 클릭 방지
            transform.position = Vector3.Lerp(transform.position, originalPosition, snapBackSpeed * Time.deltaTime);
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
            transform.position = Vector3.Lerp(transform.position, targetMoveCell.transform.position, Time.deltaTime * moveToCellSpeed);

            if (Vector3.Distance(transform.position, targetMoveCell.transform.position) < 0.1f) // 충분히 가까워지면 멈춤
            {
                transform.position = targetMoveCell.transform.position; // 최종 위치 정확히 설정
                isMovingToCell = false;
                targetMoveCell = null;
                moveProgress = 0f;
                pepperCollider.enabled = true; // 돌아오면 클릭 활성화
            }
        }
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
        spriteRenderer.sortingOrder = 1;
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

    private void OnMouseDown()
    {
        // 드래그 중이거나 이동/복귀 중이 아닐 때만 드래그 시작
        if (!isDragging && !isReturning && !isMovingToCell)
        {
            StartDragging();
        }
    }

    private void OnMouseUp()
    {
        if (!isDragging) return;
        StopDragging();
    }

    public void MoveToCell(GridCell targetCell)    //특정 칸으로 이동
    {
        if (targetCell == null) return; //targetCell이 null이면 즉시 반환

        if (currentCell != null)
        {
            currentCell.currentRank = null; //기존 칸에서 제거
        }

        targetMoveCell = targetCell;
        isMovingToCell = true;
        moveProgress = 0f;
        targetCell.currentRank = this;
        currentCell = targetCell;
        originalPosition = new Vector3(targetCell.transform.position.x, targetCell.transform.position.y, 0f);
    }

    /*
    구버전 MoveToCell (Lerp) 함수

     public void MoveToCell(GridCell targetCell)    //특정 칸으로 이동
    {
        if (currentCell != null)
        {
            currentCell.currentRank = null; //기존 칸에서 제거
        }

        targetMoveCell = targetCell;
        isMovingToCell = true;
        moveProgress = 0f;
        targetCell.currentRank = this;
        currentCell = targetCell;
        originalPosition = new Vector3(targetCell.transform.position.x, targetCell.transform.position.y, 0f); 
    }

    (구버전) 순간이동
    public void MoveToCell(GridCell targetCell)    //특정 칸으로 이동
    {
        if (currentCell != null)
        {
            currentCell.currentRank = null; //기존 칸에서 제거
        }
        currentCell = targetCell;           //새 칸으로 이동
        targetCell.currentRank = this;
        originalPosition = new Vector3(targetCell.transform.position.x, targetCell.transform.position.y, 0f);
        transform.position = originalPosition;
    }
    */

    public void ReturnToOriginalPosition() //원래 위치로 돌아가는 함수
    {
        isReturning = true;
    }

    /*
    (구버전) 원래 위치로 돌아가는 함수 
    public void ReturnToOriginalPosition()
    {
        transform.position = originalPosition;
    }
    */

    public void MergeWithCell(GridCell targetCell)
    {
        if (targetCell.currentRank == null || targetCell.currentRank.pepperLevel != pepperLevel) //같은 레벨인지 확인
        {
            ReturnToOriginalPosition(); //원래 위치로 돌아가기
            return;
        }

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