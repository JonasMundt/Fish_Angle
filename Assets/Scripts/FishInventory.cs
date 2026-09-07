using System;
using UnityEngine;

public class FishInventory : MonoBehaviour
{
    [Header("Inventar")]
    [Min(1)]
    [SerializeField] private int maximumFishCount = 1;

    [Min(0)]
    [SerializeField] private int currentFishCount;

    [Header("Fischarten")]
    [Min(0)]
    [SerializeField] private int perchCount;

    [Min(0)]
    [SerializeField] private int catsharkCount;

    [Min(0)]
    [SerializeField] private int crystalEelCount;

    [Min(0)]
    [SerializeField] private int prismTroutCount;

    public int CurrentFishCount =>
        currentFishCount;

    public int MaximumFishCount =>
        maximumFishCount;

    public int PerchCount =>
        perchCount;

    public int CatsharkCount =>
        catsharkCount;

    public int CrystalEelCount =>
        crystalEelCount;

    public int PrismTroutCount =>
        prismTroutCount;

    public bool HasFish =>
        currentFishCount > 0;

    public bool HasPerch =>
        perchCount > 0;

    public bool HasCatshark =>
        catsharkCount > 0;

    public bool HasCrystalEel =>
        crystalEelCount > 0;

    public bool HasPrismTrout =>
        prismTroutCount > 0;

    public bool IsFull =>
        currentFishCount >= maximumFishCount;

    public event Action InventoryChanged;

    private void Awake()
    {
        ValidateInventory();
    }

    public bool TryAddPerch()
    {
        if (IsFull)
        {
            Debug.Log(
                "Der Barsch konnte nicht aufgenommen werden: Inventar voll.",
                this
            );

            return false;
        }

        perchCount++;
        currentFishCount++;

        InventoryChanged?.Invoke();

        Debug.Log(
            "Barsch aufgenommen. Barsche: " +
            perchCount +
            " | Inventar: " +
            currentFishCount +
            " / " +
            maximumFishCount,
            this
        );

        return true;
    }

    public bool TryAddCatshark()
    {
        if (IsFull)
        {
            Debug.Log(
                "Der Katzenhai konnte nicht aufgenommen werden: Inventar voll.",
                this
            );

            return false;
        }

        catsharkCount++;
        currentFishCount++;

        InventoryChanged?.Invoke();

        Debug.Log(
            "Katzenhai aufgenommen. Katzenhaie: " +
            catsharkCount +
            " | Inventar: " +
            currentFishCount +
            " / " +
            maximumFishCount,
            this
        );

        return true;
    }

    public bool TryAddCrystalEel()
    {
        if (IsFull)
        {
            Debug.Log(
                "Der Kristallaal konnte nicht aufgenommen werden: Inventar voll.",
                this
            );

            return false;
        }

        crystalEelCount++;
        currentFishCount++;

        InventoryChanged?.Invoke();

        Debug.Log(
            "Kristallaal aufgenommen. Kristallaale: " +
            crystalEelCount +
            " | Inventar: " +
            currentFishCount +
            " / " +
            maximumFishCount,
            this
        );

        return true;
    }

    public bool TryAddPrismTrout()
    {
        if (IsFull)
        {
            Debug.Log(
                "Die Prismaforelle konnte nicht aufgenommen werden: Inventar voll.",
                this
            );

            return false;
        }

        prismTroutCount++;
        currentFishCount++;

        InventoryChanged?.Invoke();

        Debug.Log(
            "Prismaforelle aufgenommen. Prismaforellen: " +
            prismTroutCount +
            " | Inventar: " +
            currentFishCount +
            " / " +
            maximumFishCount,
            this
        );

        return true;
    }

    public bool TryRemovePerch()
    {
        if (perchCount <= 0)
        {
            return false;
        }

        perchCount--;
        currentFishCount--;

        InventoryChanged?.Invoke();

        return true;
    }

