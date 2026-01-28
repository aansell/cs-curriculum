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

    // Reference to the Rigidbody2D component for physics
    private Rigidbody2D rb;

    private void Start()
    {
        // Get the Rigidbody2D attached to this game object
        rb = GetComponent<Rigidbody2D>();
        
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

        // Optional: Ensure gravity is correct for the mode
        // Platformers need gravity, TopDown games usually don't
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
        // 1. Get User Input (Human Readable)
        // Reset directions to 0 first
        xDirection = Input.GetAxis("Horizontal"); // A/D or Left/Right Arrows
        yDirection = Input.GetAxis("Vertical");   // W/S or Up/Down Arrows


        
        // Calculate X Movement
        xVector = new Vector2(xDirection * xSpeed * Time.deltaTime, 0);
        

        // Calculate Y Movement
        if (isPlatformer)
        {
            // In Platformer mode, W/S keys don't move us up/down (gravity/jump does that)
            // So we set yVector to zero for movement
            yVector = new Vector2(0, 0);

            // Handle Jumping separately
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }
        else
        {
            // In TopDown mode, W/S keys move us up/down
            yVector = new Vector2(0, yDirection * ySpeed * Time.deltaTime);
        }

        // 3. Apply Movement
        // transform.Translate moves the object by the calculated vectors
        transform.Translate(xVector);
        transform.Translate(yVector);

        // 4. Interaction (Lever)
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Find all Levers
            Lever[] levers = FindObjectsByType<Lever>(FindObjectsSortMode.None);
            foreach (Lever lever in levers)
            {
                // Check distance
                if (Vector3.Distance(transform.position, lever.transform.position) < 2.0f)
                {
                    lever.Interact();
                }
            }
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
