using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(SpriteRenderer))]
public class FishingHookController : MonoBehaviour
{
    [Header("Bewegung")]
    [SerializeField] private float movementSpeed = 3f;

    [Header("Haken-Sprites")]
    [SerializeField] private FishingUpgradeState fishingUpgradeState;
    [SerializeField] private Sprite hookLevel1Sprite;
    [SerializeField] private Sprite hookLevel2Sprite;
    [SerializeField] private Sprite hookLevel3Sprite;

    [Header("Bewegungsgrenzen")]
    [SerializeField] private BoxCollider2D movementBounds;

    [Header("Schnurlänge")]
    [Min(0.1f)]
    [SerializeField] private float maximumLineDepth = 8f;

    [Header("Auswurf")]
    [SerializeField] private float castDuration = 0.8f;
    [SerializeField] private float castArcHeight = 1.5f;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip waterSplashClip;
    [SerializeField] private AudioClip fishBiteClip;
    [SerializeField] private AudioClip catchSuccessClip;
    [SerializeField] private AudioClip fishEscapeClip;
    [SerializeField] private AudioSource ambienceAudioSource;
    [SerializeField] private AudioClip waterAmbienceClip;

    [Header("Schnur")]
    [SerializeField] private Transform hookLinePoint;

    [Header("Gefangener Fisch")]
    [SerializeField] private Transform hookedFishPoint;

    [Header("Kamera")]
    [SerializeField] private SmoothCameraFollow cameraFollow;

    [Header("Minigame")]
    [SerializeField]
    private FishingMinigameUIController minigameUIController;

    [Header("Erfolgreicher Fang")]
    [SerializeField] private FishInventory fishInventory;
    [SerializeField] private PlayerFishing playerFishing;
    [SerializeField] private GameObject sellerObject;
    [SerializeField] private InventoryMessageUI inventoryMessageUI;
    [SerializeField] private SellerInteraction sellerInteraction;
    [SerializeField] private FishCollection fishCollection;

    private Rigidbody2D rb;
    private LineRenderer fishingLine;
    private SpriteRenderer hookSprite;

    private Transform rodTip;
    private Vector2 movementInput;

    private FishSwimmer hookedFish;

    private bool hookCanMove;
    private bool fishingActive;
    private bool successIsBeingHandled;

    private float minX;
    private float maxX;
    private float minY;
    private float maxY;

    private float waterSurfaceY;
    private bool waterSurfaceWasSet;

    public float MaximumLineDepth =>
        maximumLineDepth;

    public float MovementSpeed =>
    movementSpeed;

    public bool HasHookedFish =>
        hookedFish != null;

    private void Awake()
    {
        EnsureReferences();
        HideHookVisuals();
    }

    private void OnEnable()
    {
        if (fishingUpgradeState)
        {
            fishingUpgradeState
                .UpgradesChanged +=
                UpdateHookSprite;
        }
    }

    private void OnDisable()
    {
        if (fishingUpgradeState)
        {
            fishingUpgradeState
                .UpgradesChanged -=
                UpdateHookSprite;
        }
    }

    private void Start()
    {
        CalculateMovementBounds();
        UpdateHookSprite();
    }

    private void EnsureReferences()
    {
        if (!rb)
        {
            rb =
                GetComponent<Rigidbody2D>();
        }

        if (!fishingLine)
        {
            fishingLine =
                GetComponent<LineRenderer>();
        }

        if (!hookSprite)
        {
            hookSprite =
                GetComponent<SpriteRenderer>();
        }
    }

    private void UpdateHookSprite()
    {
        EnsureReferences();

        if (
            !hookSprite ||
            !fishingUpgradeState
        )
        {
            return;
        }

        switch (
            fishingUpgradeState.HookLevel
        )
        {
            case 1:

                if (hookLevel1Sprite)
                {
                    hookSprite.sprite =
                        hookLevel1Sprite;
                }

                break;

            case 2:

                if (hookLevel2Sprite)
                {
                    hookSprite.sprite =
                        hookLevel2Sprite;
                }

                break;

            case 3:

                if (hookLevel3Sprite)
                {
                    hookSprite.sprite =
                        hookLevel3Sprite;
                }

                break;
        }
    }

    private void Update()
    {
        if (!hookCanMove)
        {
            movementInput =
                Vector2.zero;

            return;
        }

        movementInput =
            new Vector2(
                Input.GetAxisRaw(
                    "Horizontal"
                ),
                Input.GetAxisRaw(
                    "Vertical"
                )
            ).normalized;
    }

