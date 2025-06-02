using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Rsank_Game_UI : MonoBehaviour
{
    public TextMeshProUGUI TimerText;
    public int Timer;

    public TextMeshProUGUI RoundText;
    public int Round;


    public TextMeshProUGUI CoinText;
    public int Coin;

    public TextMeshProUGUI ScoerText;
    public int Scoer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TimerText.text = "생존 시간 : " + Timer.ToString("0.00");
        RoundText.text = "현재 라운드 :  " + Round.ToString("1");
        CoinText.text = "현재 돈 :  " + Coin.ToString("0");
        ScoerText.text = "현재 돈 :  " + Scoer.ToString("0");

    }
}
