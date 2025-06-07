using UnityEngine;
using TMPro;

public class Pepper_Game_UI : MonoBehaviour
{
    public TextMeshProUGUI TimerText;
    public TextMeshProUGUI RoundText;
    public TextMeshProUGUI CoinText;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI TargetScoreText; // ��ǥ ���� �ؽ�Ʈ (����)

    private float timer;
    private int round;
    private int coin;
    private int score;

    void Update()
    {
        TimerText.text = "���� �ð� : " + timer.ToString("0.00") + "��";
        RoundText.text = "���� ���� : " + round.ToString();
        CoinText.text = "���� �� : " + coin.ToString();
        ScoreText.text = "���� ���� : " + score.ToString();
    }

    public void UpdateTimer(float time) => timer = time;
    public void UpdateRound(int r) => round = r;
    public void UpdateCoin(int c) => coin = c;
    public void UpdateScore(int s) => score = s;
    public void UpdateTargetScore(int target)
    {
        if (TargetScoreText != null)
            TargetScoreText.text = "��ǥ ���� : " + target.ToString();
    }
}
