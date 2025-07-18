using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement2D : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 movement;
    [SerializeField] Animator playerAnimator;
    [SerializeField] SpriteRenderer spriteRenderer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movement = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
            movement.y += 1;
        if (Input.GetKey(KeyCode.S))
            movement.y -= 1;
        if (Input.GetKey(KeyCode.D))
            movement.x += 1;
        if (Input.GetKey(KeyCode.A))
            movement.x -= 1;

        movement.Normalize(); // keep speed consistent diagonally
        if (movement != Vector2.zero)
        {
            playerAnimator.SetBool("IsWalking", true);
            if (movement.x <= 0)
                spriteRenderer.flipX = true;
            else
                spriteRenderer.flipX = false;
        }
        else
            playerAnimator.SetBool("IsWalking", false);

    }

    void FixedUpdate()
    {
        rb.velocity = movement * moveSpeed;
    }
}
