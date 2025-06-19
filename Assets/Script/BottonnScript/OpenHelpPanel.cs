using UnityEngine;
using UnityEngine.UI;

public class HelpPanelController : MonoBehaviour
{
    [Header("���� �������� (�������)")]
    public GameObject[] pages;
    [Header("���� �˾� �θ� (��ü �ǳ�)")]
    public GameObject helpPopupRoot;


    private int currentPage = -1;

    // ó�� ���� ����
    public void OpenHelp()
    {
        if (helpPopupRoot != null)
            helpPopupRoot.SetActive(true); 

        currentPage = 0;
        UpdatePages();
    }


    //   ���� ������
    public void NextPage()
    {
        if (currentPage < pages.Length - 1)
        {
            currentPage++;
            UpdatePages();
        }
    }

    //  ���� ������
    public void PrevPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            UpdatePages();
        }
    }

    //  ���� �ݱ�
    public void CloseHelp()
    {
        currentPage = -1;
        foreach (var p in pages)
            p.SetActive(false);

        if (helpPopupRoot != null)
            helpPopupRoot.SetActive(false); //��ü �˾� ����
    }


    //  ������ ǥ�� ����
    private void UpdatePages()
    {
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(i == currentPage);
        }
    }
}
