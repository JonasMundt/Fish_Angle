using TMPro;
using UnityEngine;

public class FishInventoryUI : MonoBehaviour
{
    [Header("Inventar")]
    [SerializeField] private FishInventory fishInventory;

    [Header("Anzeige")]
    [SerializeField] private TMP_Text fishCountText;

    private void OnEnable()
    {
        if (fishInventory != null)
        {
            fishInventory.InventoryChanged +=
                UpdateDisplay;
        }

        UpdateDisplay();
    }

    private void OnDisable()
    {
        if (fishInventory != null)
        {
            fishInventory.InventoryChanged -=
                UpdateDisplay;
        }
    }

    private void UpdateDisplay()
    {
        if (
            fishInventory == null ||
            fishCountText == null
        )
        {
            return;
        }

        fishCountText.text =
            fishInventory.CurrentFishCount +
            " / " +
            fishInventory.MaximumFishCount;
    }
}