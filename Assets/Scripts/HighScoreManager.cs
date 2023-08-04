using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreManager : MonoBehaviour
{
    private void OnEnable()
    {
        BirdControl.OnGameEnd += SetHighScore;
    }

    private void OnDisable()
    {
        BirdControl.OnGameEnd -= SetHighScore;
    }
    private void SetHighScore(int score)
    {
        if (score > GetHighScore())
        {
            Debug.Log("in");
            PlayerPrefs.SetInt("highScore", score);
            PlayerPrefs.Save();
        }
    }

    public static int GetHighScore()
    {
        return PlayerPrefs.GetInt("highScore");
    }
}
