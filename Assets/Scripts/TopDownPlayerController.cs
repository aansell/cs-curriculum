using UnityEngine;

public class PlatformerPlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Vector2 input = GetInput();

        if (input != Vector2.zero)
        {
            input = input.normalized;
            transform.Translate(input.normalized * speed * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector2.up * jumpForce * 10, ForceMode2D.Impulse);
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
