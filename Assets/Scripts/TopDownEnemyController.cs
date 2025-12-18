using System.Runtime.CompilerServices;
using UnityEngine;

public class TopDownEnemyController : MonoBehaviour
{

    [SerializeField] private Animator enemyAnimator;
    [SerializeField] private SpriteRenderer enemySprite;

    [Header("Enemy Behavior Ranges")]
    [SerializeField] private float maxAttackRange = 3f;
    [SerializeField] private float maxChaseRange = 10f;

    [Header("Enemy Movement Speeds")]
    [SerializeField] private float chaseSpeed = 2f;
    [SerializeField] private float patrolSpeed = 1f;

    private TopDownPlayerController playerController;

    private void Start()
    {
        playerController = GameObject.FindWithTag("Player").GetComponent<TopDownPlayerController>();
        if (playerController == null)
        {
            Debug.LogError("Player object not found in the scene.");
            this.enabled = false; // Disable this script
            return;
        }
    }

    private void Update()
    {
        Vector3 previousPosition = transform.position;

        float distanceToPlayer = Vector3.Distance(transform.position, playerController.transform.position);

        switch (distanceToPlayer)
        {
            case float d when d <= maxAttackRange:
                AttackPlayer();
                break;
            case float d when d <= maxChaseRange:
                ChasePlayer();
                break;
            default:
                PatrolArea();
                break;
        }

        Vector3 deltaPosition = (transform.position - previousPosition) / Time.deltaTime;
        UpdateAnimation(deltaPosition);

    }

    private void AttackPlayer()
    {
        Debug.Log("Attacking the player!");
        // Implement attack logic here
    }

    private void ChasePlayer()
    {
        Vector3 direction = (playerController.transform.position - transform.position).normalized;
        transform.position += direction * chaseSpeed * Time.deltaTime;
    }

    private Vector3 patrolTarget;
    private float patrolTimer = 0f;
    private void PatrolArea()
    {
        float distance = Vector3.Distance(transform.position, patrolTarget);
        if (distance < 0.5f || distance > 15 || patrolTimer > 10f) // Get a new patrol location if too close or too far or timer exceeds limit
        {
            patrolTarget = new Vector3(
                Random.Range(-10f, 10f),
                Random.Range(-10f, 10f),
                transform.position.z
            );
            patrolTimer = 0f;
        }

        patrolTimer += Time.deltaTime;
        Vector3 direction = (patrolTarget - transform.position).normalized;
        transform.position += direction * patrolSpeed * Time.deltaTime;
    }

    private Vector3 UpdateAnimation(Vector3 deltaPosition)
    {
        if (deltaPosition.magnitude > 0.1f)
        {
            if (Mathf.Abs(deltaPosition.x) > Mathf.Abs(deltaPosition.y))
            {
                // Horizontal movement
                enemyAnimator.SetInteger("Direction", 0);
                enemySprite.flipX = deltaPosition.x < 0;
            }
            else
            {
                enemySprite.flipX = false;
                // Vertical movement
                if (deltaPosition.y > 0)
                {
                    enemyAnimator.SetInteger("Direction", 1);
                }
                else
                {
                    enemyAnimator.SetInteger("Direction", 3);
                }
            }
        }
        else
        {
        }

        return deltaPosition;
    }
}
