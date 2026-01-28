using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // Variables for movement settings
    [SerializeField] private float xSpeed = 5.0f;
    [SerializeField] private float ySpeed = 5.0f;
    [SerializeField] private float jumpForce = 10.0f;

    // Variables for current input direction (-1, 0, or 1)
    private float xDirection;
    private float yDirection;

    // Variables for the calculated movement vectors
    private Vector2 xVector;
    private Vector2 yVector;

    // Configuration to switch between game modes
    [SerializeField] private bool isPlatformer;

    // References to components
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;

    private void Start()
    {
        // Get the components
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        
        // Determine the game mode based on the current scene name
        string currentSceneName = SceneManager.GetActiveScene().name;

        if (currentSceneName == "Platformer" || currentSceneName == "Start")
        {
            isPlatformer = true;
        }
        else
        {
            isPlatformer = false;
        }

        // Ensure gravity is correct for the mode
        if (isPlatformer)
        {
            rb.gravityScale = 2; 
        }
        else
        {
            rb.gravityScale = 0;
        }
    }

    private void Update()
    {
        // 1. Get User Input
        xDirection = Input.GetAxis("Horizontal"); // A/D or Left/Right Arrows
        yDirection = Input.GetAxis("Vertical");   // W/S or Up/Down Arrows

        // 2. Calculate Movement Vectors
        xVector = new Vector2(xDirection * xSpeed * Time.deltaTime, 0);

        if (isPlatformer)
        {
            yVector = new Vector2(0, 0);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }
        else
        {
            yVector = new Vector2(0, yDirection * ySpeed * Time.deltaTime);
        }

        // 3. Apply Movement
        transform.Translate(xVector);
        transform.Translate(yVector);

        // 4. Handle Animations
        UpdateAnimation();

        // 5. Interaction (Lever)
        if (Input.GetKeyDown(KeyCode.E))
        {
            Lever[] levers = FindObjectsByType<Lever>(FindObjectsSortMode.None);
            foreach (Lever lever in levers)
            {
                if (Vector3.Distance(transform.position, lever.transform.position) < 2.0f)
                {
                    lever.Interact();
                }
            }
        }
    }

    private void UpdateAnimation()
    {
        if (anim == null || sprite == null) return;

        // Check if we are moving at all
        bool isMoving = xDirection != 0 || yDirection != 0;
        anim.SetBool("IsWalking", isMoving);

        if (isMoving)
        {
            // If moving horizontally (or more horizontally than vertically)
            if (Mathf.Abs(xDirection) >= Mathf.Abs(yDirection))
            {
                anim.SetInteger("WalkDir", 1); // Side
                sprite.flipX = xDirection > 0;
            }
            // If moving vertically (and we are NOT in platformer mode)
            else if (!isPlatformer)
            {
                if (yDirection > 0)
                {
                    anim.SetInteger("WalkDir", 0); // Up
                    sprite.flipX = true;
                }
                else
                {
                    anim.SetInteger("WalkDir", 2); // Down
                    sprite.flipX = false;
                }
            }
        }

        // Handle Attack animation
        if (Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("Attack");

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            if (StatManager.manager != null)
            {
                StatManager.manager.ChangeCoins(1);
            }
            Destroy(other.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Spikes"))
        {
            if (StatManager.manager != null)
            {
                StatManager.manager.ChangeHealth(-1);
            }
        }
    }
}