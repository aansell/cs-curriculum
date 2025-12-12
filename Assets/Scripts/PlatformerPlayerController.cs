using UnityEngine;

public class TopDownPlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float sprintSpeed;

    private void Update()
    {
        Vector2 input = GetInput();

        if (input != Vector2.zero)
        {
            float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : speed;

            transform.Translate(input.normalized * currentSpeed * Time.deltaTime);
        }
    }

    private Vector2 GetInput()
    {
        Vector2 input = Vector2.zero;

        if (Input.GetKey(KeyCode.A))
        {
            input.x -= 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            input.x += 1;
        }
        if (Input.GetKey(KeyCode.W))
        {
            input.y += 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            input.y -= 1;
        }

        return input;
    }
}
