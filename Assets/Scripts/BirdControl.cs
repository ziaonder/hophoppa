using System.Collections;
using UnityEngine;

public class BirdControl : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 force = new Vector2 (0, 5f);
    private SpriteRenderer sRenderer;
    private float gravityScale = 1f;
    [SerializeField] private Sprite spriteAlive, spriteDead;
    private int numberOfTotalTouches;
    private int rayLength = 6, pipesPassedCount;
    private int instanceID = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        rb.gravityScale = 0f;
        sRenderer.sprite = spriteAlive;
        Input.multiTouchEnabled = false;
    }

    void Update()
    {
        if(Input.touchCount == 1)
        {
            numberOfTotalTouches++;
        }

        // This means the player has touched the screen and the game has started.
        if (numberOfTotalTouches == 1)
        {
            GameManager.Instance.gameState = GameManager.GameState.STARTED;
        }

        if(GameManager.Instance.gameState == GameManager.GameState.STARTED){
            rb.gravityScale = gravityScale;
            
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                rb.velocity = force;
            }
            CountPassedPipes();
        }

        if (transform.position.y > 5 || transform.position.y < -5) {
            StartCoroutine(SetDeathArrangements());
        }

        //if(GameManager.Instance.gameState == GameManager.GameState.FINISHED) { GameManager.Instance.CancelInvoke("IncreaseScore"); }
    }

    private void CountPassedPipes()
    {
        if(Physics2D.Raycast(transform.position, Vector2.up, rayLength).collider != null)
        {

            if(instanceID == Physics2D.Raycast(transform.position, Vector2.up, rayLength).collider.gameObject.GetInstanceID())
            {
                return;
            }
            else
            {
                instanceID = Physics2D.Raycast(transform.position, Vector2.up, rayLength).collider.gameObject.GetInstanceID();
                pipesPassedCount++;
                GameManager.Instance.score = pipesPassedCount;
            } 
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(SetDeathArrangements());
    }

    private IEnumerator SetDeathArrangements()
    {
        GameManager.Instance.gameState = GameManager.GameState.ENDED;
        sRenderer.sprite = spriteDead;
        yield return new WaitForSeconds(2f);
        rb.gravityScale = 0f;
        rb.velocity = Vector3.zero;
    }
}