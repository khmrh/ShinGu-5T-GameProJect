﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PepperManager : MonoBehaviour
{
    [Header("설정")]
    public GameObject pepperPrefab;              // 페퍼 프리팹
    public Transform spawnArea;                  // 밖 페퍼가 생성될 범위 오브젝트
    public float spawnOffsetZ = -3f;             // Z축 위치 보정 (앞쪽으로)

    public GridManager gridManager;              // 그리드 매니저 연결

    public int maxOutsidePepperCount = 20;       // 필드에 유지할 최대 밖 페퍼 수

    private List<GameObject> spawnedOutsidePeppers = new List<GameObject>();  // 현재 존재하는 밖 페퍼들 리스트

    public GameObject spawnerObject;

    [HideInInspector]
    public bool isRoundActive = true;  // 라운드가 진행 중인지 상태

    private Coroutine spawnRoutine = null; // 코루틴 제어용 변수

    void Start()
    {
        // 자동으로 페퍼 생성 코루틴 실행하지 않음
        // StartCoroutine(ContinuousOutsidePepperSpawner());  // 기존 자동 시작 부분 주석 처리 또는 삭제
    }

    // 외부에서 호출하여 페퍼 생성 코루틴 시작
    public void StartSpawning()
    {
        if (spawnRoutine == null)
            spawnRoutine = StartCoroutine(ContinuousOutsidePepperSpawner());
    }

    // 1초에 1개씩 밖 페퍼를 생성하며, 최대 개수 유지
    private IEnumerator ContinuousOutsidePepperSpawner()
    {
        yield return new WaitForSeconds(1f);  // 초기 대기

        while (true)
        {
            if (isRoundActive && spawnedOutsidePeppers.Count < maxOutsidePepperCount)
            {
                SpawnOneOutsidePepper();  // 1개 생성
            }

            yield return new WaitForSeconds(1f);  // 1초마다 반복
        }
    }

    // 밖 페퍼 1개 생성
    private void SpawnOneOutsidePepper()
    {
        // 재료 생성 위치 설정
        Vector3 spawnPos = GetSpawnPosition(spawnArea);
        GameObject pepper = Instantiate(pepperPrefab, spawnPos, Quaternion.identity);

        // 이동 및 참조 연결
        PepperMovement pm = pepper.GetComponent<PepperMovement>();
        pm.SetSpawnAreaFromObject(spawnerObject);
        pm.pepperManager = this;

        // OutsidePepper 스크립트 연결
        OutsidePepper outside = pepper.GetComponent<OutsidePepper>();
        if (outside == null)
            outside = pepper.AddComponent<OutsidePepper>();
        outside.pepperManager = this;

        // ✅ 등장 확률에 따라 최종 등급 결정
        int level = PassiveManager.Instance.DetermineSpawnLevel();  // 여기에 선언!

        // 등급에 맞는 스프라이트 적용
        Sprite sprite = gridManager.PepperSprites[Mathf.Clamp(level - 1, 0, gridManager.PepperSprites.Length - 1)];
        outside.SetPepper(level, sprite);

        spawnedOutsidePeppers.Add(pepper);
    }

    // 밖 페퍼가 사라질 때 호출 → 리스트에서 제거
    public void OnOutsidePepperDestroyed(GameObject outsidePepper)
    {
        if (spawnedOutsidePeppers.Contains(outsidePepper))
        {
            spawnedOutsidePeppers.Remove(outsidePepper);
        }

        Debug.Log("Outside Pepper Destroyed: " + outsidePepper.name);
    }

    // 밖 페퍼 클릭 시 그리드에 생성 요청 (라운드 진행중일 때만)
    public void HandleOutsidePepperClicked(int level, Sprite sprite)
    {
        if (!isRoundActive)
        {
            Debug.Log("라운드 종료, 밖 페퍼 클릭 무시");
            return;
        }

        bool success = gridManager.SpawnPepperBySprite(level, sprite);

        if (!success)
        {
            Debug.LogWarning("그리드에 페퍼 생성 실패");
        }
        else
        {
            Debug.Log("그리드에 페퍼 생성 성공");
        }
    }

    // 빈 셀에 특정 레벨 페퍼 생성 (직접 호출용)
    public void SpawnPepperInGrid(int level)
    {
        if (gridManager == null)
        {
            Debug.LogError("GridManager가 할당되지 않았습니다.");
            return;
        }

        GridCell emptyCell = gridManager.FindEmptyCell();

        if (emptyCell == null)
        {
            Debug.LogWarning("빈 셀이 없습니다.");
            return;
        }

        DraggablePepper newPepper = gridManager.CreateRankInCell(emptyCell, level);

        if (newPepper != null)
        {
            newPepper.transform.SetParent(emptyCell.transform);
            newPepper.transform.localPosition = new Vector3(0f, 0f, 0.9f);

            newPepper.pepperManager = this; // 🔥 여기 반드시 추가!
        }
    }

    // 밖 페퍼 생성 위치 결정 (랜덤한 X, Y, Z는 앞쪽)
    private Vector3 GetSpawnPosition(Transform area)
    {
        Vector3 center = area.position;
        Vector3 scale = area.localScale;

        float randomX = Random.Range(center.x - scale.x / 2f, center.x + scale.x / 2f);
        float randomY = Random.Range(center.y - scale.y / 2f, center.y + scale.y / 2f);
        float z = center.z + spawnOffsetZ;

        return new Vector3(randomX, randomY, z);
    }

    public void OnRoundEnd()
    {
        isRoundActive = false;

        // 밖 페퍼 즉시 원위치 처리
        foreach (var pepper in spawnedOutsidePeppers)
        {
            var draggable = pepper.GetComponent<DraggablePepper>();
            if (draggable != null && draggable.isDragging)
            {
                draggable.ForceReturnToOriginalPosition();
            }
        }

        // 그리드 안 페퍼 즉시 원위치 처리
        if (gridManager != null)
        {
            for (int x = 0; x < gridManager.gridWidth; x++)
            {
                for (int y = 0; y < gridManager.gridHeight; y++)
                {
                    var cell = gridManager.grid[x, y];
                    if (cell != null && cell.currentRank != null)
                    {
                        var draggable = cell.currentRank;
                        if (draggable.isDragging)
                        {
                            draggable.ForceReturnToOriginalPosition();
                        }
                    }
                }
            }
        }

        // 그 밖에 라운드 종료 시 필요한 추가 처리...
    }


    public void DisableGridPepperInteraction()
    {
        for (int x = 0; x < gridManager.gridWidth; x++)
        {
            for (int y = 0; y < gridManager.gridHeight; y++)
            {
                var cell = gridManager.grid[x, y];
                if (cell != null && cell.currentRank != null)
                {
                    cell.currentRank.isInteractable = false;
                }
            }
        }
    }
}
