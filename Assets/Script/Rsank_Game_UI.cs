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
        TimerText.text = "���� �ð� : " + Timer.ToString("0.00");
        RoundText.text = "���� ���� :  " + Round.ToString("1");
        CoinText.text = "���� �� :  " + Coin.ToString("0");
        ScoerText.text = "���� �� :  " + Scoer.ToString("0");

    }
}
