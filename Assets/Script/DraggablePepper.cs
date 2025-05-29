using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DraggablePepper : MonoBehaviour
{

    public int pepperLevel = 1;                   //����� ����
    public float dragSpeed = 20f;               //�巡�� �� �̵� �ӵ�
    public float snapBackSpeed = 20f;           //����ġ�� ���ư��� �ӵ�

    public bool isDragging = false;             //���� �巡�� ������
    public Vector3 originalPosition;            //���� ��ġ 
    public GridCell currentCell;                //���� ��ġ�� ĭ

    public Camera mainCamera;                   //���� ī�޶�
    public Vector3 dragOffset;                  //�巡�� �� ������ (������)
    public SpriteRenderer spriteRenderer;       //����� �̹��� ������
    public GridManager gridManager;           //���� �޴���

    private void Awake()
    {
        mainCamera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
        gridManager = FindObjectOfType<GridManager>();
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
        else if (transform.position != originalPosition && currentCell != null)
        {
            transform.position = Vector3.Lerp(transform.position, originalPosition, snapBackSpeed * Time.deltaTime);
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
        StartDragging();
    }
    private void OnMouseUp()
    {
        if (!isDragging) return;
        StopDragging();
    }

    public void MoveToCell(GridCell targetCell)     //Ư�� ĭ���� �̵�
    {
        if (currentCell != null)
        {
            currentCell.currentRank = null; //���� ĭ���� ����
        }

        currentCell = targetCell;           //�� ĭ���� �̵� 
        targetCell.currentRank = this;

        originalPosition = new Vector3(targetCell.transform.position.x, targetCell.transform.position.y, 0f);
        transform.position = originalPosition;
    }

    public void ReturnToOriginalPosition()//���� ��ġ�� ���ư��� �Լ� 
    {
        transform.position = originalPosition;
    }

    public void MergeWithCell(GridCell targetCell)
    {
        if (targetCell.currentRank == null || targetCell.currentRank.pepperLevel != pepperLevel) //���� �������� Ȯ��
        {
            ReturnToOriginalPosition(); //���� ��ġ�� ���ư���
            return;
        }

        if (currentCell != null)
        {
            currentCell.currentRank = null; //���� ĭ���� ����
        }

        gridManager.MergeRanks(this, targetCell.currentRank);

    }

    public Vector3 GetMouseWorldPosition()                  //���콺 ���� ��ǥ ���ϱ� 
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
            spriteRenderer.sprite = gridManager.PepperSprites[level - 1];     //������ �´� ��������Ʈ�� ���� 
        }
    }


}