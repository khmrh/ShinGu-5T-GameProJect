using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetting : MonoBehaviour
{
    public void GoSetting()
    {
        Time.timeScale = 0f;
    }

    public void BackGame()
    {
        Time.timeScale = 1.0f;
    }
}
