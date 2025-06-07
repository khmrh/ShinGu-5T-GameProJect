using UnityEngine;

public class GameResultUI : MonoBehaviour
{
    public GameObject successPanel;  // 성공 시 표시할 패널
    public GameObject failPanel;     // 실패 시 표시할 패널

    void Start()
    {
        HideAll(); // 시작 시 패널들 숨기기
    }

    public void ShowSuccess()
    {
        HideAll();
        successPanel.SetActive(true);
    }

    public void ShowFail()
    {
        HideAll();
        failPanel.SetActive(true);
    }

    public void HideAll()
    {
        successPanel?.SetActive(false);
        failPanel?.SetActive(false);
    }
}
