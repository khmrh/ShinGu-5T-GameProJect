using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuButtons : MonoBehaviour
{
    [Header("메인 버튼들")]
    public GameObject startButton;
    public GameObject exitButton;
    public GameObject recordButton;
    public GameObject settingsButton;

    [Header("팝업창들")]
    //public GameObject recordPopup;
    public GameObject settingsPopup;

    [Header("닫기 버튼들")]
    //public GameObject closeRecordButton;

    [Header("이동할 씬 이름")]
    public string gameSceneName = "PepperMergeGame";

    [Header("게임 종료 확인 팝업")]
    public GameObject exitConfirmPopup;
    public GameObject cancelExitButton;
    public GameObject confirmExitButton;

    private void Start()
    {
        startButton.GetComponent<Button>().onClick.AddListener(StartGame);
        exitButton.GetComponent<Button>().onClick.AddListener(OpenExitConfirmPopup);
        //recordButton.GetComponent<Button>().onClick.AddListener(ToggleRecordPopup);
        settingsButton.GetComponent<Button>().onClick.AddListener(ToggleSettingsPopup);

        //closeRecordButton.GetComponent<Button>().onClick.AddListener(CloseRecordPopup);

        cancelExitButton.GetComponent<Button>().onClick.AddListener(CloseExitConfirmPopup);
        confirmExitButton.GetComponent<Button>().onClick.AddListener(ExitGame);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void ExitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

   /* private void ToggleRecordPopup()
    {
        if (recordPopup != null)
            recordPopup.SetActive(!recordPopup.activeSelf);
    }*/

    private void ToggleSettingsPopup()
    {
        if (settingsPopup != null)
            settingsPopup.SetActive(!settingsPopup.activeSelf);
    }

    /*private void CloseRecordPopup()
    {
        if (recordPopup != null)
            recordPopup.SetActive(false);
    }*/

    private void CloseSettingsPopup()
    {
        if (settingsPopup != null)
            settingsPopup.SetActive(false);
    }

    private void OpenExitConfirmPopup()
    {
        if (exitConfirmPopup != null)
            exitConfirmPopup.SetActive(true);
    }

    private void CloseExitConfirmPopup()
    {
        if (exitConfirmPopup != null)
            exitConfirmPopup.SetActive(false);
    }
}