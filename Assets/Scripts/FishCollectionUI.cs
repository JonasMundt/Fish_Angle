using TMPro;
using UnityEngine;

public class FishCollectionUI : MonoBehaviour
{
    [Header("Sammlung")]
    [SerializeField]
    private FishCollection fishCollection;

    // =========================================================
    // BARSCH
    // =========================================================

    [Header("Barsch - Eintrag")]
    [SerializeField]
    private GameObject perchEntry;

    [SerializeField]
    private GameObject perchSelectionArrow;

    [Header("Barsch - Details")]
    [SerializeField]
    private GameObject perchDetails;

    [SerializeField]
    private TMP_Text perchCaughtText;

    [SerializeField]
    private TMP_Text perchDiscoveredText;

    [SerializeField]
    private TMP_Text perchValueText;

    [Min(0)]
    [SerializeField]
    private int perchValue = 1;

    // =========================================================
    // KATZENHAI
    // =========================================================

    [Header("Katzenhai - Eintrag")]
    [SerializeField]
    private GameObject catsharkEntry;

    [SerializeField]
    private GameObject catsharkSelectionArrow;

    [Header("Katzenhai - Details")]
    [SerializeField]
    private GameObject catsharkDetails;

    [SerializeField]
    private TMP_Text catsharkCaughtText;

    [SerializeField]
    private TMP_Text catsharkDiscoveredText;

    [SerializeField]
    private TMP_Text catsharkValueText;

    [Min(0)]
    [SerializeField]
    private int catsharkValue = 3;

    // =========================================================
    // KRISTALLAAL
    // =========================================================

    [Header("Kristallaal - Eintrag")]
    [SerializeField]
    private GameObject crystalEelEntry;

    [SerializeField]
    private GameObject crystalEelSelectionArrow;

    [Header("Kristallaal - Details")]
    [SerializeField]
    private GameObject crystalEelDetails;

    [SerializeField]
    private TMP_Text crystalEelCaughtText;

    [SerializeField]
    private TMP_Text crystalEelDiscoveredText;

    [SerializeField]
    private TMP_Text crystalEelValueText;

    [Min(0)]
    [SerializeField]
    private int crystalEelValue = 5;

    // =========================================================
    // PRISMAFORELLE
    // =========================================================

    [Header("Prismaforelle - Eintrag")]
    [SerializeField]
    private GameObject prismTroutEntry;

    [SerializeField]
    private GameObject prismTroutSelectionArrow;

    [Header("Prismaforelle - Details")]
    [SerializeField]
    private GameObject prismTroutDetails;

    [SerializeField]
    private TMP_Text prismTroutCaughtText;

    [SerializeField]
    private TMP_Text prismTroutDiscoveredText;

    [SerializeField]
    private TMP_Text prismTroutValueText;

    [Min(0)]
    [SerializeField]
    private int prismTroutValue = 8;

    // =========================================================
    // UNBEKANNT
    // =========================================================

    [Header("Unbekannter Fisch")]
    [SerializeField]
    private GameObject lockedEntry;

    [SerializeField]
    private GameObject lockedSelectionArrow;

    [SerializeField]
    private GameObject lockedDetails;

    // =========================================================
    // STATUS
    // =========================================================

    /*
     * 0 = Barsch
     * 1 = Katzenhai
     * 2 = Kristallaal
     * 3 = Prismaforelle
     * 4 = ???
     */
    
    private int selectedFishIndex;

    private bool isFocused;

    private void OnEnable()
    {
        if (fishCollection)
        {
            fishCollection.CollectionChanged +=
                UpdateCollection;
        }

        UpdateCollection();
        EnsureValidSelection();
        UpdateSelection();
    }

    private void OnDisable()
    {
        if (fishCollection)
        {
            fishCollection.CollectionChanged -=
                UpdateCollection;
        }
    }

    // =========================================================
    // SAMMLUNG AKTUALISIEREN
    // =========================================================

    public void UpdateCollection()
    {
        if (!fishCollection)
        {
            return;
        }

        UpdateEntryOrder();

        UpdatePerchDetails();
        UpdateCatsharkDetails();
        UpdateCrystalEelDetails();
        UpdatePrismTroutDetails();

        EnsureValidSelection();
        UpdateSelection();
    }

    // =========================================================
    // EINTRAGS-REIHENFOLGE
    // =========================================================

