using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Check for nearby interactable objects
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 1f);
            foreach (var hit in hits)
            {
                IInteractable interactable = hit.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    interactable.OnInteract(gameObject);
                    break;
                }
            }
        }
    }
}
