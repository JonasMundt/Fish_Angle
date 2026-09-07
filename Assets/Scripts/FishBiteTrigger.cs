using UnityEngine;

public class FishBiteTrigger : MonoBehaviour
{
    private FishSwimmer fish;

    private void Awake()
    {
        fish =
            GetComponentInParent<FishSwimmer>();
    }

    private void OnTriggerEnter2D(
        Collider2D other
    )
    {
        if (
            !fish ||
            fish.IsHooked
        )
        {
            return;
        }

        FishingHookController hook =
            other.GetComponent<FishingHookController>();

        if (!hook)
        {
            hook =
                other.GetComponentInParent<
                    FishingHookController
                >();
        }

        if (!hook)
        {
            return;
        }

        hook.TryHookFish(fish);
    }
}