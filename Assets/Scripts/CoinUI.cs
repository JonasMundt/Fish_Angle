using TMPro;
using UnityEngine;

public class CoinUI : MonoBehaviour
{
    [Header("Münzsystem")]
    [SerializeField] private CoinWallet coinWallet;

    [Header("UI")]
    [SerializeField] private TMP_Text coinText;

    private void Update()
    {
        if (!coinWallet || !coinText)
        {
            return;
        }

        coinText.text = coinWallet.Coins.ToString();
    }
}