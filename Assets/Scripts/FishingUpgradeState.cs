using System;
using UnityEngine;

public class FishingUpgradeState : MonoBehaviour
{
    [Header("Haken")]
    [Range(1, 3)]
    [SerializeField] private int hookLevel = 1;

    [Header("Schnurlänge")]
    [Range(1, 2)]
    [SerializeField] private int lineLevel = 1;

    [Header("Haken-Geschwindigkeit")]
    [Range(1, 3)]
    [SerializeField] private int hookSpeedLevel = 1;

    [Header("Münzsack")]
    [Range(1, 3)]
    [SerializeField] private int coinBagLevel = 1;

    [Header("Skins")]
    [SerializeField] private bool purpleSkinPurchased = false;

    public int HookLevel => hookLevel;
    public int LineLevel => lineLevel;
    public int HookSpeedLevel => hookSpeedLevel;
    public int CoinBagLevel => coinBagLevel;

    public bool PurpleSkinPurchased =>
        purpleSkinPurchased;

    public event Action UpgradesChanged;

    public void SetHookLevel(int newLevel)
    {
        hookLevel =
            Mathf.Clamp(newLevel, 1, 3);

        UpgradesChanged?.Invoke();
    }

    public bool HasHookLevel(int requiredLevel)
    {
        return hookLevel >= requiredLevel;
    }

    public void SetLineLevel(int newLevel)
    {
        lineLevel =
            Mathf.Clamp(newLevel, 1, 2);

        UpgradesChanged?.Invoke();
    }

    public void SetHookSpeedLevel(int newLevel)
    {
        hookSpeedLevel =
            Mathf.Clamp(newLevel, 1, 3);

        UpgradesChanged?.Invoke();
    }

    public void SetCoinBagLevel(int newLevel)
    {
        coinBagLevel =
            Mathf.Clamp(newLevel, 1, 3);

        UpgradesChanged?.Invoke();
    }

    public void PurchasePurpleSkin()
    {
        if (purpleSkinPurchased)
        {
            return;
        }

        purpleSkinPurchased = true;

        UpgradesChanged?.Invoke();
    }

    public void SetUpgradeState(
    int newHookLevel,
    int newLineLevel,
    int newHookSpeedLevel,
    int newCoinBagLevel,
    bool newPurpleSkinPurchased
    )
    {
        hookLevel =
            Mathf.Clamp(
                newHookLevel,
                1,
                3
            );

        lineLevel =
            Mathf.Clamp(
                newLineLevel,
                1,
                2
            );

        hookSpeedLevel =
            Mathf.Clamp(
                newHookSpeedLevel,
                1,
                3
            );

        coinBagLevel =
            Mathf.Clamp(
                newCoinBagLevel,
                1,
                3
            );

        purpleSkinPurchased =
            newPurpleSkinPurchased;

        UpgradesChanged?.Invoke();
    }

    private void OnValidate()
    {
        hookLevel =
            Mathf.Clamp(hookLevel, 1, 3);

        lineLevel =
            Mathf.Clamp(lineLevel, 1, 2);

        hookSpeedLevel =
            Mathf.Clamp(hookSpeedLevel, 1, 3);

        coinBagLevel =
            Mathf.Clamp(coinBagLevel, 1, 3);
    }
}