using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SellerInteraction : MonoBehaviour
{
    [Header("Dialogsystem")]
    [SerializeField]
    private DialogueUIController dialogueUIController;

    [Header("Seller-Menü")]
    [SerializeField]
    private SellerMenuController sellerMenuController;

    [Header("Interaktionshinweis")]
    [SerializeField]
    private GameObject interactionPrompt;

    [Header("Verkäufer")]
    [SerializeField] private string sellerName = "Verkäufer";

    [SerializeField] private Sprite sellerPortrait;

    public bool IntroductionCompleted =>
    introductionCompleted;

    public bool CatsharkDialogueCompleted =>
        catsharkDialogueCompleted;

    public bool CrystalEelDialogueCompleted =>
        crystalEelDialogueCompleted;

    public bool PrismTroutDialogueCompleted =>
        prismTroutDialogueCompleted;

    public bool HookUpgradeDialogueCompleted =>
        hookUpgradeDialogueCompleted;

    public bool LineUpgradeDialogueCompleted =>
        lineUpgradeDialogueCompleted;

    public bool PurpleSkinDialogueCompleted =>
        purpleSkinDialogueCompleted;

    // =========================================================
    // INTRO
    // =========================================================

    [Header("Erster Dialog")]
    [TextArea(2, 5)]
    [SerializeField]
    private string[] introductionDialogueLines =
    {
        "Ich hab dich angeln hören. Hast du was gefangen?",
        "Einen Barsch? Nicht schlecht.",
        "Ich kann selbst nicht angeln, aber Equipment hab ich genug.",
        "Keine Ahnung, wie ich überhaupt auf diesem Planeten gelandet bin.",
        "Aber Leuten beim Angeln zuzusehen, ist überraschend spannend.",
        "Mein Planet heißt Astrel. Ich war eigentlich nur auf Rundreise.",
        "Irgendwann hab ich einen Sternstein aus Astrel in diesen See geworfen.",
        "Kann gut sein, dass sich tief unten seitdem irgendwas verändert hat.",
        "Wie die Magie von Astrel diesen Planeten beeinflusst? Keine Ahnung.",
        "Moment mal... du hast nur Platz für einen Fisch?",
        "Und dein Haken sieht auch nicht gerade vertrauenerweckend aus.",
        "Bist du neu hier?",
        "...",
        "Nicht besonders gesprächig, was?",
        "Egal. Ich kann dir helfen.",
        "Schau dich einfach mal um."
    };

    // =========================================================
    // KATZENHAI
    // =========================================================

    [Header("Katzenhai-Dialog")]
    [TextArea(2, 5)]
    [SerializeField]
    private string[] catsharkDialogueLines =
    {
        "Oh, du hast einen Katzenhai gefangen? Der ist wertvoller als ein Barsch.",
        "Ich habe über Katzenhaie in Büchern gelesen. Sogar auf meinem Planeten.",
        "Ich frage mich immer noch, wie diese Bücher es eigentlich nach Astrel geschafft haben.",
        "Egal. Katzenhaie gehören zur Ordnung der Grundhaie, was ziemlich interessant ist.",
        "Vor allem, weil Katzenhaie eigentlich gar nicht so gefährlich sind wie Weiße Haie zum Beispiel.",
        "Ich kann dir dafür also ein paar Münzen mehr anbieten als für einen Barsch.",
        "...",
        "Vor allem, weil der lustiger aussieht als der Barsch."
    };

    // =========================================================
    // KRISTALLAAL
    // =========================================================

    [Header("Kristallaal-Dialog")]
    [TextArea(2, 5)]
    [SerializeField]
    private string[] crystalEelDialogueLines =
    {
        "Was ist das denn für ein Exemplar?",
        "Es sieht aus wie ein Aal... aber warum wachsen überall Kristalle aus seinem Körper?",
        "...",
        "Das könnte tatsächlich eine Folge des Sternsteins sein, den ich in den See geworfen habe.",
        "Anscheinend ist deine Schnur inzwischen lang genug, dass du ziemlich tief hinunterkommst.",
        "Tief genug, um die Auswirkungen der Magie aus Astrel mit eigenen Augen zu sehen.",
        "Armer Aal... aber irgendwie ist dadurch auch ein völlig neues Wesen entstanden.",
        "Und da wir es entdeckt haben, dürfen wir ihm wohl auch einen Namen geben.",
        "Ich würde sagen...",
        "Kristaal!",
        "...",
        "Nicht begeistert? Ich finde den gar nicht schlecht.",
        "Mann, bist du langweilig...",
        "Na gut. Dann nennen wir ihn eben Kristallaal.",
        "Zufrieden?",
        "Für so ein besonderes Wesen bekommst du natürlich auch mehr Münzen.",
        "Und halt die Augen offen. Wer weiß, was die Magie dort unten noch verändert hat."
    };

    // =========================================================
    // PRISMAFORELLE
    // =========================================================

    [Header("Prismaforellen-Dialog")]
    [TextArea(2, 5)]
    [SerializeField]
    private string[] prismTroutDialogueLines =
    {
        "Moment mal... was hast du denn da gefangen?",
        "Das ist doch eine Forelle... oder?",
        "Diese Farben... und die Kristalle...",
        "So etwas habe ich noch nie gesehen.",
        "Die Magie dort unten verändert die Fische stärker, als ich dachte.",
        "Das hier ist keine gewöhnliche Forelle mehr.",
        "Eigentlich ist es inzwischen etwas völlig Neues.",
        "Ein Wesen, das ohne den Sternstein vermutlich nie existiert hätte.",
        "Hm...",
        "Prismaforelle.",
        "Ja. Das passt.",
        "Wenn du noch mehr davon findest, bring sie ruhig zu mir.",
        "Für so ein seltenes Exemplar zahle ich dir natürlich auch entsprechend."
    };

    // =========================================================
    // HAKEN
    // =========================================================

    [Header("Haken-Upgrade-Dialog")]
    [TextArea(2, 5)]
    [SerializeField]
    private string[] hookUpgradeDialogueLines =
    {
        "Du fragst dich bestimmt, warum du überhaupt einen besseren Haken brauchst, oder?",
        "Naja, seltenere Fische brauchen bessere Haken.",
        "Der Barsch ist ... naja.",
        "Langweilig.",
        "Der Stein, den ich in den See geworfen habe, sollte neue Fische erschaffen haben. Vielleicht sogar noch andere Dinge.",
        "Mit dem neuen Haken solltest du jetzt in der Lage sein, andere Fische zu fangen.",
        "Falls nicht, solltest du meinen noch besseren Haken ausprobieren. Der kostet allerdings etwas.",
        "Also fang ruhig noch mehr Fische. Dann kann ich mit meiner Forschung weitermachen und du bekommst deine Münzen."
    };

    // =========================================================
    // SCHNURLÄNGE
    // =========================================================

    [Header("Schnurlängen-Upgrade-Dialog")]
    [TextArea(2, 5)]
    [SerializeField]
    private string[] lineUpgradeDialogueLines =
    {
        "Eine sehr schlaue Verbesserung, die Schnurlänge.",
        "Damit kannst du noch tiefer in diesen See hinab.",
        "Und dort unten wirst du mit ziemlich hoher Wahrscheinlichkeit auf interessantere Wesen stoßen.",
        "Für neue Fische und andere Wesen bekommst du natürlich auch mehr Münzen.",
        "Ich bin wirklich gespannt, wie die Magie aus Astrel diesen Planeten noch beeinflussen wird.",
        "Eure Unterwasserwelt ist jedenfalls ziemlich besonders."
    };

    // =========================================================
    // PURPLE SKIN
    // =========================================================

    [Header("Purple-Skin-Dialog")]
    [TextArea(2, 5)]
    [SerializeField]
    private string[] purpleSkinDialogueLines =
    {
        "Na? Wie gefällt dir dein neuer Look? Den habe ich selbst angefertigt.",
        "Du siehst damit auch deutlich besser aus als vorher.",
        "Mein Stil ist schließlich der beste. Und jetzt siehst du sogar ein bisschen mehr aus wie ich.",
        "Vielleicht bekomme ich später noch ein paar andere Klamotten hin.",
        "Aber erstmal kannst du mit deinem neuen Outfit deinen besseren Stil zeigen."
    };

    // =========================================================
    // VERBINDUNG ZWISCHEN DIALOGEN
    // =========================================================

    [Header("Mehrere Spezialdialoge")]
    [SerializeField]
    private string additionalDialogueLine =
        "Und noch etwas ...";

    // =========================================================
    // STEUERUNG
    // =========================================================

    [Header("Steuerung")]
    [SerializeField]
    private KeyCode interactionKey =
        KeyCode.E;

    // =========================================================
    // STATUS
    // =========================================================

    [Header("Status")]
    [SerializeField]
    private bool introductionCompleted;

    [SerializeField]
    private bool catsharkDialogueCompleted;

    [SerializeField]
    private bool crystalEelDialogueCompleted;

    [SerializeField]
    private bool prismTroutDialogueCompleted;

    [SerializeField]
    private bool hookUpgradeDialogueCompleted;

    [SerializeField]
    private bool lineUpgradeDialogueCompleted;

    [SerializeField]
    private bool purpleSkinDialogueCompleted;

    // =========================================================
    // PENDING
    // =========================================================

    private bool catsharkDialoguePending;
    private bool crystalEelDialoguePending;
    private bool prismTroutDialoguePending;
    private bool hookUpgradeDialoguePending;
    private bool lineUpgradeDialoguePending;
    private bool purpleSkinDialoguePending;

    // =========================================================
    // INTERAKTION
    // =========================================================

    private PlayerMovement playerInRange;

    private bool playerIsInRange;
    private bool interactionKeyWasReleased;

    // =========================================================
    // DIALOGSTATUS
    // =========================================================

    private bool introductionDialogueIsRunning;
    private bool specialDialogueSequenceRunning;

    private bool openMenuAfterDialogueSequence;

    private int dialoguesPlayedInCurrentSequence;

    // =========================================================
    // UNITY
    // =========================================================

    private void Awake()
    {
        HideInteractionPrompt();
    }

    private void OnEnable()
    {
        if (dialogueUIController)
        {
            dialogueUIController.DialogueClosed +=
                HandleDialogueClosed;
        }

        playerInRange = null;
        playerIsInRange = false;

        interactionKeyWasReleased = false;

        introductionDialogueIsRunning = false;
        specialDialogueSequenceRunning = false;

        openMenuAfterDialogueSequence = false;

        dialoguesPlayedInCurrentSequence = 0;

        HideInteractionPrompt();
    }

    private void OnDisable()
    {
        if (dialogueUIController)
        {
            dialogueUIController.DialogueClosed -=
                HandleDialogueClosed;
        }

        HideInteractionPrompt();

        playerInRange = null;
        playerIsInRange = false;

        interactionKeyWasReleased = false;

        introductionDialogueIsRunning = false;
        specialDialogueSequenceRunning = false;

        openMenuAfterDialogueSequence = false;

        dialoguesPlayedInCurrentSequence = 0;
    }

    private void Update()
    {
        if (!playerIsInRange || !playerInRange)
        {
            return;
        }

        bool dialogueIsActive =
            dialogueUIController &&
            dialogueUIController.IsDialogueActive;

        bool sellerMenuIsOpen =
            sellerMenuController &&
            sellerMenuController.IsOpen;

        if (
            dialogueIsActive ||
            sellerMenuIsOpen
        )
        {
            HideInteractionPrompt();
            return;
        }

        if (!interactionKeyWasReleased)
        {
            if (!Input.GetKey(interactionKey))
            {
                interactionKeyWasReleased = true;
                ShowInteractionPrompt();
            }

            return;
        }

        ShowInteractionPrompt();

        if (Input.GetKeyDown(interactionKey))
        {
            HandleSellerInteraction();
        }
    }

    // =========================================================
    // NORMALE SELLER-INTERAKTION
    // =========================================================

    private void HandleSellerInteraction()
    {
        HideInteractionPrompt();

        interactionKeyWasReleased = false;

        if (!introductionCompleted)
        {
            StartIntroductionDialogue();
            return;
        }

        if (HasPendingSpecialDialogue())
        {
            StartSpecialDialogueSequence(
                true
            );

            return;
        }

        OpenSellerMenu();
    }

    // =========================================================
    // INTRO
    // =========================================================

    private void StartIntroductionDialogue()
    {
        if (!dialogueUIController)
        {
            Debug.LogError(
                "DialogueUIController wurde am Seller nicht zugewiesen.",
                this
            );

            return;
        }

        introductionDialogueIsRunning = true;

        dialogueUIController.StartDialogue(
            sellerName,
            sellerPortrait,
            introductionDialogueLines
        );
    }

    // =========================================================
    // MENÜ WURDE GESCHLOSSEN
    // =========================================================

    public void HandleSellerMenuClosed()
    {
        if (!HasPendingSpecialDialogue())
        {
            interactionKeyWasReleased = false;
            return;
        }

        StartSpecialDialogueSequence(
            false
        );
    }

    // =========================================================
    // SPEZIALDIALOG-SEQUENZ
    // =========================================================

    private void StartSpecialDialogueSequence(
        bool shouldOpenMenuAfterwards
    )
    {
        if (
            !dialogueUIController ||
            specialDialogueSequenceRunning
        )
        {
            return;
        }

        specialDialogueSequenceRunning = true;

        openMenuAfterDialogueSequence =
            shouldOpenMenuAfterwards;

        dialoguesPlayedInCurrentSequence = 0;

        HideInteractionPrompt();

        StartNextPendingDialogue();
    }

    private void StartNextPendingDialogue()
    {
        if (!dialogueUIController)
        {
            FinishSpecialDialogueSequence();
            return;
        }

        if (
            catsharkDialoguePending &&
            !catsharkDialogueCompleted
        )
        {
            catsharkDialoguePending = false;
            catsharkDialogueCompleted = true;

            StartSpecialDialogue(
                catsharkDialogueLines
            );

            return;
        }

        if (
            crystalEelDialoguePending &&
            !crystalEelDialogueCompleted
        )
        {
            crystalEelDialoguePending = false;
            crystalEelDialogueCompleted = true;

            StartSpecialDialogue(
                crystalEelDialogueLines
            );

            return;
        }

        if (
            prismTroutDialoguePending &&
            !prismTroutDialogueCompleted
        )
        {
            prismTroutDialoguePending = false;
            prismTroutDialogueCompleted = true;

            StartSpecialDialogue(
                prismTroutDialogueLines
            );

            return;
        }

        if (
            hookUpgradeDialoguePending &&
            !hookUpgradeDialogueCompleted
        )
        {
            hookUpgradeDialoguePending = false;
            hookUpgradeDialogueCompleted = true;

            StartSpecialDialogue(
                hookUpgradeDialogueLines
            );

            return;
        }

        if (
            lineUpgradeDialoguePending &&
            !lineUpgradeDialogueCompleted
        )
        {
            lineUpgradeDialoguePending = false;
            lineUpgradeDialogueCompleted = true;

            StartSpecialDialogue(
                lineUpgradeDialogueLines
            );

            return;
        }

        if (
            purpleSkinDialoguePending &&
            !purpleSkinDialogueCompleted
        )
        {
            purpleSkinDialoguePending = false;
            purpleSkinDialogueCompleted = true;

            StartSpecialDialogue(
                purpleSkinDialogueLines
            );

            return;
        }

        FinishSpecialDialogueSequence();
    }

    private void StartSpecialDialogue(
        string[] dialogueLines
    )
    {
        string[] linesToPlay =
            dialogueLines;

        if (
            dialoguesPlayedInCurrentSequence > 0
        )
        {
            linesToPlay =
                AddAdditionalDialogueLine(
                    dialogueLines
                );
        }

        dialoguesPlayedInCurrentSequence++;

        dialogueUIController.StartDialogue(
            sellerName,
            sellerPortrait,
            linesToPlay
        );
    }

    private string[] AddAdditionalDialogueLine(
        string[] dialogueLines
    )
    {
        string[] combinedLines =
            new string[
                dialogueLines.Length + 1
            ];

        combinedLines[0] =
            additionalDialogueLine;

        for (
            int i = 0;
            i < dialogueLines.Length;
            i++
        )
        {
            combinedLines[i + 1] =
                dialogueLines[i];
        }

        return combinedLines;
    }

    private void FinishSpecialDialogueSequence()
    {
        specialDialogueSequenceRunning = false;

        dialoguesPlayedInCurrentSequence = 0;

        bool shouldOpenMenu =
            openMenuAfterDialogueSequence;

        openMenuAfterDialogueSequence = false;

        interactionKeyWasReleased = false;

        if (shouldOpenMenu)
        {
            OpenSellerMenu();
        }
    }

    // =========================================================
    // DIALOG WURDE BEENDET
    // =========================================================

    private void HandleDialogueClosed()
    {
        if (introductionDialogueIsRunning)
        {
            introductionDialogueIsRunning = false;

            introductionCompleted = true;

            interactionKeyWasReleased = false;

            OpenSellerMenu();

            return;
        }

        if (specialDialogueSequenceRunning)
        {
            StartNextPendingDialogue();
        }
    }

    // =========================================================
    // DIALOGE VORMERKEN
    // =========================================================

    public void QueueCatsharkDialogue()
    {
        if (catsharkDialogueCompleted)
        {
            return;
        }

        catsharkDialoguePending = true;
    }

    public void QueueCrystalEelDialogue()
    {
        if (crystalEelDialogueCompleted)
        {
            return;
        }

        crystalEelDialoguePending = true;
    }

    public void QueuePrismTroutDialogue()
    {
        if (prismTroutDialogueCompleted)
        {
            return;
        }

        prismTroutDialoguePending = true;
    }

    public void QueueHookUpgradeDialogue()
    {
        if (hookUpgradeDialogueCompleted)
        {
            return;
        }

        hookUpgradeDialoguePending = true;
    }

    public void QueueLineUpgradeDialogue()
    {
        if (lineUpgradeDialogueCompleted)
        {
            return;
        }

        lineUpgradeDialoguePending = true;
    }

    public void QueuePurpleSkinDialogue()
    {
        if (purpleSkinDialogueCompleted)
        {
            return;
        }

        purpleSkinDialoguePending = true;
    }

    private bool HasPendingSpecialDialogue()
    {
        return
            (
                catsharkDialoguePending &&
                !catsharkDialogueCompleted
            ) ||
            (
                crystalEelDialoguePending &&
                !crystalEelDialogueCompleted
            ) ||
            (
                prismTroutDialoguePending &&
                !prismTroutDialogueCompleted
            ) ||
            (
                hookUpgradeDialoguePending &&
                !hookUpgradeDialogueCompleted
            ) ||
            (
                lineUpgradeDialoguePending &&
                !lineUpgradeDialogueCompleted
            ) ||
            (
                purpleSkinDialoguePending &&
                !purpleSkinDialogueCompleted
            );
    }

    // =========================================================
    // SELLER-MENÜ
    // =========================================================

    private void OpenSellerMenu()
    {
        if (!sellerMenuController)
        {
            Debug.LogError(
                "SellerMenuController wurde am Seller nicht zugewiesen.",
                this
            );

            return;
        }

        HideInteractionPrompt();

        sellerMenuController.OpenMenu();
    }

    // =========================================================
    // TRIGGER
    // =========================================================

    private void OnTriggerEnter2D(
        Collider2D other
    )
    {
        PlayerMovement enteringPlayer =
            other.GetComponentInParent<PlayerMovement>();

        if (!enteringPlayer)
        {
            return;
        }

        playerInRange = enteringPlayer;
        playerIsInRange = true;

        interactionKeyWasReleased =
            !Input.GetKey(interactionKey);

        if (
            (!dialogueUIController ||
             !dialogueUIController.IsDialogueActive) &&
            (!sellerMenuController ||
             !sellerMenuController.IsOpen) &&
            interactionKeyWasReleased
        )
        {
            ShowInteractionPrompt();
        }
    }

    private void OnTriggerExit2D(
        Collider2D other
    )
    {
        PlayerMovement leavingPlayer =
            other.GetComponentInParent<PlayerMovement>();

        if (
            !leavingPlayer ||
            leavingPlayer != playerInRange
        )
        {
            return;
        }

        playerInRange = null;
        playerIsInRange = false;

        interactionKeyWasReleased = false;

        HideInteractionPrompt();
    }

    // =========================================================
    // PROMPT
    // =========================================================

    private void ShowInteractionPrompt()
    {
        if (interactionPrompt)
        {
            interactionPrompt.SetActive(true);
        }
    }

    private void HideInteractionPrompt()
    {
        if (interactionPrompt)
        {
            interactionPrompt.SetActive(false);
        }
    }

    public void SetDialogueState(
    bool newIntroductionCompleted,
    bool newCatsharkDialogueCompleted,
    bool newCrystalEelDialogueCompleted,
    bool newPrismTroutDialogueCompleted,
    bool newHookUpgradeDialogueCompleted,
    bool newLineUpgradeDialogueCompleted,
    bool newPurpleSkinDialogueCompleted
    )
    {
        introductionCompleted =
            newIntroductionCompleted;

        catsharkDialogueCompleted =
            newCatsharkDialogueCompleted;

        crystalEelDialogueCompleted =
            newCrystalEelDialogueCompleted;

        prismTroutDialogueCompleted =
            newPrismTroutDialogueCompleted;

        hookUpgradeDialogueCompleted =
            newHookUpgradeDialogueCompleted;

        lineUpgradeDialogueCompleted =
            newLineUpgradeDialogueCompleted;

        purpleSkinDialogueCompleted =
            newPurpleSkinDialogueCompleted;

        // Laufende Session-Zustände beim Laden immer zurücksetzen.
        catsharkDialoguePending = false;
        crystalEelDialoguePending = false;
        prismTroutDialoguePending = false;
        hookUpgradeDialoguePending = false;
        lineUpgradeDialoguePending = false;
        purpleSkinDialoguePending = false;

        introductionDialogueIsRunning = false;
        specialDialogueSequenceRunning = false;
        openMenuAfterDialogueSequence = false;
        dialoguesPlayedInCurrentSequence = 0;
    }
}