using UnityEngine;

public class FallingCrystal : MonoBehaviour
{
    [Header("Fallgeschwindigkeit")]
    [SerializeField] private float minFallSpeed = 0.7f;
    [SerializeField] private float maxFallSpeed = 1.4f;

    [Header("Seitliches Schweben")]
    [SerializeField] private float minSwayStrength = 0.05f;
    [SerializeField] private float maxSwayStrength = 0.2f;

    [SerializeField] private float minSwaySpeed = 0.8f;
    [SerializeField] private float maxSwaySpeed = 1.8f;

    [Header("Rotation")]
    [SerializeField] private float minRotationSpeed = -25f;
    [SerializeField] private float maxRotationSpeed = 25f;

    [Header("Lebensdauer")]
    [SerializeField] private float lifetime = 12f;

    private float fallSpeed;
    private float rotationSpeed;

    private float swayStrength;
    private float swaySpeed;
    private float swayOffset;

    private void Start()
    {
        fallSpeed = Random.Range(minFallSpeed, maxFallSpeed);

        rotationSpeed = Random.Range(
            minRotationSpeed,
            maxRotationSpeed
        );

        swayStrength = Random.Range(
            minSwayStrength,
            maxSwayStrength
        );

        swaySpeed = Random.Range(
            minSwaySpeed,
            maxSwaySpeed
        );

        swayOffset = Random.Range(0f, Mathf.PI * 2f);

        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        float horizontalMovement =
            Mathf.Sin(Time.time * swaySpeed + swayOffset)
            * swayStrength;

        Vector3 movement = new Vector3(
            horizontalMovement,
            -fallSpeed,
            0f
        );

        transform.position += movement * Time.deltaTime;

        transform.Rotate(
            0f,
            0f,
            rotationSpeed * Time.deltaTime
        );
    }
}