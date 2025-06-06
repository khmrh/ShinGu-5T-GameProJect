using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int currentScore = 0;

    public void AddScore(int amount)
    {
        currentScore += amount;
        Debug.Log("현재 점수: " + currentScore);
        
    }

    public void ResetScore()
    {
        currentScore = 0;

    }

    public int GetScore()
    {
        return currentScore;

    }
}
