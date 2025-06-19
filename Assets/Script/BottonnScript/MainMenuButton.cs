using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuButtons : MonoBehaviour
{
    [Header("���� ��ư��")]
    public GameObject startButton;
    public GameObject exitButton;
    public GameObject recordButton;
    public GameObject settingsButton;

    [Header("�˾�â��")]
    //public GameObject recordPopup;
    public GameObject settingsPopup;

    [Header("�ݱ� ��ư��")]
    //public GameObject closeRecordButton;

    [Header("�̵��� �� �̸�")]
    public string gameSceneName = "PepperMergeGame";

    [Header("���� ���� Ȯ�� �˾�")]
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