    private void UpdateEntryOrder()
    {
        if (!fishCollection)
        {
            return;
        }

        bool perchDiscovered =
            fishCollection.HasDiscoveredPerch;

        bool catsharkDiscovered =
            fishCollection.HasDiscoveredCatshark;

        bool crystalEelDiscovered =
            fishCollection.HasDiscoveredCrystalEel;

        bool prismTroutDiscovered =
            fishCollection.HasDiscoveredPrismTrout;

        int currentIndex = 0;

        if (perchEntry)
        {
            perchEntry.SetActive(perchDiscovered);

            if (perchDiscovered)
            {
                perchEntry.transform.SetSiblingIndex(
                    currentIndex
                );

                currentIndex++;
            }
        }

        if (catsharkEntry)
        {
            catsharkEntry.SetActive(
                catsharkDiscovered
            );

            if (catsharkDiscovered)
            {
                catsharkEntry.transform.SetSiblingIndex(
                    currentIndex
                );

                currentIndex++;
            }
        }

        if (crystalEelEntry)
        {
            crystalEelEntry.SetActive(
                crystalEelDiscovered
            );

            if (crystalEelDiscovered)
            {
                crystalEelEntry.transform.SetSiblingIndex(
                    currentIndex
                );

                currentIndex++;
            }
        }

        if (prismTroutEntry)
        {
            prismTroutEntry.SetActive(
                prismTroutDiscovered
            );

            if (prismTroutDiscovered)
            {
                prismTroutEntry.transform.SetSiblingIndex(
                    currentIndex
                );

                currentIndex++;
            }
        }

        if (lockedEntry)
        {
            lockedEntry.SetActive(true);

            lockedEntry.transform.SetSiblingIndex(
                currentIndex
            );
        }
    }

    // =========================================================
    // DETAILS
    // =========================================================

    private void UpdatePerchDetails()
    {
        if (!fishCollection)
        {
            return;
        }

        if (perchCaughtText)
        {
            perchCaughtText.text =
                "Gefangen: " +
                fishCollection.PerchCaughtCount +
                "x";
        }

        if (perchDiscoveredText)
        {
            perchDiscoveredText.text =
                "Entdeckt: Ja";
        }

        if (perchValueText)
        {
            perchValueText.text =
                "Wert: " +
                perchValue +
                " Münze";
        }
    }

    private void UpdateCatsharkDetails()
    {
        if (!fishCollection)
        {
            return;
        }

        if (catsharkCaughtText)
        {
            catsharkCaughtText.text =
                "Gefangen: " +
                fishCollection.CatsharkCaughtCount +
                "x";
        }

        if (catsharkDiscoveredText)
        {
            catsharkDiscoveredText.text =
                "Entdeckt: Ja";
        }

        if (catsharkValueText)
        {
            catsharkValueText.text =
                "Wert: " +
                catsharkValue +
                " Münzen";
        }
    }

    private void UpdateCrystalEelDetails()
    {
        if (!fishCollection)
        {
            return;
        }

        if (crystalEelCaughtText)
        {
            crystalEelCaughtText.text =
                "Gefangen: " +
                fishCollection.CrystalEelCaughtCount +
                "x";
        }

        if (crystalEelDiscoveredText)
        {
            crystalEelDiscoveredText.text =
                "Entdeckt: Ja";
        }

        if (crystalEelValueText)
        {
            crystalEelValueText.text =
                "Wert: " +
                crystalEelValue +
                " Münzen";
        }
    }

    private void UpdatePrismTroutDetails()
    {
        if (!fishCollection)
        {
            return;
        }

        if (prismTroutCaughtText)
        {
            prismTroutCaughtText.text =
                "Gefangen: " +
                fishCollection.PrismTroutCaughtCount +
                "x";
        }

        if (prismTroutDiscoveredText)
        {
            prismTroutDiscoveredText.text =
                "Entdeckt: Ja";
        }

        if (prismTroutValueText)
        {
            prismTroutValueText.text =
                "Wert: " +
                prismTroutValue +
                " Münzen";
        }
    }

    // =========================================================
    // FOKUS
    // =========================================================

    public void SetFocused(
        bool focused
    )
    {
        isFocused = focused;

        UpdateSelection();
    }

    // =========================================================
    // AUSWAHL
    // =========================================================

    private void EnsureValidSelection()
    {
        if (!fishCollection)
        {
            selectedFishIndex = 4;
            return;
        }

        if (
            selectedFishIndex == 0 &&
            fishCollection.HasDiscoveredPerch
        )
        {
            return;
        }

        if (
            selectedFishIndex == 1 &&
            fishCollection.HasDiscoveredCatshark
        )
        {
            return;
        }

        if (
            selectedFishIndex == 2 &&
            fishCollection.HasDiscoveredCrystalEel
        )
        {
            return;
        }

        if (
            selectedFishIndex == 3 &&
            fishCollection.HasDiscoveredPrismTrout
        )
        {
            return;
        }

        if (selectedFishIndex == 4)
        {
            return;
        }

        if (fishCollection.HasDiscoveredPerch)
        {
            selectedFishIndex = 0;
        }
        else if (
            fishCollection.HasDiscoveredCatshark
        )
        {
            selectedFishIndex = 1;
        }
        else if (
            fishCollection.HasDiscoveredCrystalEel
        )
        {
            selectedFishIndex = 2;
        }
        else if (
            fishCollection.HasDiscoveredPrismTrout
        )
        {
            selectedFishIndex = 3;
        }
        else
        {
            selectedFishIndex = 4;
        }
    }

