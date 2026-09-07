using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Bubble : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private Vector3 startPosition;
    private Vector3 startScale;

    private float riseSpeed;
    private float lifetime;
    private float maximumRiseHeight;

    private float horizontalDriftSpeed;
    private float swayAmount;
    private float swaySpeed;

    private float age;
    private float swayOffset;
    private float driftDirection;

    private Color originalColor;

    private bool initialized;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    public void Initialize(
        Sprite bubbleSprite,
        float newRiseSpeed,
        float newLifetime,
        float newMaximumRiseHeight,
        float newHorizontalDriftSpeed,
        float newSwayAmount,
        float newSwaySpeed,
        float newScale
    )
    {
        if (bubbleSprite != null)
        {
            spriteRenderer.sprite = bubbleSprite;
        }

        riseSpeed = newRiseSpeed;
        lifetime = newLifetime;
        maximumRiseHeight = newMaximumRiseHeight;

        horizontalDriftSpeed = newHorizontalDriftSpeed;
        swayAmount = newSwayAmount;
        swaySpeed = newSwaySpeed;

        startPosition = transform.position;

        startScale = Vector3.one * newScale;
        transform.localScale = startScale;

        age = 0f;

        swayOffset = Random.Range(
            0f,
            Mathf.PI * 2f
        );

        driftDirection = Random.Range(
            -1f,
            1f
        );

        originalColor = spriteRenderer.color;
        initialized = true;
    }

    private void Update()
    {
        if (!initialized)
        {
            return;
        }

        age += Time.deltaTime;

        MoveBubble();
        FadeBubble();
        SlightlyChangeScale();

        bool lifetimeFinished =
            age >= lifetime;

        bool maximumHeightReached =
            transform.position.y >=
            startPosition.y + maximumRiseHeight;

        if (lifetimeFinished || maximumHeightReached)
        {
            Destroy(gameObject);
        }
    }

    private void MoveBubble()
    {
        float upwardMovement =
            riseSpeed * age;

        float sidewaysDrift =
            driftDirection *
            horizontalDriftSpeed *
            age;

        float naturalSway =
            Mathf.Sin(
                age * swaySpeed + swayOffset
            ) * swayAmount;

        transform.position = new Vector3(
            startPosition.x +
            sidewaysDrift +
            naturalSway,

            startPosition.y +
            upwardMovement,

            startPosition.z
        );
    }

    private void FadeBubble()
    {
        float lifetimeProgress =
            Mathf.Clamp01(age / lifetime);

        float alpha = 1f;

        // Erst in den letzten 25 Prozent ausblenden
        if (lifetimeProgress >= 0.75f)
        {
            alpha = Mathf.InverseLerp(
                1f,
                0.75f,
                lifetimeProgress
            );
        }

        Color currentColor = originalColor;
        currentColor.a = originalColor.a * alpha;

        spriteRenderer.color = currentColor;
    }

    private void SlightlyChangeScale()
    {
        float pulse =
            1f +
            Mathf.Sin(
                age * swaySpeed * 0.7f +
                swayOffset
            ) * 0.025f;

        transform.localScale =
            startScale * pulse;
    }
}