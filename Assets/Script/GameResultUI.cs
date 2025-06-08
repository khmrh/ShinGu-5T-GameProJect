using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using static Unity.Collections.Unicode;

public class GameResultUI : MonoBehaviour
{
    public GameObject successPanel;
    public GameObject failPanel;

    public TMP_Text successTargetScoreText;
    public TMP_Text successCurrentScoreText;
    public TMP_Text successCoinText;
    public TMP_Text successRoundText;

    public TMP_Text failTargetScoreText;
    public TMP_Text failCurrentScoreText;
    public TMP_Text failCoinText;
    public TMP_Text failRoundText;

    public Button nextRoundButton;  //  버튼 연결

    public void HideAll()
    {
        successPanel.SetActive(false);
        failPanel.SetActive(false);
    }

    public void ShowSuccess(int targetScore, int currentScore, int coin, int round)
    {
        HideAll();
        successPanel.SetActive(true);
        successTargetScoreText.text = $"목표 점수: {targetScore}";
        successCurrentScoreText.text = $"현재 점수: {currentScore}";
        successCoinText.text = $"코인: {coin}";
        successRoundText.text = $"라운드: {round}";

        nextRoundButton?.gameObject.SetActive(true); // 버튼 보이기
    }

    public void ShowFail(int targetScore, int currentScore, int coin, int round)
    {
        HideAll();
        failPanel.SetActive(true);
        failTargetScoreText.text = $"목표 점수: {targetScore}";
        failCurrentScoreText.text = $"현재 점수: {currentScore}";
        failCoinText.text = $"코인: {coin}";
        failRoundText.text = $"라운드: {round}";

        nextRoundButton?.gameObject.SetActive(false); // 실패 시 버튼 숨김
    }
}
