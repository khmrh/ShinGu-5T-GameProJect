using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToMainMenuOnClick : MonoBehaviour
{
    public string mainMenuSceneName = "MainMenu"; // �� �̸� ����

    private void OnMouseDown()
    {
        Debug.Log("[Debug] ���� �޴� ������ �̵�");
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
