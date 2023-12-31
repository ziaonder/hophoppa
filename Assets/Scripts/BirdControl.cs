using System;
using System.Collections;
using UnityEngine;

public class BirdControl : MonoBehaviour
{
    public static event Action OnGameStarted, OnHit;
    public static event Action<int> OnGameEnd;
    public static BirdControl Instance;
    private Vector3 initialPosition = new Vector3(-0.22f, 0, 0);
    private Rigidbody2D rb;
    private Vector2 force = new Vector2 (0, 5f);
    private SpriteRenderer sRenderer;
    private float gravityScale = 1f;
    [SerializeField] private Sprite spriteAlive, spriteDead;
    private int numberOfTotalTouches;
    private int rayLength = 6;
    public int pipesPassedCount;
    private int instanceID = 0;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;

        rb = GetComponent<Rigidbody2D>();
        sRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        GameManager.Instance.gameState = GameManager.GameState.SET;
        rb.gravityScale = 0f;
        sRenderer.sprite = spriteAlive;
        Input.multiTouchEnabled = false;
    }

    private void Update()
    {
        if(GameManager.Instance.gameState == GameManager.GameState.PAUSED)
            rb.Sleep();

        if(GameManager.Instance.gameState == GameManager.GameState.RUNNING)
            rb.WakeUp();

        if (GameManager.Instance.gameState == GameManager.GameState.SET)
        {
            numberOfTotalTouches = 0;
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0f;
        }

        if (Input.touchCount == 1 && UIRaycaster.isTouchEnabled)
        {
            numberOfTotalTouches++;
        }

        // This means the player has touched the screen and the game has started.
        if (numberOfTotalTouches == 1)
        {
            GameManager.Instance.gameState = GameManager.GameState.RUNNING;
            OnGameStarted?.Invoke();
        }

        if(GameManager.Instance.gameState == GameManager.GameState.RUNNING){
            rb.gravityScale = gravityScale;
            
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                if(UIRaycaster.isPauseButtonHit != true)
                {
                    rb.velocity = force;
                }
            }
            CountPassedPipes();
        }

        if (transform.position.y > 5 || transform.position.y < -5) {
            if(sRenderer.sprite != spriteDead) // if player is not dead
            {
                StartCoroutine(SetDeathArrangements());
            }
        }
    }

    private void CountPassedPipes()
    {
        if(Physics2D.Raycast(transform.position, Vector2.up, rayLength).collider != null)
        {
            if(instanceID != Physics2D.Raycast(transform.position, Vector2.up, rayLength).collider.gameObject.GetInstanceID())
            {
                instanceID = Physics2D.Raycast(transform.position, Vector2.up, rayLength).collider.gameObject.GetInstanceID();
                pipesPassedCount++;
                GameManager.Instance.score = pipesPassedCount;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        rb.velocity = Vector2.zero;
        OnHit?.Invoke();
        StartCoroutine(SetDeathArrangements());
    }

    private IEnumerator SetDeathArrangements()
    {
        GameManager.Instance.gameState = GameManager.GameState.ENDED;
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
        sRenderer.sprite = spriteDead;
        OnGameEnd?.Invoke(pipesPassedCount);
        yield return new WaitForSeconds(2f);

        // This condition makes sure player's physics are not changed while not in GameState.ENDED state. Without this there was some stuttering.
        if(GameManager.Instance.gameState == GameManager.GameState.ENDED)
        {
            rb.gravityScale = 0f;
            rb.velocity = Vector3.zero;
        }
    }

    public void RestartGame()
    {
        transform.position = initialPosition;
        instanceID = 0;
        pipesPassedCount = 0;
        sRenderer.sprite = spriteAlive;
        gameObject.GetComponent<CircleCollider2D>().enabled = true;
    }
}