using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private int damage = 1;

    private void Start()
    {
        // Destroy self after lifetime seconds
        Destroy(gameObject, lifetime);
    }

    public void Initialize(Vector3 targetPos)
    {
        // Calculate direction towards target
        Vector3 direction = (targetPos - transform.position).normalized;
        
        // Rotate to face direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        
        // Set velocity (simpler than updating position manually every frame)
        // Ensure there is a Rigidbody2D on the projectile prefab, or move manually in Update if preferred.
        // For simplicity/robustness without assuming RB2D:
    }

    private void Update()
    {
        // Move forward (right relative to self rotation)
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       if (collision.CompareTag("Player"))
       {
           Debug.Log("Projectile hit the player!");
           if (StatManager.manager != null)
           {
               StatManager.manager.ChangeHealth(-damage);
           }
           Destroy(gameObject);
       }
       // Destroy if it hits walls (optional, assuming "Ground" tag or similar)
       else if (collision.CompareTag("Ground")) 
       {
           Destroy(gameObject);
       }
    }
}
