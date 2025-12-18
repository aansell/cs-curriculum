using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 5f;

    private Vector3 targetPosition;
    private Vector3 moveDirection;
    private float spawnTime;

    public void Initialize(Vector3 targetPos)
    {
        targetPosition = targetPos;
        moveDirection = (targetPosition - transform.position).normalized;
        spawnTime = Time.time;
    }

    private void Update()
    {
        transform.position += moveDirection * speed * Time.deltaTime;

        // Rotate the projectile
        transform.Rotate(0, 0, 720f * Time.deltaTime);

        if (Time.time >= spawnTime + lifetime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       if (collision.CompareTag("Player"))
       {
           Debug.Log("Projectile hit the player!");
           Destroy(gameObject);
       }
    }
}
