using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToMainMenuOnClick : MonoBehaviour
{
    public string mainMenuSceneName = "MainMenu"; // 씬 이름 설정

    private void OnMouseDown()
    {
        Debug.Log("[Debug] 메인 메뉴 씬으로 이동");
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
