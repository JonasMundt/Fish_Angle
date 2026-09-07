using System.Collections;
using TMPro;
using UnityEngine;

public class InventoryMessageUI : MonoBehaviour
{
    [Header("Anzeige")]
    [SerializeField] private TMP_Text messageText;

    [Min(0.1f)]
    [SerializeField] private float displayDuration = 2.5f;

    private Coroutine hideCoroutine;

    private void Awake()
    {
        if (messageText == null)
        {
            messageText = GetComponent<TMP_Text>();
        }

        HideMessage();
    }

    public void ShowInventoryFullMessage()
    {
        ShowMessage(
            "Dein Fischinventar ist voll!"
        );
    }

    public void ShowMessage(string message)
    {
        if (messageText == null)
        {
            return;
        }

        messageText.text = message;
        messageText.gameObject.SetActive(true);

        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
        }

        hideCoroutine = StartCoroutine(
            HideMessageAfterDelay()
        );
    }

    public void HideMessage()
    {
        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
            hideCoroutine = null;
        }

        if (messageText != null)
        {
            messageText.gameObject.SetActive(false);
        }
    }

    private IEnumerator HideMessageAfterDelay()
    {
        yield return new WaitForSeconds(
            displayDuration
        );

        hideCoroutine = null;

        if (messageText != null)
        {
            messageText.gameObject.SetActive(false);
        }
    }
}