    private void FixedUpdate()
    {
        if (!hookCanMove)
        {
            if (rb)
            {
                rb.linearVelocity =
                    Vector2.zero;
            }

            return;
        }

        Vector2 nextPosition =
            rb.position +
            movementInput *
            movementSpeed *
            Time.fixedDeltaTime;

        nextPosition.x =
            Mathf.Clamp(
                nextPosition.x,
                minX,
                maxX
            );

        nextPosition.y =
            Mathf.Clamp(
                nextPosition.y,
                minY,
                maxY
            );

        rb.MovePosition(
            nextPosition
        );
    }

    private void LateUpdate()
    {
        if (
            !fishingActive ||
            !rodTip ||
            !hookLinePoint ||
            !fishingLine ||
            !fishingLine.enabled
        )
        {
            return;
        }

        UpdateFishingLinePositions();
    }

    public void StartCast(
        Transform newRodTip,
        Vector3 waterEntryPosition
    )
    {
        EnsureReferences();
        UpdateHookSprite();

        if (
            !newRodTip ||
            !hookLinePoint
        )
        {
            Debug.LogError(
                "RodTip oder HookLinePoint wurde nicht zugewiesen.",
                this
            );

            return;
        }

        if (!movementBounds)
        {
            Debug.LogError(
                "HookMovementBounds wurde nicht zugewiesen.",
                this
            );

            return;
        }

        if (!hookedFishPoint)
        {
            Debug.LogError(
                "HookedFishPoint wurde nicht zugewiesen.",
                this
            );

            return;
        }

        rodTip =
            newRodTip;

        waterSurfaceY =
            waterEntryPosition.y;

        waterSurfaceWasSet = true;

        CalculateMovementBounds();

        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }

        transform.position =
            rodTip.position;

        rb.position =
            rodTip.position;

        rb.linearVelocity =
            Vector2.zero;

        rb.simulated = true;

        fishingActive = true;
        hookCanMove = false;
        successIsBeingHandled = false;

        movementInput =
            Vector2.zero;

        hookedFish = null;

        if (minigameUIController)
        {
            minigameUIController
                .HideMinigame();
        }

        ShowHookVisuals();
        UpdateFishingLinePositions();

        StopAllCoroutines();