    public bool TryRemoveCatshark()
    {
        if (catsharkCount <= 0)
        {
            return false;
        }

        catsharkCount--;
        currentFishCount--;

        InventoryChanged?.Invoke();

        return true;
    }

    public bool TryRemoveCrystalEel()
    {
        if (crystalEelCount <= 0)
        {
            return false;
        }

        crystalEelCount--;
        currentFishCount--;

        InventoryChanged?.Invoke();

        return true;
    }

    public bool TryRemovePrismTrout()
    {
        if (prismTroutCount <= 0)
        {
            return false;
        }

        prismTroutCount--;
        currentFishCount--;

        InventoryChanged?.Invoke();

        return true;
    }

    public void SetMaximumFishCount(
        int newMaximumFishCount
    )
    {
        maximumFishCount =
            Mathf.Max(
                1,
                newMaximumFishCount
            );

        ValidateInventory();

        InventoryChanged?.Invoke();
    }

    public void AddInventorySlots(
        int additionalSlots
    )
    {
        if (additionalSlots <= 0)
        {
            return;
        }

        SetMaximumFishCount(
            maximumFishCount +
            additionalSlots
        );
    }


    public void SetInventoryState(
        int newMaximumFishCount,
        int newPerchCount,
        int newCatsharkCount,
        int newCrystalEelCount,
        int newPrismTroutCount
    )
    {
        maximumFishCount =
            Mathf.Max(
                1,
                newMaximumFishCount
            );

        perchCount =
            Mathf.Max(
                0,
                newPerchCount
            );

        catsharkCount =
            Mathf.Max(
                0,
                newCatsharkCount
            );

        crystalEelCount =
            Mathf.Max(
                0,
                newCrystalEelCount
            );

        prismTroutCount =
            Mathf.Max(
                0,
                newPrismTroutCount
            );

        ValidateInventory();

        InventoryChanged?.Invoke();
    }
    private void ValidateInventory()
    {
        maximumFishCount =
            Mathf.Max(
                1,
                maximumFishCount
            );

        perchCount =
            Mathf.Max(
                0,
                perchCount
            );

        catsharkCount =
            Mathf.Max(
                0,
                catsharkCount
            );

        crystalEelCount =
            Mathf.Max(
                0,
                crystalEelCount
            );

        prismTroutCount =
            Mathf.Max(
                0,
                prismTroutCount
            );

        /*
         * Die Gesamtzahl ergibt sich aus allen
         * tatsächlich vorhandenen Fischarten.
         */
        currentFishCount =
            perchCount +
            catsharkCount +
            crystalEelCount +
            prismTroutCount;

        /*
         * Sicherheitsprüfung für den Inspector.
         * Im normalen Spiel sollte dieser Fall
         * nicht auftreten.
         */
        if (
            currentFishCount >
            maximumFishCount
        )
        {
            int overflow =
                currentFishCount -
                maximumFishCount;

            int prismTroutsToRemove =
                Mathf.Min(
                    prismTroutCount,
                    overflow
                );

            prismTroutCount -=
                prismTroutsToRemove;

            overflow -=
                prismTroutsToRemove;

            int crystalEelsToRemove =
                Mathf.Min(
                    crystalEelCount,
                    overflow
                );

            crystalEelCount -=
                crystalEelsToRemove;

            overflow -=
                crystalEelsToRemove;

            int catsharksToRemove =
                Mathf.Min(
                    catsharkCount,
                    overflow
                );

            catsharkCount -=
                catsharksToRemove;

            overflow -=
                catsharksToRemove;

            if (overflow > 0)
            {
                perchCount =
                    Mathf.Max(
                        0,
                        perchCount -
                        overflow
                    );
            }

            currentFishCount =
                perchCount +
                catsharkCount +
                crystalEelCount +
                prismTroutCount;
        }
    }

    private void OnValidate()
    {
        ValidateInventory();
    }
}