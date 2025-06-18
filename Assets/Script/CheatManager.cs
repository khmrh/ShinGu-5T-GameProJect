using UnityEngine;

public class CheatManager : MonoBehaviour
{
    public ScoreManager scoreManager;
    public GridManager gridManager;
    public GameTimerManager timerManager;

    void Update()
    {
        //  Q 키: 코인 +1000
        if (Input.GetKeyDown(KeyCode.Q))
        {
            scoreManager.AddCoin(1000);
            Debug.Log("치트 사용: 코인 +1000");
        }

        //  W 키: 1단계/2단계 스폰 → 5단계/6단계로 변경
        if (Input.GetKeyDown(KeyCode.W))
        {
            SpawnHighLevelPepper();
            Debug.Log("치트 사용: 5~6단계 페퍼 스폰");
        }

        //  E 키: 남은 시간을 3초로 설정
        if (Input.GetKeyDown(KeyCode.E))
        {
            timerManager.SetTime(3f);  // 타이머 스크립트에 SetTime 함수가 필요
            Debug.Log("치트 사용: 남은 시간 3초");
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            scoreManager.AddScore(10000);
            Debug.Log("치트 사용: 점수 +10000");
        }
    }

    void SpawnHighLevelPepper()
    {
        GridCell emptyCell = gridManager.FindEmptyCell();
        if (emptyCell != null)
        {
            int level = Random.Range(0, 100) < 50 ? 5 : 6;
            gridManager.CreateRankInCell(emptyCell, level);
        }
    }
}