        StartCoroutine(
            CastHook(
                waterEntryPosition
            )
        );
    }

    private IEnumerator CastHook(
        Vector3 targetPosition
    )
    {
        Vector3 startPosition =
            rodTip.position;

        float elapsedTime = 0f;

        while (
            elapsedTime <
            castDuration
        )
        {
            elapsedTime +=
                Time.deltaTime;

            float progress =
                Mathf.Clamp01(
                    elapsedTime /
                    castDuration
                );

            Vector3 currentPosition =
                Vector3.Lerp(
                    startPosition,
                    targetPosition,
                    progress
                );

            float arc =
                Mathf.Sin(
                    progress *
                    Mathf.PI
                ) *
                castArcHeight;

            currentPosition.y += arc;

            transform.position =
                currentPosition;

            rb.position =
                currentPosition;

            yield return null;
        }

        Vector2 clampedTarget =
            new Vector2(
                Mathf.Clamp(
                    targetPosition.x,
                    minX,
                    maxX
                ),
                Mathf.Clamp(
                    targetPosition.y,
                    minY,
                    maxY
                )
            );

        transform.position =
            clampedTarget;

        rb.position =
            clampedTarget;

        if (audioSource && waterSplashClip)
        {
            audioSource.PlayOneShot(waterSplashClip);
        }

        if (ambienceAudioSource && waterAmbienceClip)
        {
            ambienceAudioSource.clip = waterAmbienceClip;
            ambienceAudioSource.loop = true;
            ambienceAudioSource.Play();
        }

        if (cameraFollow)
        {
            cameraFollow.FollowHook(transform);
        }

        hookCanMove = true;
    }

    public bool CanDetectFish()
    {
        return
            fishingActive &&
            hookCanMove &&
            hookedFish == null;
    }

    public bool TryHookFish(
        FishSwimmer fish
    )
    {
        if (
            !fish ||
            !CanDetectFish()
        )
        {
            return false;
        }

        if (
            !fishingUpgradeState ||
            !fishingUpgradeState
                .HasHookLevel(
                    fish.RequiredHookLevel
                )
        )
        {
            Debug.Log(
                "Dieser Haken ist für diesen Fisch noch nicht stark genug.",
                fish.gameObject
            );

            return false;
        }

        hookedFish = fish;

        hookCanMove = false;
        movementInput =
            Vector2.zero;

        if (rb)
        {
            rb.linearVelocity =
                Vector2.zero;
        }

        fish.AttachToHook(
            hookedFishPoint
        );

        if (audioSource && fishBiteClip)
        {
            audioSource.PlayOneShot(fishBiteClip);
        }

        if (minigameUIController)
        {
            minigameUIController
                .ShowMinigame(
                    hookedFish
                );
        }
        else
        {
            Debug.LogWarning(
                "MinigameUIController wurde am FishingHook nicht zugewiesen.",
                this
            );
        }

        return true;
    }

    public void HandleMinigameSuccess()
    {
        if (successIsBeingHandled || !fishingActive || hookedFish == null)
        {
            return;
        }

        successIsBeingHandled = true;
        hookCanMove = false;
        movementInput =
            Vector2.zero;

        if (minigameUIController)
        {
            minigameUIController
                .HideMinigame();
        }

        if (!fishInventory)
        {
            Debug.LogError(
                "FishInventory wurde am FishingHook nicht zugewiesen.",
                this
            );

            successIsBeingHandled =
                false;

            return;
        }

        if (fishInventory.IsFull)
        {
            if (inventoryMessageUI)
            {
                inventoryMessageUI
                    .ShowInventoryFullMessage();
            }

            EscapeFishBecauseInventoryIsFull();
            return;
        }

        bool fishWasAdded =
            TryAddHookedFishToInventory();

        if (!fishWasAdded)
        {
            if (inventoryMessageUI)
            {
                inventoryMessageUI
                    .ShowInventoryFullMessage();
            }

            EscapeFishBecauseInventoryIsFull();
            return;
        }

        if (audioSource && catchSuccessClip)
        {
            audioSource.PlayOneShot(catchSuccessClip);
        }

        RegisterCatchInCollection();

        if (hookedFish != null && hookedFish.FishType == FishType.Catshark && sellerInteraction)
        {
        sellerInteraction.QueueCatsharkDialogue();
        }

        if (hookedFish != null &&hookedFish.FishType == FishType.CrystalEel &&sellerInteraction)
        {
        sellerInteraction.QueueCrystalEelDialogue();
        }

        if (hookedFish != null && hookedFish.FishType == FishType.PrismTrout &&sellerInteraction)
        {
        sellerInteraction.QueuePrismTroutDialogue();
        }

        if (sellerObject &&! sellerObject.activeSelf)
        {
            sellerObject.SetActive(true);
        }

        Debug.Log(
            "Fisch gefangen! Inventar: " +
            fishInventory.CurrentFishCount +
            " / " +
            fishInventory.MaximumFishCount,
            this
        );

        /*
         * Durch diesen Fang wurde das Inventar voll.
         * Jetzt wird der gesamte Angelvorgang beendet.
         */
        if (fishInventory.IsFull)
        {
            if (playerFishing)
            {
                playerFishing
                    .FinishSuccessfulCatch();
            }
            else
            {
                Debug.LogError(
                    "PlayerFishing wurde am FishingHook nicht zugewiesen.",
                    this
                );

                StopFishing();
            }

            return;
        }

        /*
         * Es ist noch Platz:
         * Fisch entfernen und mit demselben
         * Haken direkt weiterangeln.
         */
        ContinueFishingAfterSuccessfulCatch();
    }

    private bool TryAddHookedFishToInventory()
    {
        if (
            !fishInventory ||
            hookedFish == null
        )
        {
            return false;
        }

        switch (hookedFish.FishType)
        {
            case FishType.Perch:

                return
                    fishInventory
                        .TryAddPerch();

            case FishType.Catshark:

                return
                    fishInventory
                        .TryAddCatshark();

            case FishType.CrystalEel:

                return
                    fishInventory
                        .TryAddCrystalEel();

            case FishType.PrismTrout:

                return
                    fishInventory
                        .TryAddPrismTrout();

            default:

                Debug.LogWarning(
                    "Für diese Fischart gibt es noch keine Inventarlogik.",
                    hookedFish.gameObject
                );

                return false;
        }
    }

    private void RegisterCatchInCollection()
    {
        if (!fishCollection || hookedFish == null)
        {
            return;
        }

        switch (hookedFish.FishType)
        {
            case FishType.Perch:
                fishCollection.RegisterPerchCatch();
                break;

            case FishType.Catshark:
                fishCollection.RegisterCatsharkCatch();
                break;

            case FishType.CrystalEel:
                fishCollection.RegisterCrystalEelCatch();
                break;
            
            case FishType.PrismTrout:
                fishCollection.RegisterPrismTroutCatch();
                break;
        }
    }

    private void ContinueFishingAfterSuccessfulCatch()
    {
        if (hookedFish != null)
        {
            hookedFish
                .RemoveHookedFish();

            hookedFish = null;
        }

        successIsBeingHandled = false;

        hookCanMove = true;

        movementInput =
            Vector2.zero;

        if (rb)
        {
            rb.simulated = true;

            rb.linearVelocity =
                Vector2.zero;
        }

        Debug.Log(
            "Fisch gefangen. Der Haken bleibt im Wasser.",
            this
        );
    }

    private void EscapeFishBecauseInventoryIsFull()
    {
        if (hookedFish == null)
        {
            successIsBeingHandled =
                false;

            return;
        }

        FishSwimmer escapingFish =
            hookedFish;

        hookedFish = null;

        escapingFish
            .EscapeFromHook();

        hookCanMove = true;

        movementInput =
            Vector2.zero;

        successIsBeingHandled =
            false;

        if (rb)
        {
            rb.simulated = true;

            rb.linearVelocity =
                Vector2.zero;
        }

        Debug.Log(
            "Inventar voll: Der Fisch schwimmt wieder weg.",
            this
        );
    }

    public void HandleMinigameFailure()
    {
        if (
            !fishingActive ||
            hookedFish == null
        )
        {
            return;
        }

        FishSwimmer escapingFish =
            hookedFish;

        hookedFish = null;

        escapingFish
            .EscapeFromHook();

        if (audioSource && fishEscapeClip)
        {
            audioSource.PlayOneShot(fishEscapeClip);
        }

        if (minigameUIController)
        {
            minigameUIController
                .HideMinigame();
        }

        hookCanMove = true;

        movementInput =
            Vector2.zero;

        successIsBeingHandled =
            false;

        if (rb)
        {
            rb.simulated = true;

            rb.linearVelocity =
                Vector2.zero;
        }

        Debug.Log(
            "Der Fisch ist entkommen. Der Haken bleibt im Wasser.",
            this
        );
    }

    public void SetHookMovementEnabled(
        bool enabled
    )
    {
        if (
            !fishingActive ||
            hookedFish != null
        )
        {
            hookCanMove = false;
            return;
        }

        hookCanMove =
            enabled;

        movementInput =
            Vector2.zero;

        if (
            !enabled &&
            rb
        )
        {
            rb.linearVelocity =
                Vector2.zero;
        }
    }

    public void StopFishing()
    {
        StopAllCoroutines();

        hookCanMove = false;
        fishingActive = false;
        successIsBeingHandled = false;

        movementInput =
            Vector2.zero;

        if (minigameUIController)
        {
            minigameUIController
                .HideMinigame();
        }

        if (ambienceAudioSource)
        {
            ambienceAudioSource.Stop();
        }

        if (hookedFish != null)
        {
            hookedFish
                .RemoveHookedFish();

            hookedFish = null;
        }

        if (rb)
        {
            rb.linearVelocity =
                Vector2.zero;

            rb.simulated = false;
        }

        HideHookVisuals();
    }

    public void SetMovementSpeed(float newSpeed)
    {
        movementSpeed =
            Mathf.Max(
                0.1f,
                newSpeed
            );
    }
    public void SetMaximumLineDepth(
        float newDepth
    )
    {
        maximumLineDepth =
            Mathf.Max(
                0.1f,
                newDepth
            );

        CalculateMovementBounds();
        ClampHookToCurrentBounds();
    }

    public void AddMaximumLineDepth(
        float additionalDepth
    )
    {
        SetMaximumLineDepth(
            maximumLineDepth +
            additionalDepth
        );
    }

    private void ShowHookVisuals()
    {
        if (hookSprite)
        {
            hookSprite.enabled =
                true;
        }

        if (fishingLine)
        {
            fishingLine.positionCount =
                2;

            fishingLine.enabled =
                true;
        }
    }

    private void HideHookVisuals()
    {
        if (hookSprite)
        {
            hookSprite.enabled =
                false;
        }

        if (fishingLine)
        {
            fishingLine.positionCount =
                2;

            fishingLine.enabled =
                false;
        }

        if (rb)
        {
            rb.simulated =
                false;
        }
    }

    private void UpdateFishingLinePositions()
    {
        fishingLine.SetPosition(
            0,
            rodTip.position
        );

        fishingLine.SetPosition(
            1,
            hookLinePoint.position
        );
    }

    private void CalculateMovementBounds()
    {
        if (!movementBounds)
        {
            return;
        }

        Bounds bounds =
            movementBounds.bounds;

        minX = bounds.min.x;
        maxX = bounds.max.x;
        maxY = bounds.max.y;

        if (!waterSurfaceWasSet)
        {
            minY = bounds.min.y;
            return;
        }

        float lineDepthLimit =
            waterSurfaceY -
            maximumLineDepth;

        minY =
            Mathf.Max(
                bounds.min.y,
                lineDepthLimit
            );
    }

    private void ClampHookToCurrentBounds()
    {
        if (!rb)
        {
            return;
        }

        Vector2 clampedPosition =
            new Vector2(
                Mathf.Clamp(
                    rb.position.x,
                    minX,
                    maxX
                ),
                Mathf.Clamp(
                    rb.position.y,
                    minY,
                    maxY
                )
            );

        transform.position =
            clampedPosition;

        rb.position =
            clampedPosition;
    }

    private void OnValidate()
    {
        maximumLineDepth =
            Mathf.Max(
                0.1f,
                maximumLineDepth
            );
    }
}