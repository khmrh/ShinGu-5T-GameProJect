using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Rsank_Game_UI : MonoBehaviour
{
    public TextMeshProUGUI TimerText;
    public TextMeshProUGUI RoundText;
    public TextMeshProUGUI CoinText;
    public TextMeshProUGUI ScoreText;

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

    // 외부에서 값을 갱신하는 public 함수
    public void UpdateTimer(float time) => timer = time;
    public void UpdateRound(int r) => round = r;
    public void UpdateCoin(int c) => coin = c;
    public void UpdateScore(int s) => score = s;
}