using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class Pickupable : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OnPickup(other.gameObject);
        }
    }

    public abstract void OnPickup(GameObject player);
}
