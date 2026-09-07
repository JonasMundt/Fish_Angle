using UnityEngine;

public class SeagullMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float movementSpeed = 2f;
    [SerializeField] private bool flyToRight = true;

    [Header("Screen boundaries")]
    [SerializeField] private float leftBoundary = -12f;
    [SerializeField] private float rightBoundary = 12f;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateDirection();
    }

    private void Update()
    {
        float direction = flyToRight ? 1f : -1f;

        transform.Translate(
            Vector3.right * direction * movementSpeed * Time.deltaTime
        );

        CheckBoundaries();
    }

    private void CheckBoundaries()
    {
        if (flyToRight && transform.position.x > rightBoundary)
        {
            Vector3 newPosition = transform.position;
            newPosition.x = leftBoundary;
            transform.position = newPosition;
        }
        else if (!flyToRight && transform.position.x < leftBoundary)
        {
            Vector3 newPosition = transform.position;
            newPosition.x = rightBoundary;
            transform.position = newPosition;
        }
    }

    private void UpdateDirection()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = !flyToRight;
        }
    }
}