    private void UpdateSelection()
    {
        bool perchSelected =
            selectedFishIndex == 0 &&
            fishCollection &&
            fishCollection.HasDiscoveredPerch;

        bool catsharkSelected =
            selectedFishIndex == 1 &&
            fishCollection &&
            fishCollection.HasDiscoveredCatshark;

        bool crystalEelSelected =
            selectedFishIndex == 2 &&
            fishCollection &&
            fishCollection.HasDiscoveredCrystalEel;

        bool prismTroutSelected =
            selectedFishIndex == 3 &&
            fishCollection &&
            fishCollection.HasDiscoveredPrismTrout;

        bool lockedSelected =
            selectedFishIndex == 4;

        if (perchSelectionArrow)
        {
            perchSelectionArrow.SetActive(
                isFocused &&
                perchSelected
            );
        }

        if (catsharkSelectionArrow)
        {
            catsharkSelectionArrow.SetActive(
                isFocused &&
                catsharkSelected
            );
        }

        if (crystalEelSelectionArrow)
        {
            crystalEelSelectionArrow.SetActive(
                isFocused &&
                crystalEelSelected
            );
        }

        if (prismTroutSelectionArrow)
        {
            prismTroutSelectionArrow.SetActive(
                isFocused &&
                prismTroutSelected
            );
        }

        if (lockedSelectionArrow)
        {
            lockedSelectionArrow.SetActive(
                isFocused &&
                lockedSelected
            );
        }

        if (perchDetails)
        {
            perchDetails.SetActive(
                perchSelected
            );
        }

        if (catsharkDetails)
        {
            catsharkDetails.SetActive(
                catsharkSelected
            );
        }

        if (crystalEelDetails)
        {
            crystalEelDetails.SetActive(
                crystalEelSelected
            );
        }

        if (prismTroutDetails)
        {
            prismTroutDetails.SetActive(
                prismTroutSelected
            );
        }

        if (lockedDetails)
        {
            lockedDetails.SetActive(
                lockedSelected
            );
        }
}

    // =========================================================
    // MOUSE
    // =========================================================

    public void HoverPerchEntry()
    {
        if (
            !fishCollection ||
            !fishCollection.HasDiscoveredPerch
        )
        {
            return;
        }

        isFocused = true;
        selectedFishIndex = 0;

        UpdateSelection();
    }

    public void HoverCatsharkEntry()
    {
        if (
            !fishCollection ||
            !fishCollection.HasDiscoveredCatshark
        )
        {
            return;
        }

        isFocused = true;
        selectedFishIndex = 1;

        UpdateSelection();
    }

    public void HoverCrystalEelEntry()
    {
        if (
            !fishCollection ||
            !fishCollection.HasDiscoveredCrystalEel
        )
        {
            return;
        }

        isFocused = true;
        selectedFishIndex = 2;

        UpdateSelection();
    }

    public void HoverPrismTroutEntry()
    {
        if (
            !fishCollection ||
            !fishCollection.HasDiscoveredPrismTrout
        )
        {
            return;
        }

        isFocused = true;
        selectedFishIndex = 3;

        UpdateSelection();
    }

    public void HoverLockedEntry()
    {
        isFocused = true;
        selectedFishIndex = 4;

        UpdateSelection();
    }

    // =========================================================
    // TASTATUR
    // =========================================================

    public void MoveSelection(
        int direction
    )
    {
        int[] availableEntries =
            GetAvailableEntries();

        if (
            availableEntries == null ||
            availableEntries.Length == 0
        )
        {
            return;
        }

        int currentPosition = 0;

        for (
            int i = 0;
            i < availableEntries.Length;
            i++
        )
        {
            if (
                availableEntries[i] ==
                selectedFishIndex
            )
            {
                currentPosition = i;
                break;
            }
        }

        currentPosition += direction;

        if (currentPosition < 0)
        {
            currentPosition =
                availableEntries.Length - 1;
        }
        else if (
            currentPosition >=
            availableEntries.Length
        )
        {
            currentPosition = 0;
        }

        selectedFishIndex =
            availableEntries[
                currentPosition
            ];

        UpdateSelection();
    }

    public void ExitCollectionEntry()
    {
        isFocused = false;

        UpdateSelection();
    }

    private int[] GetAvailableEntries()
    {
        bool hasPerch =
            fishCollection &&
            fishCollection.HasDiscoveredPerch;

        bool hasCatshark =
            fishCollection &&
            fishCollection.HasDiscoveredCatshark;

        bool hasCrystalEel =
            fishCollection &&
            fishCollection.HasDiscoveredCrystalEel;

        bool hasPrismTrout =
            fishCollection &&
            fishCollection.HasDiscoveredPrismTrout;

        int count = 1;

        if (hasPerch)
        {
            count++;
        }

        if (hasCatshark)
        {
            count++;
        }

        if (hasCrystalEel)
        {
            count++;
        }

        if (hasPrismTrout)
        {
            count++;
        }

        int[] availableEntries =
            new int[count];

        int index = 0;

        if (hasPerch)
        {
            availableEntries[index] = 0;
            index++;
        }

        if (hasCatshark)
        {
            availableEntries[index] = 1;
            index++;
        }

        if (hasCrystalEel)
        {
            availableEntries[index] = 2;
            index++;
        }

        if (hasPrismTrout)
        {
            availableEntries[index] = 3;
            index++;
        }

        availableEntries[index] = 4;

        return availableEntries;
    }
}