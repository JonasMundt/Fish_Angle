using UnityEngine;
using UnityEngine.UI;

public class FishingMinigameUIController : MonoBehaviour
{
    [Header("Minigame")]
    [SerializeField]
    private GameObject fishingMinigameUI;

    [Header("UI-Reihenfolge")]
    [SerializeField]
    private RectTransform fishIcon;

    [Header("Fisch-Icon")]
    [SerializeField]
    private Image fishIconImage;

    [Header("Minigame Controller")]
    [SerializeField]
    private FishingMinigameFishController fishController;

    [SerializeField]
    private FishingMinigameProgressController progressController;

    public bool IsMinigameActive
    {
        get
        {
            return
                fishingMinigameUI &&
                fishingMinigameUI.activeSelf;
        }
    }

    private void Awake()
    {
        if (!fishIconImage && fishIcon)
        {
            fishIconImage =
                fishIcon.GetComponent<Image>();
        }

        /*
         * Falls im Inspector kein Controller
         * eingetragen wurde, wird er direkt
         * vom FishIcon geholt.
         */
        if (!fishController && fishIcon)
        {
            fishController =
                fishIcon.GetComponent<
                    FishingMinigameFishController
                >();
        }

        HideMinigame();
    }

    public void ShowMinigame(
        FishSwimmer fish
    )
    {
        if (!fishingMinigameUI)
        {
            Debug.LogError(
                "FishingMinigameUI wurde nicht zugewiesen.",
                this
            );

            return;
        }

        if (!fish)
        {
            Debug.LogError(
                "FishSwimmer wurde dem Minigame nicht übergeben.",
                this
            );

            return;
        }

        /*
         * Zuerst aktivieren.
         * Dadurch läuft OnEnable() der
         * Minigame-Komponenten zuerst durch.
         */
        fishingMinigameUI.SetActive(true);

        /*
         * Jetzt das richtige Fisch-Icon setzen.
         */
        UpdateFishIcon(fish);

        /*
         * Danach erst die individuellen
         * Werte des gefangenen Fisches anwenden.
         */
        if (fishController)
        {
            fishController.ApplyFishSettings(
                fish
            );

            /*
             * Nach dem Anwenden neu starten,
             * damit direkt mit den neuen
             * Einstellungen begonnen wird.
             */
            fishController.ResetFish();
        }
        else
        {
            Debug.LogError(
                "FishingMinigameFishController wurde nicht gefunden.",
                this
            );
        }

        if (progressController)
        {
            progressController.ApplyFishSettings(
                fish
            );

            progressController.ResetProgress();
        }

        if (fishIcon)
        {
            fishIcon.SetAsLastSibling();
        }

        Debug.Log(
            "MINIGAME FISCH: " +
            fish.FishType +
            " | Movement: " +
            fish.MinigameMovementSpeed +
            " | Wait: " +
            fish.MinigameMinimumWaitTime +
            " - " +
            fish.MinigameMaximumWaitTime +
            " | Distance: " +
            fish.MinigameMinimumTargetDistance +
            " | Increase: " +
            fish.MinigameProgressIncreaseSpeed +
            " | Decrease: " +
            fish.MinigameProgressDecreaseSpeed,
            fish.gameObject
        );
    }

    private void UpdateFishIcon(
        FishSwimmer fish
    )
    {
        if (
            !fishIconImage ||
            !fish ||
            !fish.MinigameIcon
        )
        {
            return;
        }

        fishIconImage.sprite =
            fish.MinigameIcon;

        fishIconImage.preserveAspect =
            true;
    }

    public void HideMinigame()
    {
        if (fishingMinigameUI)
        {
            fishingMinigameUI.SetActive(false);
        }
    }
}