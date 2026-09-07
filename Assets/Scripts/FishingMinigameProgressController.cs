using UnityEngine;

public class FishingMinigameProgressController : MonoBehaviour
{
    [Header("Minigame")]
    [SerializeField]
    private FishingMinigameUIController minigameUIController;

    [SerializeField]
    private FishingHookController fishingHook;

    [Header("Fang-Erkennung")]
    [SerializeField] private RectTransform fishIcon;
    [SerializeField] private RectTransform catchBar;

    [Header("Fortschrittsanzeige")]
    [SerializeField] private RectTransform progressFrame;
    [SerializeField] private RectTransform progressFill;

    [Header("Fortschritt")]
    [Range(0f, 1f)]
    [SerializeField] private float startingProgress = 0.35f;

    [Min(0f)]
    [SerializeField] private float progressIncreaseSpeed = 0.25f;

    [Min(0f)]
    [SerializeField] private float progressDecreaseSpeed = 0.12f;

    [Header("Niederlage")]
    [Min(0f)]
    [SerializeField] private float timeAtZeroBeforeFailure = 3f;

    [Header("Testanzeige")]
    [SerializeField] private bool showDebugMessages;

    private float currentProgress;
    private float maximumFillHeight;
    private float zeroProgressTimer;

    private bool minigameWasActive;
    private bool resultWasTriggered;

    public float CurrentProgress =>
        currentProgress;

    public bool IsFishInsideCatchBar
    {
        get;
        private set;
    }

    private void Awake()
    {
        CalculateMaximumFillHeight();
        ResetProgress();
    }

    private void Update()
    {
        bool minigameIsActive =
            minigameUIController &&
            minigameUIController.IsMinigameActive;

        if (
            minigameIsActive &&
            !minigameWasActive
        )
        {
            ResetProgress();
        }

        minigameWasActive =
            minigameIsActive;

        if (
            !minigameIsActive ||
            resultWasTriggered
        )
        {
            return;
        }

        if (
            !fishIcon ||
            !catchBar ||
            !progressFrame ||
            !progressFill
        )
        {
            return;
        }

        IsFishInsideCatchBar =
            CheckFishInsideCatchBar();

        UpdateProgress();
        UpdateProgressVisual();

        if (currentProgress >= 1f)
        {
            TriggerSuccess();
            return;
        }

        UpdateFailureTimer();
    }

    public void ApplyFishSettings(
        FishSwimmer fish
    )
    {
        if (!fish)
        {
            return;
        }

        progressIncreaseSpeed =
            Mathf.Max(
                0f,
                fish.MinigameProgressIncreaseSpeed
            );

        progressDecreaseSpeed =
            Mathf.Max(
                0f,
                fish.MinigameProgressDecreaseSpeed
            );
    }

    public void ResetProgress()
    {
        CalculateMaximumFillHeight();

        currentProgress =
            Mathf.Clamp01(
                startingProgress
            );

        zeroProgressTimer = 0f;
        resultWasTriggered = false;
        IsFishInsideCatchBar = false;

        UpdateProgressVisual();
    }

    private bool CheckFishInsideCatchBar()
    {
        Vector3[] fishCorners =
            new Vector3[4];

        Vector3[] catchBarCorners =
            new Vector3[4];

        fishIcon.GetWorldCorners(
            fishCorners
        );

        catchBar.GetWorldCorners(
            catchBarCorners
        );

        float fishBottom =
            fishCorners[0].y;

        float fishTop =
            fishCorners[1].y;

        float catchBarBottom =
            catchBarCorners[0].y;

        float catchBarTop =
            catchBarCorners[1].y;

        return
            fishTop >= catchBarBottom &&
            fishBottom <= catchBarTop;
    }

    private void UpdateProgress()
    {
        if (IsFishInsideCatchBar)
        {
            currentProgress +=
                progressIncreaseSpeed *
                Time.deltaTime;
        }
        else
        {
            currentProgress -=
                progressDecreaseSpeed *
                Time.deltaTime;
        }

        currentProgress =
            Mathf.Clamp01(
                currentProgress
            );

        if (showDebugMessages)
        {
            Debug.Log(
                "Fangfortschritt: " +
                Mathf.RoundToInt(
                    currentProgress *
                    100f
                ) +
                "% | Null-Timer: " +
                zeroProgressTimer.ToString("F1"),
                this
            );
        }
    }

    private void UpdateFailureTimer()
    {
        if (currentProgress <= 0f)
        {
            zeroProgressTimer +=
                Time.deltaTime;

            if (
                zeroProgressTimer >=
                timeAtZeroBeforeFailure
            )
            {
                TriggerFailure();
            }

            return;
        }

        zeroProgressTimer = 0f;
    }

    private void TriggerSuccess()
    {
        if (resultWasTriggered)
        {
            return;
        }

        resultWasTriggered = true;
        zeroProgressTimer = 0f;

        if (fishingHook)
        {
            fishingHook
                .HandleMinigameSuccess();
        }
        else
        {
            Debug.LogError(
                "FishingHook wurde im ProgressController nicht zugewiesen.",
                this
            );

            if (minigameUIController)
            {
                minigameUIController
                    .HideMinigame();
            }
        }
    }

    private void TriggerFailure()
    {
        if (resultWasTriggered)
        {
            return;
        }

        resultWasTriggered = true;
        zeroProgressTimer = 0f;

        if (fishingHook)
        {
            fishingHook
                .HandleMinigameFailure();
        }
        else
        {
            Debug.LogError(
                "FishingHook wurde im ProgressController nicht zugewiesen.",
                this
            );

            if (minigameUIController)
            {
                minigameUIController
                    .HideMinigame();
            }
        }
    }

    private void UpdateProgressVisual()
    {
        if (!progressFill)
        {
            return;
        }

        float newHeight =
            maximumFillHeight *
            currentProgress;

        progressFill.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Vertical,
            newHeight
        );
    }

    private void CalculateMaximumFillHeight()
    {
        if (!progressFrame)
        {
            return;
        }

        maximumFillHeight =
            progressFrame.rect.height;
    }

    private void OnValidate()
    {
        startingProgress =
            Mathf.Clamp01(
                startingProgress
            );

        progressIncreaseSpeed =
            Mathf.Max(
                0f,
                progressIncreaseSpeed
            );

        progressDecreaseSpeed =
            Mathf.Max(
                0f,
                progressDecreaseSpeed
            );

        timeAtZeroBeforeFailure =
            Mathf.Max(
                0f,
                timeAtZeroBeforeFailure
            );
    }
}