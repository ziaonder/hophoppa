using System.Collections.Generic;
using UnityEngine;

public class PipeControl : MonoBehaviour
{
    public static PipeControl Instance;
    private float distanceBetweenPipesHorizontally, distanceBetweenPipesVertically = 3.5f, bottomPipeMaxBorder = -1, bottomPipeMinBorder = -4;
    private Vector2 pipeSpawnPoint = new Vector2(5, -1), invertedPipeSpawnPoint = new Vector2(5, 2.5f);
    private Vector2 pipeVelocity = new Vector2(-2f, 0);
    private float verticalCamWorldSize, horizontalCamWorldSize, camAspectRatio;
    private SpriteRenderer spriteRenderer;
    private static float duplicatePipeYAxis;
    private float pipeSize;
    private Camera cam;

    [SerializeField] private GameObject pipe;
    private GameObject invertedPipe;
    private List<GameObject> pipes;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;

        cam = Camera.main;
        invertedPipe = pipe;
        spriteRenderer = pipe.GetComponent<SpriteRenderer>();
        pipes = new List<GameObject>();
    }

    private void Start()
    {
        pipeSize = FloatDigitReducer(spriteRenderer.bounds.size.x);

        verticalCamWorldSize = cam.orthographicSize;
        camAspectRatio = cam.aspect;
        string formattedValue = camAspectRatio.ToString("0.00");
        camAspectRatio = float.Parse(formattedValue);
        horizontalCamWorldSize = FloatDigitReducer(camAspectRatio * verticalCamWorldSize);
        distanceBetweenPipesHorizontally = horizontalCamWorldSize + pipeSize / 2;

        // turning the duplicate pipe upside down to make it invertedPipe.
        invertedPipe.transform.rotation = Quaternion.Euler(0, 0, 180);

        pipes.Add(Instantiate(pipe, pipeSpawnPoint, Quaternion.identity));
        pipes.Add(Instantiate(invertedPipe, invertedPipeSpawnPoint, invertedPipe.transform.rotation));

        pipes.Add(Instantiate(pipe, new Vector2(pipeSpawnPoint.x + distanceBetweenPipesHorizontally, pipeSpawnPoint.y), Quaternion.identity));
        pipes.Add(Instantiate(invertedPipe, new Vector2(invertedPipeSpawnPoint.x + distanceBetweenPipesHorizontally,
            invertedPipeSpawnPoint.y), invertedPipe.transform.rotation));
    }

    private void Update()
    {
        if (GameManager.Instance.gameState == GameManager.GameState.RUNNING)
        {
            foreach (var pipe in pipes)
            {
                pipe.GetComponent<Rigidbody2D>().velocity = pipeVelocity;
            }
        }
        else
        {
            foreach (var pipe in pipes)
            {
                pipe.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
            return;
        }

        for(int i = 0; i < pipes.Count; i++)
        {
            // Checks whether the pipe is out of the screen.
            if (FloatDigitReducer(pipes[i].transform.position.x + pipeSize / 2) < -horizontalCamWorldSize)
            {
                if (i == 0 || i == 2) // if the pipes are non-inverted.
                {
                    pipes[i].transform.position = PipeRespawnPoint(false);
                }
                else // if the pipes are inverted. 
                {
                    pipes[i].transform.position = PipeRespawnPoint(true);
                }
            }
        }
    }

    private Vector2 PipeRespawnPoint(bool isPipeInverted)
    {
        //return Vector2.zero;
        if (!isPipeInverted)
        {
            return new Vector2(horizontalCamWorldSize + pipeSize / 2, duplicatePipeYAxis = Random.Range(bottomPipeMinBorder, bottomPipeMaxBorder + 1));
        }
        else
        {
            return new Vector2(horizontalCamWorldSize + pipeSize / 2, duplicatePipeYAxis + distanceBetweenPipesVertically);
        }
    }

    private float FloatDigitReducer(float value)
    {
        return float.Parse(value.ToString("0.00"));
    }

    public void RestartGame()
    {
        pipes[0].transform.position = pipeSpawnPoint;
        pipes[1].transform.position = invertedPipeSpawnPoint;
        pipes[1].transform.rotation = invertedPipe.transform.rotation;
        pipes[2].transform.position = new Vector2(pipeSpawnPoint.x + distanceBetweenPipesHorizontally, pipeSpawnPoint.y);
        pipes[3].transform.position = new Vector2(invertedPipeSpawnPoint.x + distanceBetweenPipesHorizontally,
            invertedPipeSpawnPoint.y);
        pipes[3].transform.rotation = invertedPipe.transform.rotation;
    }
}