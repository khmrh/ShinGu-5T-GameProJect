using UnityEngine;
using UnityEngine.UI;

public class HelpPanelController : MonoBehaviour
{
    [Header("도움말 페이지들 (순서대로)")]
    public GameObject[] pages;
    [Header("도움말 팝업 부모 (전체 판넬)")]
    public GameObject helpPopupRoot;


    private int currentPage = -1;

    // 처음 도움말 열기
    public void OpenHelp()
    {
        if (helpPopupRoot != null)
            helpPopupRoot.SetActive(true); 

        currentPage = 0;
        UpdatePages();
    }


    //   다음 페이지
    public void NextPage()
    {
        if (currentPage < pages.Length - 1)
        {
            currentPage++;
            UpdatePages();
        }
    }

    //  이전 페이지
    public void PrevPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            UpdatePages();
        }
    }

    //  도움말 닫기
    public void CloseHelp()
    {
        currentPage = -1;
        foreach (var p in pages)
            p.SetActive(false);

        if (helpPopupRoot != null)
            helpPopupRoot.SetActive(false); //전체 팝업 끄기
    }


    //  페이지 표시 갱신
    private void UpdatePages()
    {
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(i == currentPage);
        }
    }
}
