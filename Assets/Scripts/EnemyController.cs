using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float speed = 2.0f;
    [SerializeField] private int damage = 1;

    // Platformer Logic
    private bool isPlatformer;
    [SerializeField] private float patrolDistance = 3.0f;
    private Vector3 startPosition;
    private int direction = 1;

    // TopDown Logic
    [SerializeField] private float chaseDistance = 5.0f;
    private Transform playerTransform;

    private float damageCooldown = 1.0f;
    private float lastDamageTime = -1.0f;

    private void Start()
    {
        startPosition = transform.position;

        // Auto-detect mode based on scene name
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "Platformer" || sceneName == "Start")
        {
            isPlatformer = true;
        }
        else
        {
            isPlatformer = false;
        }

        // Find player for TopDown chasing
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    private void Update()
    {
        if (isPlatformer)
        {
            PlatformerMove();
        }
        else
        {
            TopDownMove();
        }
    }

    private void PlatformerMove()
    {
        // Simple Patrol: Move back and forth from start position
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);

        float dist = Vector3.Distance(startPosition, transform.position);
        if (dist > patrolDistance)
        {
            direction *= -1; // Switch direction
            // Ensure we don't get stuck flipping
            if (direction == 1 && transform.position.x < startPosition.x) direction = 1;
             else if (direction == -1 && transform.position.x > startPosition.x) direction = -1;
             
             //Visual flip
             GetComponentInChildren<SpriteRenderer>().flipX = direction < 0;
        }
    }

    private void TopDownMove()
    {
        if (playerTransform == null) return;

        float dist = Vector3.Distance(transform.position, playerTransform.position);

        if (dist < chaseDistance)
        {
            // Chase the player
            Vector3 dir = (playerTransform.position - transform.position).normalized;
            transform.Translate(dir * speed * Time.deltaTime);
            
            // Visual flip
            GetComponentInChildren<SpriteRenderer>().flipX = dir.x < 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            TryDamage();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            TryDamage();
        }
    }

    private void TryDamage()
    {
        if (Time.time > lastDamageTime + damageCooldown)
        {
            if (StatManager.manager != null)
            {
                StatManager.manager.ChangeHealth(-damage);
                lastDamageTime = Time.time;
            }
        }
    }
}