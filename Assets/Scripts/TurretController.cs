using UnityEngine;

public class TurretController : MonoBehaviour
{
    [SerializeField] private float maxAttackRange = 7f;
    [SerializeField] private float attackCooldown = 2f;
    private float lastAttackTime = 0f;

    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;

    private PlayerController playerController;

    private void Start()
    {
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        if (playerController == null)
        {
            Debug.LogError("Player object not found in the scene.");
            this.enabled = false; // Disable this script
            return;
        }
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerController.transform.position);

        if (distanceToPlayer <= maxAttackRange && Time.time >= lastAttackTime + attackCooldown)
        {
            AttackPlayer();
            lastAttackTime = Time.time;
        }
    }

    private void AttackPlayer()
    {
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, playerController.transform.position - projectileSpawnPoint.position);

        GameObject proj = Instantiate(projectilePrefab, projectileSpawnPoint.position, rotation);
        proj.GetComponent<Projectile>().Initialize(playerController.transform.position);
    }
}