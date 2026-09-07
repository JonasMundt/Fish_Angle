using UnityEngine;

public enum FishType
{
    Perch,
    Catshark,
    CrystalEel,
    PrismTrout
    
}

[RequireComponent(typeof(SpriteRenderer))]
public class FishSwimmer : MonoBehaviour
{
    [Header("Fischart")]
    [SerializeField] private FishType fishType = FishType.Perch;

    [Header("Fangvoraussetzung")]
    [Range(1, 3)]
    [SerializeField] private int requiredHookLevel = 1;

    [Header("Leichte natürliche Bewegung")]
    [SerializeField] private float verticalSwayAmount = 0.08f;
    [SerializeField] private float verticalSwaySpeed = 1.5f;

    [Header("Punkte am Fisch")]
    [SerializeField] private Transform fishMouthPoint;
    [SerializeField] private Transform bitePoint;
    [Header("BitePoint pro Animationsframe")]
    [SerializeField] private Vector2 bitePointFrame1;
    [SerializeField] private Vector2 bitePointFrame2;
    [SerializeField] private Vector2 bitePointFrame3;
    [Header("FishMouthPoint pro Animationsframe")]
    [SerializeField] private Vector2 fishMouthPointFrame1;
    [SerializeField] private Vector2 fishMouthPointFrame2;
    [SerializeField] private Vector2 fishMouthPointFrame3;

    [Header("Position am Haken")]
    [SerializeField] private float hookedRotationZ = 0f;

    [Header("Flucht nach verlorenem Minigame")]
    [Min(0.1f)]
    [SerializeField] private float escapeSpeed = 8f;

    [Min(1f)]
    [SerializeField] private float escapeDistance = 15f;

    // =========================================================
    // MINIGAME
    // =========================================================

    [Header("Minigame")]
    [SerializeField] private Sprite minigameIcon;

    [Min(1f)]
    [SerializeField] private float minigameMovementSpeed = 90f;

    [Min(0f)]
    [SerializeField] private float minigameMinimumWaitTime = 0.4f;

    [Min(0f)]
    [SerializeField] private float minigameMaximumWaitTime = 1.2f;

    [Min(0f)]
    [SerializeField] private float minigameMinimumTargetDistance = 40f;

    [Min(0f)]
    [SerializeField] private float minigameProgressIncreaseSpeed = 0.25f;

    [Min(0f)]
    [SerializeField] private float minigameProgressDecreaseSpeed = 0.12f;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private Vector3 targetPosition;

    private float movementSpeed;
    private float baseY;
    private float randomSwayOffset;

    private bool isInitialized;
    private bool swimsRight;
    private bool isEscaping;

    private System.Action onDespawn;

    public bool IsHooked { get; private set; }

    public FishType FishType =>
        fishType;

    public int RequiredHookLevel =>
        requiredHookLevel;

    public Sprite MinigameIcon =>
        minigameIcon;

    public float MinigameMovementSpeed =>
        minigameMovementSpeed;

    public float MinigameMinimumWaitTime =>
        minigameMinimumWaitTime;

    public float MinigameMaximumWaitTime =>
        minigameMaximumWaitTime;

    public float MinigameMinimumTargetDistance =>
        minigameMinimumTargetDistance;

    public float MinigameProgressIncreaseSpeed =>
        minigameProgressIncreaseSpeed;

    public float MinigameProgressDecreaseSpeed =>
        minigameProgressDecreaseSpeed;

    private void Awake()
    {
        spriteRenderer =
            GetComponent<SpriteRenderer>();

        animator =
            GetComponent<Animator>();
    }

    public void Initialize(
        Vector3 newTargetPosition,
        float newMovementSpeed,
        bool spriteFacesRight,
        System.Action despawnCallback
    )
    {
        targetPosition =
            newTargetPosition;

        movementSpeed =
            newMovementSpeed;

        onDespawn =
            despawnCallback;

        baseY =
            transform.position.y;

        randomSwayOffset =
            Random.Range(
                0f,
                Mathf.PI * 2f
            );

        swimsRight =
            targetPosition.x >
            transform.position.x;

        if (spriteFacesRight)
        {
            spriteRenderer.flipX =
                !swimsRight;
        }
        else
        {
            spriteRenderer.flipX =
                swimsRight;
        }

        UpdateBitePointPosition();

        IsHooked = false;
        isEscaping = false;
        isInitialized = true;
    }

    private void Update()
    {
        if (
            !isInitialized ||
            IsHooked
        )
        {
            return;
        }

        MoveFish();
        CheckTargetReached();
    }

    private void UpdateBitePointPosition()
    {
        if (!bitePoint)
        {
            return;
        }

        Vector3 localPosition =
            bitePoint.localPosition;

        localPosition.x =
            Mathf.Abs(localPosition.x) *
            (swimsRight ? 1f : -1f);

        bitePoint.localPosition =
            localPosition;
    }

    public void SetBitePointFrame1()
    {
        SetBitePointPosition(
            bitePointFrame1
        );
    }

    public void SetBitePointFrame2()
    {
        SetBitePointPosition(
            bitePointFrame2
        );
    }

    public void SetBitePointFrame3()
    {
        SetBitePointPosition(
            bitePointFrame3
        );
    }

    private void SetBitePointPosition(
    Vector2 position)
    {
        if (!bitePoint)
        {
            return;
        }

        float xPosition =
            Mathf.Abs(position.x) *
            (swimsRight ? 1f : -1f);

        bitePoint.localPosition =
            new Vector3(
                xPosition,
                position.y,
                bitePoint.localPosition.z
            );
    }

