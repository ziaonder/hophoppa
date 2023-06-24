using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int score;
    public enum GameState {SET, STARTED, PAUSED, ENDED}
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
        gameState = GameState.SET;
    }
}