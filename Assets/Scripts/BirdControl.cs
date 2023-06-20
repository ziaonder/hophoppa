using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdControl : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 force = new Vector2 (0, 6f);
    private SpriteRenderer sRenderer;
    [SerializeField] private Sprite spriteAlive, spriteDead;

    private enum PlayerState {ALIVE, DEAD};
    private PlayerState playerState;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sRenderer = GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        playerState = PlayerState.ALIVE;
        rb.gravityScale = .8f;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerState == PlayerState.DEAD) {
            sRenderer.sprite = spriteDead;
            return; 
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            rb.velocity = force;
        }

        if (transform.position.y > 5 || transform.position.y < -5) {
            playerState = PlayerState.DEAD;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerState = PlayerState.DEAD;
    }
}
