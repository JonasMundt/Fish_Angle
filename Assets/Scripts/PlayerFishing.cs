using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerFishing : MonoBehaviour
{
    [Header("Spieler")]
    [SerializeField] private PlayerMovement playerMovement;

    [Header("Haken")]
    [SerializeField] private FishingHookController fishingHook;
    [SerializeField] private Transform rodTip;
    [SerializeField] private Transform hookWaterEntryPoint;

    [Header("Kamera")]
    [SerializeField] private SmoothCameraFollow cameraFollow;

    [Header("Minigame")]
    [SerializeField]
    private FishingMinigameUIController minigameUIController;

    [Header("Inventar")]
    [SerializeField] private FishInventory fishInventory;
    [SerializeField] private InventoryMessageUI inventoryMessageUI;

    [Header("Angeln beenden")]
    [SerializeField] private KeyCode stopFishingKey = KeyCode.E;
    [SerializeField] private float requiredHoldDuration = 1f;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip fishingCastClip;

    private Animator animator;

    private float stopHoldTimer;

    private bool hookWasReleased;
    private bool canStartStopHold;
    private bool isStoppingFishing;

    public bool IsFishing { get; private set; }

    public float StopHoldProgress
    {
        get
        {
            if (requiredHoldDuration <= 0f)
            {
                return 0f;
            }

            return Mathf.Clamp01(
                stopHoldTimer /
                requiredHoldDuration
            );
        }
    }

    private void Awake()
    {
        animator =
            GetComponent<Animator>();

        if (!playerMovement)
        {
            playerMovement =
                GetComponent<PlayerMovement>();
        }

        if (
            !cameraFollow &&
            Camera.main
        )
        {
            cameraFollow =
                Camera.main.GetComponent<
                    SmoothCameraFollow
                >();
        }
    }

    private void Update()
    {
        HandleStopFishingInput();
    }

    public void StartFishing()
    {
        if (
            fishInventory &&
            fishInventory.IsFull
        )
        {
            if (inventoryMessageUI)
            {
                inventoryMessageUI
                    .ShowInventoryFullMessage();
            }
            else
            {
                Debug.Log(
                    "Dein Fischinventar ist voll!",
                    this
                );
            }

            return;
        }

        if (
            IsFishing ||
            isStoppingFishing
        )
        {
            return;
        }

        if (
            !fishingHook ||
            !rodTip ||
            !hookWaterEntryPoint
        )
        {
            Debug.LogError(
                "FishingHook, RodTip oder HookWaterEntryPoint fehlt.",
                this
            );

            return;
        }

        IsFishing = true;

        hookWasReleased = false;
        canStartStopHold = false;
        isStoppingFishing = false;
        stopHoldTimer = 0f;

        if (minigameUIController)
        {
            minigameUIController
                .HideMinigame();
        }

        if (playerMovement)
        {
            playerMovement
                .SetMovementEnabled(false);
        }

        animator.SetBool(
            "IsWalking",
            false
        );

        animator.ResetTrigger(
            "StopFishing"
        );

        animator.ResetTrigger(
            "StartFishing"
        );

        animator.SetTrigger(
            "StartFishing"
        );
    }

    private void HandleStopFishingInput()
    {
        if (
            !IsFishing ||
            isStoppingFishing ||
            !hookWasReleased
        )
        {
            stopHoldTimer = 0f;
            return;
        }

        if (
            minigameUIController &&
            minigameUIController
                .IsMinigameActive
        )
        {
            stopHoldTimer = 0f;
            return;
        }

        if (!canStartStopHold)
        {
            if (
                !Input.GetKey(
                    stopFishingKey
                )
            )
            {
                canStartStopHold = true;
            }

            stopHoldTimer = 0f;
            return;
        }

        if (
            Input.GetKey(
                stopFishingKey
            )
        )
        {
            stopHoldTimer +=
                Time.deltaTime;

            if (
                stopHoldTimer >=
                requiredHoldDuration
            )
            {
                BeginStopFishing();
            }
        }
        else
        {
            stopHoldTimer = 0f;
        }
    }

    public void ReleaseFishingHook()
    {
        if (
            !IsFishing ||
            isStoppingFishing ||
            hookWasReleased
        )
        {
            return;
        }

        if (
            !fishingHook ||
            !rodTip ||
            !hookWaterEntryPoint
        )
        {
            Debug.LogError(
                "FishingHook, RodTip oder HookWaterEntryPoint fehlt.",
                this
            );

            return;
        }

        hookWasReleased = true;

        fishingHook.StartCast(
            rodTip,
            hookWaterEntryPoint.position
        );

        if (audioSource && fishingCastClip)
        {
            audioSource.PlayOneShot(fishingCastClip);
        }
    }

    private void BeginStopFishing()
    {
        if (
            !IsFishing ||
            isStoppingFishing
        )
        {
            return;
        }

        isStoppingFishing = true;
        stopHoldTimer = 0f;

        if (minigameUIController)
        {
            minigameUIController
                .HideMinigame();
        }

        if (fishingHook)
        {
            fishingHook.StopFishing();
        }

        if (cameraFollow)
        {
            cameraFollow
                .FollowPlayer(transform);
        }

        animator.ResetTrigger(
            "StartFishing"
        );

        animator.ResetTrigger(
            "StopFishing"
        );

        animator.SetTrigger(
            "StopFishing"
        );
    }

    public void FinishStopFishing()
    {
        animator.ResetTrigger(
            "StartFishing"
        );

        animator.ResetTrigger(
            "StopFishing"
        );

        if (minigameUIController)
        {
            minigameUIController
                .HideMinigame();
        }

        IsFishing = false;

        hookWasReleased = false;
        canStartStopHold = false;
        isStoppingFishing = false;
        stopHoldTimer = 0f;

        if (playerMovement)
        {
            playerMovement
                .SetMovementEnabled(true);
        }
    }

    /*
     * Wird nach einem erfolgreichen Fang aufgerufen.
     * Das Angeln wird nur beendet, wenn das
     * Inventar jetzt voll ist.
     */
    public void FinishSuccessfulCatch()
    {
        if (
            !IsFishing ||
            isStoppingFishing
        )
        {
            return;
        }

        if (
            fishInventory &&
            fishInventory.IsFull
        )
        {
            BeginStopFishing();
        }
    }

    public void StopFishingImmediately()
    {
        if (
            !IsFishing &&
            !isStoppingFishing
        )
        {
            return;
        }

        if (minigameUIController)
        {
            minigameUIController
                .HideMinigame();
        }

        if (fishingHook)
        {
            fishingHook.StopFishing();
        }

        if (cameraFollow)
        {
            cameraFollow
                .FollowPlayer(transform);
        }

        IsFishing = false;

        hookWasReleased = false;
        canStartStopHold = false;
        isStoppingFishing = false;
        stopHoldTimer = 0f;

        animator.ResetTrigger(
            "StartFishing"
        );

        animator.ResetTrigger(
            "StopFishing"
        );

        animator.Play(
            "MainCharacter_Idle"
        );

        if (playerMovement)
        {
            playerMovement
                .SetMovementEnabled(true);
        }
    }
}