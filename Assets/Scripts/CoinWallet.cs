using UnityEngine;

public class CoinWallet : MonoBehaviour
{
    [Header("Münzen")]
    [Min(0)]
    [SerializeField] private int coins = 0;

    [Header("Münzsack")]
    [Min(1)]
    [SerializeField] private int maximumCoins = 10;

    public int Coins =>
        coins;

    public int MaximumCoins =>
        maximumCoins;

    public bool IsFull =>
        coins >= maximumCoins;

    public void AddCoins(
        int amount
    )
    {
        if (amount <= 0)
        {
            return;
        }

        coins =
            Mathf.Clamp(
                coins + amount,
                0,
                maximumCoins
            );
    }

    public bool CanAddCoins(
        int amount
    )
    {
        if (amount <= 0)
        {
            return false;
        }

        return
            coins + amount <=
            maximumCoins;
    }

    public bool SpendCoins(
        int amount
    )
    {
        if (amount <= 0)
        {
            return false;
        }

        if (coins < amount)
        {
            return false;
        }

        coins -= amount;

        return true;
    }

    public void SetCoins(
        int amount
    )
    {
        coins =
            Mathf.Clamp(
                amount,
                0,
                maximumCoins
            );
    }

    public void SetMaximumCoins(
        int newMaximum
    )
    {
        maximumCoins =
            Mathf.Max(
                1,
                newMaximum
            );

        coins =
            Mathf.Clamp(
                coins,
                0,
                maximumCoins
            );
    }

    private void OnValidate()
    {
        maximumCoins =
            Mathf.Max(
                1,
                maximumCoins
            );

        coins =
            Mathf.Clamp(
                coins,
                0,
                maximumCoins
            );
    }
}