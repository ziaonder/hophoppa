using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int score;
    public enum GameState {SET, RUNNING, PAUSED, ENDED}
    public GameState gameState;
    public GameState previousGameState;

    // Start is called before the first frame update
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
    }
}