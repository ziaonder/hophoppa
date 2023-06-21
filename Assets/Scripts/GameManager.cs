using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int score;
    public enum GameState {STARTED, PAUSED}
    public GameState gameState;

    // Start is called before the first frame update
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        gameState = GameState.PAUSED;
    }

    //public void IncreaseScore()
    //{
    //    score += 50;
    //}
}