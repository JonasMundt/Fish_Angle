using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Bewegung")]
    [SerializeField] private float movementSpeed = 4f;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private float horizontalInput;
    private bool movementEnabled = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (!movementEnabled)
        {
            horizontalInput = 0f;
            animator.SetBool("IsWalking", false);
            return;
        }

        horizontalInput = Input.GetAxisRaw("Horizontal");

        animator.SetBool(
            "IsWalking",
            horizontalInput != 0f
        );

        if (horizontalInput > 0f)
        {
            spriteRenderer.flipX = false;
        }
        else if (horizontalInput < 0f)
        {
            spriteRenderer.flipX = true;
        }
    }

    private void FixedUpdate()
    {
        if (!movementEnabled)
        {
            rb.linearVelocity = new Vector2(
                0f,
                rb.linearVelocity.y
            );

            return;
        }

        rb.linearVelocity = new Vector2(
            horizontalInput * movementSpeed,
            rb.linearVelocity.y
        );
    }

    public void SetMovementEnabled(bool enabled)
    {
        movementEnabled = enabled;

        if (!movementEnabled)
        {
            horizontalInput = 0f;

            rb.linearVelocity = new Vector2(
                0f,
                rb.linearVelocity.y
            );

            animator.SetBool("IsWalking", false);
        }
    }
}