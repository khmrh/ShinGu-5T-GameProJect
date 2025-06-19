using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int currentScore = 0;      // 현재 점수
    public int round = 1;             // 현재 라운드
    public int coin = 0;              // 획득 코인 추적용 (실제 골드는 PlayerData에 저장)

    public Pepper_Game_UI gameUI;     // 게임 중 UI 관리 스크립트 참조
    public GameResultUI resultUI;     // 게임 결과 UI 관리 스크립트 참조

    private bool goalReached = false; // 목표 점수 도달 여부

    void Start()
    {
        // 게임 중 UI가 연결되어 있지 않으면 씬에서 자동으로 찾아 연결
        if (gameUI == null)
            gameUI = FindObjectOfType<Pepper_Game_UI>();

        // 결과 UI도 마찬가지로 자동 연결
        if (resultUI == null)
            resultUI = FindObjectOfType<GameResultUI>();

        UpdateUI();  // 초기 UI 갱신
    }

    // 라운드 시작 시 초기화 함수
    public void StartRound(int newRound)
    {
        round = newRound;
        currentScore = 0;
        goalReached = false;
        UpdateUI();  // UI 갱신
    }

    // 다음 라운드 준비 시 초기화 함수 (중복 가능)
    public void ResetForNextRound(int nextRound)
    {
        round = nextRound;
        currentScore = 0;
        goalReached = false;
        UpdateUI();  // UI 갱신
    }

    // 점수 추가
    public void AddScore(int amount)
    {
        currentScore += amount;
        CheckGoalReached(); // 목표 점수 도달 체크
        UpdateUI();         // UI 갱신
    }

    // 점수 차감 (0 미만 불가)
    public void SubtractScore(int amount)
    {
        currentScore = Mathf.Max(0, currentScore - amount);
        CheckGoalReached();
        UpdateUI();
    }

    // 코인(골드) 추가 함수
    public void AddCoin(int amount)
    {
        coin += amount; // 내부 추적용(선택사항)
        PlayerData.Instance.AddGold(amount);  // 실제 플레이어 골드에 반영
        UpdateUI();  // UI 갱신
    }

    // 목표 점수 도달 여부 체크
    private void CheckGoalReached()
    {
        int target = CalculateTargetScore(round);
        if (currentScore >= target)
        {
            goalReached = true;
        }
    }

    // 목표 점수 도달 여부 반환
    public bool IsGoalReached()
    {
        return goalReached;
    }

    // 라운드에 따른 목표 점수 계산
    public int CalculateTargetScore(int r)
    {
        float baseScore = 12000f;
        float multiplier = Mathf.Pow(1.1f, r - 1);
        int target = Mathf.FloorToInt(baseScore * multiplier);
        return target;
    }

    // UI를 현재 상태에 맞게 갱신하는 함수
    public void UpdateUI()
    {
        if (gameUI != null)
        {
            gameUI.UpdateScore(currentScore);
            // ★ 여기서 UI에 표시할 코인은 실제 플레이어 골드 기준으로 전달
            gameUI.UpdateCoin(PlayerData.Instance.currentGold);
            gameUI.UpdateRound(round);
            gameUI.UpdateTargetScore(CalculateTargetScore(round));
        }
    }

    // UI 갱신만 필요할 때 별도 호출 가능
    public void RefreshUIOnly()
    {
        UpdateUI();
    }
}
