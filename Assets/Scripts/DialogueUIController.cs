using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUIController : MonoBehaviour
{
    [Header("Dialog-UI")]
    [SerializeField] private GameObject dialogueUI;
    [SerializeField] private Image speakerPortrait;
    [SerializeField] private TMP_Text speakerNameText;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private GameObject continueIndicator;

    [Header("Spieler")]
    [SerializeField] private PlayerMovement playerMovement;

    [Header("Schreibgeschwindigkeit")]
    [Min(0.001f)]
    [SerializeField] private float secondsPerCharacter = 0.035f;

    [Header("Steuerung")]
    [SerializeField] private KeyCode continueKey = KeyCode.E;

    [Header("Vorläufiger Test")]
    [SerializeField] private bool enableTestDialogue = true;
    [SerializeField] private KeyCode testDialogueKey = KeyCode.T;

    [TextArea(2, 5)]
    [SerializeField]
    private string[] testDialogueLines =
    {
        "Ein Barsch?",
        "Nicht schlecht für deinen ersten Fang.",
        "Ich könnte ihn dir abnehmen. Natürlich nicht umsonst."
    };

    [SerializeField] private string testSpeakerName = "Verkäufer";
    [SerializeField] private Sprite testSpeakerPortrait;

    private string[] currentDialogueLines;

    private int currentLineIndex;
    private Coroutine typingCoroutine;

    private bool isDialogueActive;
    private bool isTyping;
    private bool canReadContinueInput;

    public bool IsDialogueActive =>
        isDialogueActive;

    public event Action DialogueClosed;

    private void Awake()
    {
        if (dialogueUI)
        {
            dialogueUI.SetActive(false);
        }

        if (continueIndicator)
        {
            continueIndicator.SetActive(false);
        }

        if (dialogueText)
        {
            dialogueText.text = string.Empty;
        }
    }

    private void Update()
    {
        if (
            enableTestDialogue &&
            !isDialogueActive &&
            Input.GetKeyDown(testDialogueKey)
        )
        {
            StartDialogue(
                testSpeakerName,
                testSpeakerPortrait,
                testDialogueLines
            );

            return;
        }

        if (!isDialogueActive)
        {
            return;
        }

        if (!canReadContinueInput)
        {
            if (!Input.GetKey(continueKey))
            {
                canReadContinueInput = true;
            }

            return;
        }

        if (Input.GetKeyDown(continueKey))
        {
            HandleContinueInput();
        }
    }

    public void StartDialogue(
        string speakerName,
        Sprite portrait,
        string[] dialogueLines
    )
    {
        if (
            dialogueLines == null ||
            dialogueLines.Length == 0
        )
        {
            Debug.LogWarning(
                "Der Dialog enthält keine Textabschnitte.",
                this
            );

            return;
        }

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        currentDialogueLines = dialogueLines;
        currentLineIndex = 0;

        isDialogueActive = true;
        isTyping = false;
        canReadContinueInput = false;

        if (speakerNameText)
        {
            speakerNameText.text =
                speakerName;
        }

        if (speakerPortrait)
        {
            speakerPortrait.sprite =
                portrait;

            speakerPortrait.enabled =
                portrait;
        }

        if (dialogueUI)
        {
            dialogueUI.SetActive(true);
        }

        if (continueIndicator)
        {
            continueIndicator.SetActive(false);
        }

        if (playerMovement)
        {
            playerMovement.SetMovementEnabled(false);
        }

        ShowCurrentLine();
    }

    private void HandleContinueInput()
    {
        if (isTyping)
        {
            CompleteCurrentLineImmediately();
            return;
        }

        currentLineIndex++;

        if (
            currentDialogueLines == null ||
            currentLineIndex >=
            currentDialogueLines.Length
        )
        {
            CloseDialogue();
            return;
        }

        ShowCurrentLine();
    }

    private void ShowCurrentLine()
    {
        if (
            currentDialogueLines == null ||
            currentLineIndex < 0 ||
            currentLineIndex >=
            currentDialogueLines.Length
        )
        {
            CloseDialogue();
            return;
        }

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine =
            StartCoroutine(
                TypeCurrentLine()
            );
    }

    private IEnumerator TypeCurrentLine()
    {
        isTyping = true;

        if (continueIndicator)
        {
            continueIndicator.SetActive(false);
        }

        string line =
            currentDialogueLines[
                currentLineIndex
            ];

        if (dialogueText)
        {
            dialogueText.text =
                string.Empty;
        }

        for (
            int i = 0;
            i < line.Length;
            i++
        )
        {
            if (dialogueText)
            {
                dialogueText.text +=
                    line[i];
            }

            yield return
                new WaitForSecondsRealtime(
                    secondsPerCharacter
                );
        }

        typingCoroutine = null;
        isTyping = false;

        if (continueIndicator)
        {
            continueIndicator.SetActive(true);
        }
    }

    private void CompleteCurrentLineImmediately()
    {
        if (!isTyping)
        {
            return;
        }

        if (typingCoroutine != null)
        {
            StopCoroutine(
                typingCoroutine
            );

            typingCoroutine = null;
        }

        if (
            dialogueText &&
            currentDialogueLines != null &&
            currentLineIndex >= 0 &&
            currentLineIndex <
            currentDialogueLines.Length
        )
        {
            dialogueText.text =
                currentDialogueLines[
                    currentLineIndex
                ];
        }

        isTyping = false;

        if (continueIndicator)
        {
            continueIndicator.SetActive(true);
        }
    }

    public void CloseDialogue()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(
                typingCoroutine
            );

            typingCoroutine = null;
        }

        isDialogueActive = false;
        isTyping = false;
        canReadContinueInput = false;

        currentDialogueLines = null;
        currentLineIndex = 0;

        if (dialogueText)
        {
            dialogueText.text =
                string.Empty;
        }

        if (continueIndicator)
        {
            continueIndicator.SetActive(false);
        }

        if (dialogueUI)
        {
            dialogueUI.SetActive(false);
        }

        if (playerMovement)
        {
            playerMovement.SetMovementEnabled(true);
        }

        DialogueClosed?.Invoke();
    }

    private void OnValidate()
    {
        secondsPerCharacter =
            Mathf.Max(
                0.001f,
                secondsPerCharacter
            );
    }
}