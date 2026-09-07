using System;
using UnityEngine;

public class FishCollection : MonoBehaviour
{
    [Header("Barsch")]
    [Min(0)]
    [SerializeField] private int perchCaughtCount;

    [Header("Katzenhai")]
    [Min(0)]
    [SerializeField] private int catsharkCaughtCount;

    [Header("Kristallaal")]
    [Min(0)]
    [SerializeField] private int crystalEelCaughtCount;

    [Header("Prismaforelle")]
    [Min(0)]
    [SerializeField] private int prismTroutCaughtCount;

    public int PerchCaughtCount => perchCaughtCount;

    public int CatsharkCaughtCount => catsharkCaughtCount;

    public int CrystalEelCaughtCount => crystalEelCaughtCount;

    public int PrismTroutCaughtCount => prismTroutCaughtCount;

    public bool HasDiscoveredPerch => perchCaughtCount > 0;

    public bool HasDiscoveredCatshark => catsharkCaughtCount > 0;

    public bool HasDiscoveredCrystalEel => crystalEelCaughtCount > 0;

    public bool HasDiscoveredPrismTrout => prismTroutCaughtCount > 0;

    public event Action CollectionChanged;

    public void RegisterPerchCatch()
    {
        perchCaughtCount++;

        CollectionChanged?.Invoke();
    }

    public void RegisterCatsharkCatch()
    {
        catsharkCaughtCount++;

        CollectionChanged?.Invoke();
    }

    public void RegisterCrystalEelCatch()
    {
        crystalEelCaughtCount++;

        CollectionChanged?.Invoke();
    }

    public void RegisterPrismTroutCatch()
    {
        prismTroutCaughtCount++;

        CollectionChanged?.Invoke();
    }

    private void OnValidate()
    {
        perchCaughtCount =
            Mathf.Max(
                0,
                perchCaughtCount
            );

        catsharkCaughtCount =
            Mathf.Max(
                0,
                catsharkCaughtCount
            );

        crystalEelCaughtCount =
            Mathf.Max(
                0,
                crystalEelCaughtCount
            );

        prismTroutCaughtCount =
            Mathf.Max(
                0,
                prismTroutCaughtCount
            );
    }

        public void SetCollectionState(
        int newPerchCaughtCount,
        int newCatsharkCaughtCount,
        int newCrystalEelCaughtCount,
        int newPrismTroutCaughtCount
    )
    {
        perchCaughtCount =
            Mathf.Max(
                0,
                newPerchCaughtCount
            );

        catsharkCaughtCount =
            Mathf.Max(
                0,
                newCatsharkCaughtCount
            );

        crystalEelCaughtCount =
            Mathf.Max(
                0,
                newCrystalEelCaughtCount
            );

        prismTroutCaughtCount =
            Mathf.Max(
                0,
                newPrismTroutCaughtCount
            );

        CollectionChanged?.Invoke();
    }
}