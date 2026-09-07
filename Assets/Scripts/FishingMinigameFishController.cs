using UnityEngine;

public class FishingMinigameFishController : MonoBehaviour
{
    [Header("UI-Referenzen")]
    [SerializeField] private RectTransform fishingArea;
    [SerializeField] private RectTransform fishIcon;

    [Header("Bewegung")]
    [Min(1f)]
    [SerializeField] private float movementSpeed = 90f;

    [Min(0f)]
    [SerializeField] private float minimumWaitTime = 0.4f;

    [Min(0f)]
    [SerializeField] private float maximumWaitTime = 1.2f;

    [Min(0f)]
    [SerializeField] private float minimumTargetDistance = 40f;

    [Min(0.1f)]
    [SerializeField] private float targetTolerance = 2f;

    [Header("Startposition")]
    [Range(0f, 1f)]
    [SerializeField] private float startingHeight = 0.5f;

    private float targetY;
    private float waitTimer;

    private bool hasTarget;
    private bool isWaiting;

    private void Awake()
    {
        if (!fishIcon)
        {
            fishIcon =
                GetComponent<RectTransform>();
        }
    }

    private void OnEnable()
    {
        ResetFish();
    }

    private void Update()
    {
        if (
            !fishingArea ||
            !fishIcon
        )
        {
            return;
        }

        if (isWaiting)
        {
            waitTimer -=
                Time.deltaTime;

            if (waitTimer <= 0f)
            {
                isWaiting = false;

                ChooseNewTarget();
            }

            return;
        }

        if (!hasTarget)
        {
            ChooseNewTarget();
        }

        MoveTowardsTarget();
    }

    public void ApplyFishSettings(
        FishSwimmer fish
    )
    {
        if (!fish)
        {
            return;
        }

        movementSpeed =
            Mathf.Max(
                1f,
                fish.MinigameMovementSpeed
            );

        minimumWaitTime =
            Mathf.Max(
                0f,
                fish.MinigameMinimumWaitTime
            );

        maximumWaitTime =
            Mathf.Max(
                minimumWaitTime,
                fish.MinigameMaximumWaitTime
            );

        minimumTargetDistance =
            Mathf.Max(
                0f,
                fish.MinigameMinimumTargetDistance
            );
    }

    public void ResetFish()
    {
        if (
            !fishingArea ||
            !fishIcon
        )
        {
            return;
        }

        float minimumY;
        float maximumY;

        GetVerticalLimits(
            out minimumY,
            out maximumY
        );

        Vector2 position =
            fishIcon.anchoredPosition;

        position.y =
            Mathf.Lerp(
                minimumY,
                maximumY,
                startingHeight
            );

        fishIcon.anchoredPosition =
            position;

        hasTarget = false;
        isWaiting = false;
        waitTimer = 0f;

        ChooseNewTarget();
    }

    private void MoveTowardsTarget()
    {
        Vector2 position =
            fishIcon.anchoredPosition;

        position.y =
            Mathf.MoveTowards(
                position.y,
                targetY,
                movementSpeed *
                Time.deltaTime
            );

        fishIcon.anchoredPosition =
            position;

        if (
            Mathf.Abs(
                position.y -
                targetY
            ) <=
            targetTolerance
        )
        {
            position.y =
                targetY;

            fishIcon.anchoredPosition =
                position;

            hasTarget = false;
            isWaiting = true;

            waitTimer =
                Random.Range(
                    minimumWaitTime,
                    maximumWaitTime
                );
        }
    }

    private void ChooseNewTarget()
    {
        float minimumY;
        float maximumY;

        GetVerticalLimits(
            out minimumY,
            out maximumY
        );

        float currentY =
            fishIcon.anchoredPosition.y;

        float availableDistance =
            maximumY - minimumY;

        float clampedMinimumDistance =
            Mathf.Min(
                minimumTargetDistance,
                availableDistance
            );

        /*
         * Mehrere Versuche, ein ausreichend
         * weit entferntes Ziel zu finden.
         */
        for (int i = 0; i < 12; i++)
        {
            float candidateY =
                Random.Range(
                    minimumY,
                    maximumY
                );

            if (
                Mathf.Abs(
                    candidateY -
                    currentY
                ) >=
                clampedMinimumDistance
            )
            {
                targetY =
                    candidateY;

                hasTarget = true;

                return;
            }
        }

        /*
         * Fallback:
         * Falls durch Zufall kein passendes
         * Ziel gefunden wurde, wird die weiter
         * entfernte Seite gewählt.
         */
        float distanceToBottom =
            Mathf.Abs(
                currentY -
                minimumY
            );

        float distanceToTop =
            Mathf.Abs(
                maximumY -
                currentY
            );

        targetY =
            distanceToTop >=
            distanceToBottom
                ? maximumY
                : minimumY;

        hasTarget = true;
    }

    private void GetVerticalLimits(
        out float minimumY,
        out float maximumY
    )
    {
        minimumY = 0f;

        maximumY =
            fishingArea.rect.height -
            fishIcon.rect.height;

        maximumY =
            Mathf.Max(
                minimumY,
                maximumY
            );
    }

    private void OnValidate()
    {
        minimumWaitTime =
            Mathf.Max(
                0f,
                minimumWaitTime
            );

        maximumWaitTime =
            Mathf.Max(
                minimumWaitTime,
                maximumWaitTime
            );

        movementSpeed =
            Mathf.Max(
                1f,
                movementSpeed
            );

        minimumTargetDistance =
            Mathf.Max(
                0f,
                minimumTargetDistance
            );

        targetTolerance =
            Mathf.Max(
                0.1f,
                targetTolerance
            );
    }
}