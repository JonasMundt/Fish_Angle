using UnityEngine;

public class FishingSpot : MonoBehaviour
{
    [SerializeField] private GameObject interactionPrompt;

    private bool playerIsInside;
    private PlayerFishing playerFishing;

    private void Start()
    {
        UpdateInteractionPrompt();
    }

    private void Update()
    {
        if (!playerIsInside || playerFishing == null)
        {
            return;
        }

        UpdateInteractionPrompt();

        if (
            !playerFishing.IsFishing &&
            Input.GetKeyDown(KeyCode.E)
        )
        {
            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(false);
            }

            playerFishing.StartFishing();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerFishing fishing =
            other.GetComponent<PlayerFishing>();

        if (fishing == null)
        {
            return;
        }

        playerIsInside = true;
        playerFishing = fishing;

        UpdateInteractionPrompt();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        PlayerFishing fishing =
            other.GetComponent<PlayerFishing>();

        if (
            fishing == null ||
            fishing != playerFishing
        )
        {
            return;
        }

        playerIsInside = false;
        playerFishing = null;

        UpdateInteractionPrompt();
    }

    private void UpdateInteractionPrompt()
    {
        if (interactionPrompt == null)
        {
            return;
        }

        bool shouldShow =
            playerIsInside &&
            playerFishing != null &&
            !playerFishing.IsFishing;

        interactionPrompt.SetActive(shouldShow);
    }
}