    public void SetFishMouthPointFrame1()
    {
        SetFishMouthPointPosition(
            fishMouthPointFrame1
        );
    }

    public void SetFishMouthPointFrame2()
    {
        SetFishMouthPointPosition(
            fishMouthPointFrame2
        );
    }

    public void SetFishMouthPointFrame3()
    {
        SetFishMouthPointPosition(
            fishMouthPointFrame3
        );
    }

    private void SetFishMouthPointPosition(
    Vector2 position
    )
    {
        if (!fishMouthPoint)
        {
            return;
        }

        float xPosition =
            Mathf.Abs(position.x) *
            (swimsRight ? 1f : -1f);

        fishMouthPoint.localPosition =
            new Vector3(
                xPosition,
                position.y,
                fishMouthPoint.localPosition.z
            );
    }

    public void AttachToHook(Transform hookPoint)
    {
        if (IsHooked ||!hookPoint ||!fishMouthPoint)
        {
            return;
        }

        IsHooked = true;
        isEscaping = false;

        SetBiteColliderEnabled(false);

        if (animator)
        {
            animator.enabled = false;
        }

        Vector3 originalWorldScale =
            transform.lossyScale;

        transform.SetParent(
            null,
            true
        );

        transform.rotation =
            Quaternion.Euler(
                0f,
                0f,
                hookedRotationZ
            );

        /*
        * Der FishMouthPoint wurde bereits
        * passend zur aktuellen Schwimmrichtung
        * gesetzt. Deshalb hier NICHT noch
        * einmal spiegeln.
        */
        Vector3 actualMouthWorldPosition =
            transform.TransformPoint(
                fishMouthPoint.localPosition
            );

        Vector3 mouthOffset =
            actualMouthWorldPosition -
            transform.position;

        transform.position =
            hookPoint.position -
            mouthOffset;

        transform.SetParent(
            hookPoint,
            true
        );

        Vector3 parentScale =
            hookPoint.lossyScale;

        transform.localScale =
            new Vector3(
                parentScale.x != 0f
                    ? originalWorldScale.x /
                    parentScale.x
                    : originalWorldScale.x,

                parentScale.y != 0f
                    ? originalWorldScale.y /
                    parentScale.y
                    : originalWorldScale.y,

                parentScale.z != 0f
                    ? originalWorldScale.z /
                    parentScale.z
                    : originalWorldScale.z
            );
    }

    public void EscapeFromHook()
    {
        if (!IsHooked)
        {
            return;
        }

        transform.SetParent(
            null,
            true
        );

        transform.rotation =
            Quaternion.identity;

        IsHooked = false;
        isEscaping = true;
        isInitialized = true;

        SetBiteColliderEnabled(false);

        if (animator)
        {
            animator.enabled = true;
        }

        baseY =
            transform.position.y;

        movementSpeed =
            escapeSpeed;

        float direction =
            swimsRight ? 1f : -1f;

        targetPosition =
            new Vector3(
                transform.position.x +
                direction * escapeDistance,

                transform.position.y,
                transform.position.z
            );

        randomSwayOffset =
            Random.Range(
                0f,
                Mathf.PI * 2f
            );
    }

    public void RemoveHookedFish()
    {
        if (!IsHooked)
        {
            return;
        }

        IsHooked = false;

        onDespawn?.Invoke();

        Destroy(gameObject);
    }

    private void SetBiteColliderEnabled(
        bool enabled
    )
    {
        if (!bitePoint)
        {
            return;
        }

        Collider2D biteCollider =
            bitePoint.GetComponent<Collider2D>();

        if (biteCollider)
        {
            biteCollider.enabled =
                enabled;
        }
    }

    private void MoveFish()
    {
        float direction =
            swimsRight ? 1f : -1f;

        float nextX =
            transform.position.x +
            direction *
            movementSpeed *
            Time.deltaTime;

        float swayAmount =
            isEscaping
                ? verticalSwayAmount * 0.5f
                : verticalSwayAmount;

        float verticalSway =
            Mathf.Sin(
                Time.time *
                verticalSwaySpeed +
                randomSwayOffset
            ) * swayAmount;

        transform.position =
            new Vector3(
                nextX,
                baseY + verticalSway,
                transform.position.z
            );
    }

    private void CheckTargetReached()
    {
        bool targetReached =
            swimsRight
                ? transform.position.x >=
                targetPosition.x
                : transform.position.x <=
                targetPosition.x;

        if (!targetReached)
        {
            return;
        }

        onDespawn?.Invoke();

        Destroy(gameObject);
    }

    private void OnValidate()
    {
        requiredHookLevel =
            Mathf.Clamp(
                requiredHookLevel,
                1,
                3
            );

        escapeSpeed =
            Mathf.Max(
                0.1f,
                escapeSpeed
            );

        escapeDistance =
            Mathf.Max(
                1f,
                escapeDistance
            );

        minigameMovementSpeed =
            Mathf.Max(
                1f,
                minigameMovementSpeed
            );

        minigameMinimumWaitTime =
            Mathf.Max(
                0f,
                minigameMinimumWaitTime
            );

        minigameMaximumWaitTime =
            Mathf.Max(
                minigameMinimumWaitTime,
                minigameMaximumWaitTime
            );

        minigameMinimumTargetDistance =
            Mathf.Max(
                0f,
                minigameMinimumTargetDistance
            );

        minigameProgressIncreaseSpeed =
            Mathf.Max(
                0f,
                minigameProgressIncreaseSpeed
            );

        minigameProgressDecreaseSpeed =
            Mathf.Max(
                0f,
                minigameProgressDecreaseSpeed
            );
    }
}