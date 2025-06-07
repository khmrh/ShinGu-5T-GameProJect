using UnityEngine;
using TMPro;

public class Pepper_Game_UI : MonoBehaviour
{
    public TextMeshProUGUI TimerText;
    public TextMeshProUGUI RoundText;
    public TextMeshProUGUI CoinText;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI TargetScoreText; // 목표 점수 텍스트 (선택)

    private float timer;
    private int round;
    private int coin;
    private int score;

    void Update()
    {
        TimerText.text = "남은 시간 : " + timer.ToString("0.00") + "초";
        RoundText.text = "현재 라운드 : " + round.ToString();
        CoinText.text = "현재 돈 : " + coin.ToString();
        ScoreText.text = "현재 점수 : " + score.ToString();
    }

    public void UpdateTimer(float time) => timer = time;
    public void UpdateRound(int r) => round = r;
    public void UpdateCoin(int c) => coin = c;
    public void UpdateScore(int s) => score = s;
    public void UpdateTargetScore(int target)
    {
        if (TargetScoreText != null)
            TargetScoreText.text = "목표 점수 : " + target.ToString();
    }
}
