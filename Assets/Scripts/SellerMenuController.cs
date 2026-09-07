using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SellerMenuController : MonoBehaviour
{
    [Header("Seller Menu")]
    [SerializeField] private GameObject sellerMenu;
    [SerializeField] private SellerInteraction sellerInteraction;

    [Header("Spieler")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerSkinController playerSkinController;

    [Header("Inventar")]
    [SerializeField] private FishInventory fishInventory;

    [Header("Angel-Upgrades")]
    [SerializeField] private FishingUpgradeState fishingUpgradeState;
    [SerializeField] private FishingHookController fishingHook;

    [Header("Münzen")]
    [SerializeField] private CoinWallet coinWallet;
    [SerializeField] private TMP_Text sellerCoinText;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip coinSellClip;
    [SerializeField] private AudioClip upgradeBuyClip;

    [Header("Close Button")]
    [SerializeField] private Button closeButton;

    [Min(1f)]
    [SerializeField] private float closeButtonHoverScale = 1.06f;

    [Range(0.8f, 1f)]
    [SerializeField] private float closeButtonPressedScale = 0.92f;

    [Min(0.01f)]
    [SerializeField] private float closeButtonPressDuration = 0.08f;

    [Header("Navigation Buttons")]
    [SerializeField] private Button sellButton;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button collectionButton;

    [Header("Navigation Sprites")]
    [SerializeField] private Sprite navButtonNormal;
    [SerializeField] private Sprite navButtonSelected;

    [Header("Content")]
    [SerializeField] private GameObject sellContent;
    [SerializeField] private GameObject upgradeContent;
    [SerializeField] private GameObject collectionContent;
    [SerializeField] private FishCollectionUI fishCollectionUI;

    [Header("Content Buttons")]
    [SerializeField] private Button sellContentButton;
    [SerializeField] private Button catsharkSellContentButton;
    [SerializeField] private Button upgradeContentButton;
    [SerializeField] private Button collectionContentButton;
    [SerializeField] private Button crystalEelSellContentButton;
    [SerializeField] private Button prismTroutSellContentButton;

    // =========================================================
    // VERKAUFEN
    // =========================================================

    [Header("Barsch-Eintrag")]
    [SerializeField] private GameObject perchEntry;
    [SerializeField] private GameObject perchSelectionArrow;
    [SerializeField] private TMP_Text perchCountText;

    [Header("Katzenhai-Eintrag")]
    [SerializeField] private GameObject catsharkEntry;
    [SerializeField] private GameObject catsharkSelectionArrow;
    [SerializeField] private TMP_Text catsharkCountText;

    [Header("Kristallaal-Eintrag")]
    [SerializeField] private GameObject crystalEelEntry;
    [SerializeField] private GameObject crystalEelSelectionArrow;
    [SerializeField] private TMP_Text crystalEelCountText;

    [Header("Prismaforelle-Eintrag")]
    [SerializeField] private GameObject prismTroutEntry;
    [SerializeField] private GameObject prismTroutSelectionArrow;
    [SerializeField] private TMP_Text prismTroutCountText;

    [Header("Barsch-Details")]
    [SerializeField] private GameObject perchDetails;

    [Header("Katzenhai-Details")]
    [SerializeField] private GameObject catsharkDetails;

    [Header("Kristallaal-Details")]
    [SerializeField] private GameObject crystalEelDetails;

    [Header("Prismaforelle-Details")]
    [SerializeField] private GameObject prismTroutDetails;

    [Header("Verkaufspreise")]
    [Min(1)]
    [SerializeField] private int perchSellValue = 1;

    [Min(1)]
    [SerializeField] private int catsharkSellValue = 3;

    [Min(1)]
    [SerializeField] private int crystalEelSellValue = 5;

    [Min(1)]
    [SerializeField] private int prismTroutSellValue = 10;

    [Header("Sell Action Buttons")]
    [SerializeField] private Image sellActionButtonImage;
    [SerializeField] private Image catsharkSellActionButtonImage;
    [SerializeField] private Image crystalEelSellActionButtonImage;
    [SerializeField] private Image prismTroutSellActionButtonImage;

    [SerializeField] private Sprite sellActionButtonNormal;
    [SerializeField] private Sprite sellActionButtonSelected;

    // =========================================================
    // UPGRADES
    // =========================================================

    [Header("Upgrade Selection Arrows")]
    [SerializeField] private GameObject inventoryUpgradeArrow;
    [SerializeField] private GameObject lineUpgradeArrow;
    [SerializeField] private GameObject hookUpgradeArrow;
    [SerializeField] private GameObject hookSpeedUpgradeArrow;
    [SerializeField] private GameObject coinBagUpgradeArrow;
    [SerializeField] private GameObject skinsArrow;

    [Header("Upgrade Details")]
    [SerializeField] private GameObject inventoryUpgradeDetails;
    [SerializeField] private GameObject lineUpgradeDetails;
    [SerializeField] private GameObject hookUpgradeDetails;
    [SerializeField] private GameObject hookSpeedUpgradeDetails;
    [SerializeField] private GameObject coinBagUpgradeDetails;
    [SerializeField] private GameObject skinsDetails;

    [Header("Upgrade Buy Buttons")]
    [SerializeField] private Button inventoryBuyButton;
    [SerializeField] private Button lineBuyButton;
    [SerializeField] private Button hookBuyButton;
    [SerializeField] private Button hookSpeedBuyButton;
    [SerializeField] private Button coinBagBuyButton;
    [SerializeField] private Button skinsBuyButton;

    [Header("Upgrade Buy Button Sprites")]
    [SerializeField] private Sprite buyActionButtonNormal;
    [SerializeField] private Sprite buyActionButtonSelected;

    // =========================================================
    // INVENTAR UPGRADE
    // =========================================================

    [Header("Inventar-Upgrade")]
    [SerializeField] private TMP_Text inventoryUpgradeInfoText;
    [SerializeField] private TMP_Text inventoryUpgradePriceText;
    [SerializeField] private TMP_Text inventoryUpgradeValueText;
    [SerializeField] private GameObject inventoryUpgradeCoinIcon;

    [Min(1)]
    [SerializeField] private int inventoryUpgradeLevel2Price = 1;

    [Min(1)]
    [SerializeField] private int inventoryUpgradeLevel3Price = 20;

    // =========================================================
    // SCHNURLÄNGEN-UPGRADE
    // =========================================================

    [Header("Schnurlängen-Upgrade")]
    [SerializeField] private TMP_Text lineUpgradeInfoText;
    [SerializeField] private TMP_Text lineUpgradePriceText;
    [SerializeField] private TMP_Text lineUpgradeValueText;
    [SerializeField] private GameObject lineUpgradeCoinIcon;

    [Min(1)]
    [SerializeField] private int lineUpgradeLevel2Price = 10;

    [Header("Schnurlängen-Tiefen")]
    [Min(0.1f)]
    [SerializeField] private float lineLevel1Depth = 30f;

    [Min(0.1f)]
    [SerializeField] private float lineLevel2Depth = 45f;

    // =========================================================
    // HAKEN UPGRADE
    // =========================================================

    [Header("Haken-Upgrade")]
    [SerializeField] private TMP_Text hookUpgradeInfoText;
    [SerializeField] private TMP_Text hookUpgradePriceText;
    [SerializeField] private TMP_Text hookUpgradeValueText;
    [SerializeField] private GameObject hookUpgradeCoinIcon;

    [Min(1)]
    [SerializeField] private int hookUpgradeLevel2Price = 5;

    [Min(1)]
    [SerializeField] private int hookUpgradeLevel3Price = 50;

    // =========================================================
    // HAKEN-GESCHWINDIGKEITS-UPGRADE
    // =========================================================

    [Header("Haken-Geschwindigkeits-Upgrade")]
    [SerializeField] private TMP_Text hookSpeedUpgradeInfoText;
    [SerializeField] private TMP_Text hookSpeedUpgradePriceText;
    [SerializeField] private TMP_Text hookSpeedUpgradeValueText;
    [SerializeField] private GameObject hookSpeedUpgradeCoinIcon;

    [Min(1)]
    [SerializeField] private int hookSpeedUpgradeLevel2Price = 15;

    [Min(1)]
    [SerializeField] private int hookSpeedUpgradeLevel3Price = 40;

    [Header("Haken-Geschwindigkeiten")]
    [Min(0.1f)]
    [SerializeField] private float hookSpeedLevel1 = 3f;

    [Min(0.1f)]
    [SerializeField] private float hookSpeedLevel2 = 4f;

    [Min(0.1f)]
    [SerializeField] private float hookSpeedLevel3 = 5f;

    [Header("Button Press")]
    [Range(0.8f, 1f)]
    [SerializeField] private float pressedScale = 0.94f;

    [Min(0.01f)]
    [SerializeField] private float pressDuration = 0.08f;

    [Header("Testing")]
    [SerializeField] private bool enableTestOpen = true;
    [SerializeField] private KeyCode testOpenKey = KeyCode.M;

    public bool IsOpen { get; private set; }

    private Button[] navigationButtons;
    private GameObject[] contentPanels;

    private GameObject[] upgradeSelectionArrows;
    private GameObject[] upgradeDetailPanels;
    private Button[] upgradeBuyButtons;

    private int selectedNavigationIndex;
    private int selectedUpgradeIndex;

    /*
     * 0 = Barsch
     * 1 = Katzenhai
     * 2 = Kristallaal
     * 3 = Prismaforelle
     */
    private int selectedFishIndex;

    private bool contentFocused;
    private bool sellActionFocused;
    private bool upgradeActionFocused;

    private Vector3 perchSellButtonOriginalScale;
    private Vector3 catsharkSellButtonOriginalScale;
    private Vector3 crystalEelSellButtonOriginalScale;
    private Vector3 prismTroutSellButtonOriginalScale;
    private Vector3 closeButtonOriginalScale;

    private Coroutine sellButtonPressCoroutine;
    private Coroutine closeButtonPressCoroutine;
    private Coroutine upgradeBuyButtonPressCoroutine;

    private void Awake()
    {
        navigationButtons = new Button[]
        {
            sellButton,
            upgradeButton,
            collectionButton
        };

        contentPanels = new GameObject[]
        {
            sellContent,
            upgradeContent,
            collectionContent
        };

        upgradeSelectionArrows = new GameObject[]
        {
            inventoryUpgradeArrow,
            lineUpgradeArrow,
            hookUpgradeArrow,
            hookSpeedUpgradeArrow,
            coinBagUpgradeArrow,
            skinsArrow
        };

        upgradeDetailPanels = new GameObject[]
        {
            inventoryUpgradeDetails,
            lineUpgradeDetails,
            hookUpgradeDetails,
            hookSpeedUpgradeDetails,
            coinBagUpgradeDetails,
            skinsDetails
        };

        upgradeBuyButtons = new Button[]
        {
            inventoryBuyButton,
            lineBuyButton,
            hookBuyButton,
            hookSpeedBuyButton,
            coinBagBuyButton,
            skinsBuyButton
        };

        if (sellContentButton)
        {
            perchSellButtonOriginalScale = sellContentButton.transform.localScale;
        }

        if (catsharkSellContentButton)
        {
            catsharkSellButtonOriginalScale = catsharkSellContentButton.transform.localScale;
        }

        if (crystalEelSellContentButton)
        {
            crystalEelSellButtonOriginalScale = crystalEelSellContentButton.transform.localScale;
        }

        if (prismTroutSellContentButton)
        {
            prismTroutSellButtonOriginalScale = prismTroutSellContentButton.transform.localScale;
        }

        if (closeButton)
        {
            closeButtonOriginalScale = closeButton.transform.localScale;
        }
    }

    private void OnEnable()
    {
        if (fishInventory)
        {
            fishInventory.InventoryChanged +=
                UpdateSellInventoryDisplay;
        }

        if (fishingUpgradeState)
        {
            fishingUpgradeState.UpgradesChanged +=
                HandleUpgradesChanged;
        }
    }

    private void OnDisable()
    {
        if (fishInventory)
        {
            fishInventory.InventoryChanged -=
                UpdateSellInventoryDisplay;
        }

        if (fishingUpgradeState)
        {
            fishingUpgradeState.UpgradesChanged -=
                HandleUpgradesChanged;
        }
    }

    private void HandleUpgradesChanged()
    {
        ApplyCurrentLineDepth();
        ApplyCurrentHookSpeed();
        ApplyCurrentCoinBagCapacity();

        UpdateInventoryUpgradeState();
        UpdateLineUpgradeState();
        UpdateHookUpgradeState();
        UpdateHookSpeedUpgradeState();
        UpdateCoinBagUpgradeState();
        UpdateSkinUpgradeState();
    }

    private void Start()
    {
        if (sellButton)
        {
            sellButton.onClick.AddListener(() => SelectNavigationButton(0));
        }

        if (upgradeButton)
        {
            upgradeButton.onClick.AddListener(() => SelectNavigationButton(1));
        }

        if (collectionButton)
        {
            collectionButton.onClick.AddListener(() => SelectNavigationButton(2));
        }

        if (sellContentButton)
        {
            sellContentButton.onClick.AddListener(HandleSellButtonPressed);
        }

        if (catsharkSellContentButton)
        {
            catsharkSellContentButton.onClick.AddListener(HandleSellButtonPressed);
        }

        if (crystalEelSellContentButton)
        {
            crystalEelSellContentButton.onClick.AddListener(HandleSellButtonPressed);
        }

        if (prismTroutSellContentButton)
        {
            prismTroutSellContentButton.onClick.AddListener(HandleSellButtonPressed);
        }

        if (closeButton)
        {
            closeButton.onClick.AddListener(
                HandleCloseButtonPressed
            );
        }

        selectedFishIndex = 0;
        selectedUpgradeIndex = 0;

        ApplyCurrentLineDepth();
        ApplyCurrentHookSpeed();
        ApplyCurrentCoinBagCapacity();

        UpdateSellInventoryDisplay();
        UpdateSellerCoinDisplay();
        UpdateUpgradeSelection();
        UpdateUpgradeBuyButtonVisuals();

        UpdateInventoryUpgradeState();
        UpdateLineUpgradeState();
        UpdateHookUpgradeState();
        UpdateHookSpeedUpgradeState();
        UpdateCoinBagUpgradeState();
        UpdateSkinUpgradeState();

        CloseMenu();
    }

    private void Update()
    {
        if (!IsOpen)
        {
            if (enableTestOpen && Input.GetKeyDown(testOpenKey))
            {
                OpenMenu();
            }

            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseMenu();
            return;
        }

        if (!contentFocused)
        {
            HandleNavigationInput();
        }
        else
        {
            HandleContentInput();
        }
    }

    private void HandleNavigationInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveNavigationSelection(-1);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveNavigationSelection(1);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            FocusContent();
        }
    }

    private void HandleContentInput()
    {
        if (selectedNavigationIndex == 0)
        {
            HandleSellContentInput();
            return;
        }

        if (selectedNavigationIndex == 1)
        {
            HandleUpgradeContentInput();
            return;
        }

        if (selectedNavigationIndex == 2)
        {
            HandleCollectionContentInput();
            return;
        }
    }


    // =========================================================
    // MÜNZSACK-UPGRADE
    // =========================================================

    [Header("Münzsack-Upgrade")]
    [SerializeField] private TMP_Text coinBagUpgradeInfoText;
    [SerializeField] private TMP_Text coinBagUpgradePriceText;
    [SerializeField] private TMP_Text coinBagUpgradeValueText;
    [SerializeField] private GameObject coinBagUpgradeCoinIcon;

    [Min(1)]
    [SerializeField] private int coinBagUpgradeLevel2Price = 10;

    [Min(1)]
    [SerializeField] private int coinBagUpgradeLevel3Price = 30;

    [Header("Münzsack-Kapazität")]
    [Min(1)]
    [SerializeField] private int coinBagLevel1Capacity = 10;

    [Min(1)]
    [SerializeField] private int coinBagLevel2Capacity = 30;

    [Min(1)]
    [SerializeField] private int coinBagLevel3Capacity = 50;

    // =========================================================
    // SKIN-UPGRADE
    // =========================================================

    [Header("Skin-Upgrade")]
    [SerializeField] private TMP_Text skinsUpgradeInfoText;
    [SerializeField] private TMP_Text skinsUpgradePriceText;
    [SerializeField] private TMP_Text skinsUpgradeValueText;
    [SerializeField] private GameObject skinsUpgradeCoinIcon;

    [Min(1)]
    [SerializeField] private int purpleSkinPrice = 100;

    // =========================================================
    // SELL INPUT
    // =========================================================

    private void HandleSellContentInput()
    {
        if (sellActionFocused)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                FocusSellList();
                return;
            }

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                HandleSellButtonPressed();
            }

            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            FocusNavigation();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            FocusSellActionButton();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveFishSelection(-1);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveFishSelection(1);
        }
    }

    private void MoveFishSelection(int direction)
    {
        if (!fishInventory)
        {
            return;
        }

        bool hasPerch = fishInventory.HasPerch;

        bool hasCatshark = fishInventory.HasCatshark;

        bool hasCrystalEel = fishInventory.HasCrystalEel;

        bool hasPrismTrout = fishInventory.HasPrismTrout;

        if (!hasPerch && !hasCatshark && !hasCrystalEel && !hasPrismTrout)
        {
            return;
        }

        int startIndex =
            selectedFishIndex;

        do
        {
            selectedFishIndex += direction;

            if (selectedFishIndex < 0)
            {
                selectedFishIndex = 3;
            }
            else if (selectedFishIndex > 3)
            {
                selectedFishIndex = 0;
            }

            if (IsSelectedFishAvailable())
            {
                break;
            }

        } while (
            selectedFishIndex != startIndex
        );

        sellActionFocused = false;

        UpdateSellFishSelection();
        UpdateSellActionButtonVisual();
    }

    private void FocusSellList()
    {
        if (selectedNavigationIndex != 0 || !fishInventory || !fishInventory.HasFish)
        {
            return;
        }

        contentFocused = true;
        sellActionFocused = false;

        if (EventSystem.current)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }

        UpdateSellFishSelection();
        UpdateSellActionButtonVisual();
    }

    private void FocusSellActionButton()
    {
        if (selectedNavigationIndex != 0 || !IsSelectedFishAvailable())
        {
            return;
        }

        Button button =
            GetSelectedSellButton();

        if (!button || !button.interactable)
        {
            return;
        }

        contentFocused = true;
        sellActionFocused = true;

        if (EventSystem.current)
        {
            EventSystem.current.SetSelectedGameObject(
                button.gameObject
            );
        }

        UpdateSellFishSelection();
        UpdateSellActionButtonVisual();
    }

    private Button GetSelectedSellButton()
    {
        switch (selectedFishIndex)
        {
            case 0:
                return sellContentButton;

            case 1:
                return catsharkSellContentButton;

            case 2:
                return crystalEelSellContentButton;

            case 3:
                return prismTroutSellContentButton;

            default:
                return null;
        }
    }

    private bool IsSelectedFishAvailable()
    {
        if (!fishInventory)
        {
            return false;
        }

        switch (selectedFishIndex)
        {
            case 0:
                return fishInventory.HasPerch;

            case 1:
                return fishInventory.HasCatshark;

            case 2:
                return fishInventory.HasCrystalEel;

            case 3:
                return fishInventory.HasPrismTrout;

            default:
                return false;
        }
    }

    private void EnsureValidFishSelection()
    {
        if (!fishInventory)
        {
            return;
        }

        if (IsSelectedFishAvailable())
        {
            return;
        }

        if (fishInventory.HasPerch)
        {
            selectedFishIndex = 0;
        }
        else if (fishInventory.HasCatshark)
        {
            selectedFishIndex = 1;
        }
        else if (fishInventory.HasCrystalEel)
        {
            selectedFishIndex = 2;
        }
        else if (fishInventory.HasPrismTrout)
        {
            selectedFishIndex = 3;
        }
    }

    private void UpdateSellFishSelection()
    {
        if (!fishInventory)
        {
            return;
        }

        bool hasPerch =
            fishInventory.HasPerch;

        bool hasCatshark =
            fishInventory.HasCatshark;

        bool hasCrystalEel =
            fishInventory.HasCrystalEel;

        bool hasPrismTrout =
            fishInventory.HasPrismTrout;

        if (perchDetails)
        {
            perchDetails.SetActive(hasPerch && selectedFishIndex == 0);
        }

        if (catsharkDetails)
        {
            catsharkDetails.SetActive(hasCatshark && selectedFishIndex == 1);
        }

        if (crystalEelDetails)
        {
            crystalEelDetails.SetActive(hasCrystalEel && selectedFishIndex == 2);
        }

        if (prismTroutDetails)
        {
            prismTroutDetails.SetActive( hasPrismTrout && selectedFishIndex == 3);
        }

        bool showSelectionArrow = contentFocused && selectedNavigationIndex == 0 &&
            !sellActionFocused;

        if (perchSelectionArrow)
        {
            perchSelectionArrow.SetActive(showSelectionArrow && hasPerch && selectedFishIndex == 0);
        }

        if (catsharkSelectionArrow)
        {
            catsharkSelectionArrow.SetActive(showSelectionArrow && hasCatshark && selectedFishIndex == 1);
        }

        if (crystalEelSelectionArrow)
        {
            crystalEelSelectionArrow.SetActive(showSelectionArrow && hasCrystalEel &&selectedFishIndex == 2);
        }

        if (prismTroutSelectionArrow)
        {
            prismTroutSelectionArrow.SetActive(showSelectionArrow && hasPrismTrout && selectedFishIndex == 3);
        }
    }

    private void UpdateSellEntryOrder()
    {
        if (!fishInventory)
        {
            return;
        }

        int currentIndex = 0;

        if (perchEntry && fishInventory.HasPerch)
        {
            perchEntry.transform.SetSiblingIndex(currentIndex);

            currentIndex++;
        }

        if (catsharkEntry && fishInventory.HasCatshark)
        {
            catsharkEntry.transform.SetSiblingIndex(currentIndex);

            currentIndex++;
        }

        if (crystalEelEntry && fishInventory.HasCrystalEel)
        {
            crystalEelEntry.transform.SetSiblingIndex(currentIndex);
        }

        if (prismTroutEntry && fishInventory.HasPrismTrout)
        {
            prismTroutEntry.transform.SetSiblingIndex(currentIndex);
        }
    }

    // =========================================================
    // UPGRADE INPUT
    // =========================================================

    private void HandleUpgradeContentInput()
    {
        if (upgradeActionFocused)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                FocusUpgradeList();
                return;
            }

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                HandleUpgradeBuyButtonPressed();
            }

            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            FocusNavigation();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            FocusUpgradeBuyButton();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveUpgradeSelection(-1);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveUpgradeSelection(1);
        }

    }

    private void HandleCollectionContentInput()
    {
        if (!fishCollectionUI)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            fishCollectionUI.SetFocused(false);

            FocusNavigation();

            return;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            fishCollectionUI.MoveSelection(-1);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            fishCollectionUI.MoveSelection(1);
        }
    }

    public void OpenMenu()
    {
        if (!sellerMenu)
        {
            return;
        }

        sellerMenu.SetActive(true);
        IsOpen = true;

        if (playerMovement)
        {
            playerMovement.SetMovementEnabled(false);
        }

        contentFocused = false;
        sellActionFocused = false;
        upgradeActionFocused = false;

        EnsureValidFishSelection();

        UpdateSellInventoryDisplay();
        UpdateSellerCoinDisplay();

        UpdateInventoryUpgradeState();
        UpdateLineUpgradeState();
        UpdateHookUpgradeState();
        UpdateHookSpeedUpgradeState();
        UpdateCoinBagUpgradeState();
        UpdateSkinUpgradeState();

        selectedUpgradeIndex = 0;

        UpdateUpgradeSelection();
        UpdateUpgradeBuyButtonVisuals();

        SelectNavigationButton(0);
        FocusNavigation();
    }

    public void CloseMenu()
    {
        if (!sellerMenu)
        {
            return;
        }

        bool menuWasOpen =
            IsOpen;

        ResetSellActionButtonVisual();
        ResetSellButtonScale();
        ResetCloseButtonScale();
        ResetUpgradeBuyButtonScale();

        sellerMenu.SetActive(false);

        IsOpen = false;
        contentFocused = false;
        sellActionFocused = false;
        upgradeActionFocused = false;

        if (EventSystem.current)
        {
            EventSystem.current
                .SetSelectedGameObject(null);
        }

        if (playerMovement)
        {
            playerMovement
                .SetMovementEnabled(true);
        }

        /*
        * Nur ein tatsächlich geöffnetes Menü
        * meldet sein Schließen.
        *
        * Dadurch löst das CloseMenu() in Start()
        * keinen Spezialdialog aus.
        */
        if (menuWasOpen &&sellerInteraction)
        {
            sellerInteraction
                .HandleSellerMenuClosed();
        }
    }

    private void MoveNavigationSelection(
        int direction
    )
    {
        int newIndex =
            selectedNavigationIndex + direction;

        if (newIndex < 0)
        {
            newIndex =
                navigationButtons.Length - 1;
        }
        else if (newIndex >= navigationButtons.Length)
        {
            newIndex = 0;
        }

        SelectNavigationButton(newIndex);
    }

    private void SelectNavigationButton(int index)
    {
        if (navigationButtons == null || index < 0 || index >= navigationButtons.Length)
        {
            return;
        }

        selectedNavigationIndex = index;

        contentFocused = false;
        sellActionFocused = false;
        upgradeActionFocused = false;

        if (fishCollectionUI)
        {
            fishCollectionUI.SetFocused(false);
        }

        if (selectedNavigationIndex == 0)
        {
            EnsureValidFishSelection();
        }

        if (selectedNavigationIndex == 1)
        {
            selectedUpgradeIndex = 0;
        }

        UpdateNavigationVisuals();
        UpdateContent();

        UpdateSellFishSelection();
        UpdateSellActionButtonVisual();

        UpdateUpgradeSelection();
        UpdateUpgradeBuyButtonVisuals();

        FocusNavigation();
    }

    private void UpdateNavigationVisuals()
    {
        for (
            int i = 0;
            i < navigationButtons.Length;
            i++
        )
        {
            Button button =
                navigationButtons[i];

            if (!button)
            {
                continue;
            }

            Image buttonImage =
                button.GetComponent<Image>();

            if (!buttonImage)
            {
                continue;
            }

            buttonImage.sprite =
                i == selectedNavigationIndex
                    ? navButtonSelected
                    : navButtonNormal;
        }
    }

    private void UpdateContent()
    {
        if (contentPanels == null)
        {
            return;
        }

        for (int i = 0; i < contentPanels.Length; i++)
        {
            GameObject contentPanel =
                contentPanels[i];

            if (!contentPanel)
            {
                continue;
            }

            contentPanel.SetActive(
                i == selectedNavigationIndex
            );
        }
    }

    // =========================================================
    // UPGRADE AUSWAHL
    // =========================================================

    private void MoveUpgradeSelection(
        int direction
    )
    {
        if (
            upgradeSelectionArrows == null ||
            upgradeSelectionArrows.Length == 0
        )
        {
            return;
        }

        selectedUpgradeIndex += direction;

        if (selectedUpgradeIndex < 0)
        {
            selectedUpgradeIndex =
                upgradeSelectionArrows.Length - 1;
        }
        else if (selectedUpgradeIndex >= upgradeSelectionArrows.Length)
        {
            selectedUpgradeIndex = 0;
        }

        UpdateUpgradeSelection();
        UpdateUpgradeBuyButtonVisuals();
    }

    private void UpdateUpgradeSelection()
    {
        if (upgradeSelectionArrows != null)
        {
            for (int i = 0; i < upgradeSelectionArrows.Length; i++)
            {
                GameObject selectionArrow =
                    upgradeSelectionArrows[i];

                if (!selectionArrow)
                {
                    continue;
                }

                selectionArrow.SetActive(
                    contentFocused &&
                    selectedNavigationIndex == 1 &&
                    !upgradeActionFocused &&
                    i == selectedUpgradeIndex
                );
            }
        }

        if (upgradeDetailPanels != null)
        {
            for (int i = 0; i < upgradeDetailPanels.Length; i++)
            {
                GameObject detailPanel =
                    upgradeDetailPanels[i];

                if (!detailPanel)
                {
                    continue;
                }

                detailPanel.SetActive(i == selectedUpgradeIndex);
            }
        }
    }

    private void FocusUpgradeList()
    {
        if (selectedNavigationIndex != 1)
        {
            return;
        }

        contentFocused = true;
        upgradeActionFocused = false;

        if (EventSystem.current)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }

        UpdateUpgradeSelection();
        UpdateUpgradeBuyButtonVisuals();
    }

    private void FocusUpgradeBuyButton()
    {
        if (selectedNavigationIndex != 1 || upgradeBuyButtons == null || selectedUpgradeIndex < 0 ||
            selectedUpgradeIndex >=
            upgradeBuyButtons.Length
        )
        {
            return;
        }

        Button button =
            upgradeBuyButtons[
                selectedUpgradeIndex
            ];

        if (!button || !button.interactable)
        {
            return;
        }

        contentFocused = true;
        upgradeActionFocused = true;

        if (EventSystem.current)
        {
            EventSystem.current.SetSelectedGameObject(button.gameObject);
        }

        UpdateUpgradeSelection();
        UpdateUpgradeBuyButtonVisuals();
    }

    private void UpdateUpgradeBuyButtonVisuals()
    {
        if (upgradeBuyButtons == null)
        {
            return;
        }

        for (int i = 0; i < upgradeBuyButtons.Length; i++)
        {
            Button button =
                upgradeBuyButtons[i];

            if (!button)
            {
                continue;
            }

            Image buttonImage =
                button.GetComponent<Image>();

            if (!buttonImage)
            {
                continue;
            }

            bool isSelected =
                selectedNavigationIndex == 1 &&
                contentFocused &&
                upgradeActionFocused &&
                i == selectedUpgradeIndex &&
                button.interactable;

            buttonImage.sprite =
                isSelected
                    ? buyActionButtonSelected
                    : buyActionButtonNormal;
        }
    }

    // =========================================================
    // INVENTAR UPGRADE
    // =========================================================

    private void UpdateInventoryUpgradeState()
    {
        if (!fishInventory)
        {
            return;
        }

        int maximumSlots =
            fishInventory.MaximumFishCount;

        if (maximumSlots < 5)
        {
            if (inventoryUpgradeInfoText)
            {
                inventoryUpgradeInfoText.text =
                    "1 → 5";
            }

            if (inventoryUpgradePriceText)
            {
                inventoryUpgradePriceText.text =
                    inventoryUpgradeLevel2Price.ToString();
            }

            if (inventoryUpgradeValueText)
            {
                inventoryUpgradeValueText.text =
                    "1 → 5 Slots";
            }

            if (inventoryUpgradeCoinIcon)
            {
                inventoryUpgradeCoinIcon.SetActive(true);
            }

            if (inventoryBuyButton)
            {
                inventoryBuyButton.gameObject.SetActive(true);
                inventoryBuyButton.interactable = true;
            }

            return;
        }

        if (maximumSlots < 10)
        {
            if (inventoryUpgradeInfoText)
            {
                inventoryUpgradeInfoText.text =
                    "5 → 10";
            }

            if (inventoryUpgradePriceText)
            {
                inventoryUpgradePriceText.text =
                    inventoryUpgradeLevel3Price.ToString();
            }

            if (inventoryUpgradeValueText)
            {
                inventoryUpgradeValueText.text =
                    "5 → 10 Slots";
            }

            if (inventoryUpgradeCoinIcon)
            {
                inventoryUpgradeCoinIcon.SetActive(true);
            }

            if (inventoryBuyButton)
            {
                inventoryBuyButton.gameObject.SetActive(true);
                inventoryBuyButton.interactable = true;
            }

            return;
        }

        if (inventoryUpgradeInfoText)
        {
            inventoryUpgradeInfoText.text =
                "MAX";
        }

        if (inventoryUpgradePriceText)
        {
            inventoryUpgradePriceText.text =
                "";
        }

        if (inventoryUpgradeValueText)
        {
            inventoryUpgradeValueText.text =
                "10 Slots - MAX";
        }

        if (inventoryUpgradeCoinIcon)
        {
            inventoryUpgradeCoinIcon.SetActive(false);
        }

        if (inventoryBuyButton)
        {
            inventoryBuyButton.gameObject.SetActive(false);
        }
    }

    // =========================================================
    // SCHNURLÄNGEN-UPGRADE
    // =========================================================

    private void UpdateLineUpgradeState()
    {
        if (!fishingUpgradeState)
        {
            return;
        }

        int lineLevel =
            fishingUpgradeState.LineLevel;

        if (lineLevel == 1)
        {
            if (lineUpgradeInfoText)
            {
                lineUpgradeInfoText.text =
                    "10 m → 20 m";
            }

            if (lineUpgradePriceText)
            {
                lineUpgradePriceText.text =
                    lineUpgradeLevel2Price.ToString();
            }

            if (lineUpgradeValueText)
            {
                lineUpgradeValueText.text =
                    "10 m → 20 m";
            }

            if (lineUpgradeCoinIcon)
            {
                lineUpgradeCoinIcon.SetActive(true);
            }

            if (lineBuyButton)
            {
                lineBuyButton.gameObject.SetActive(true);
                lineBuyButton.interactable = true;
            }

            return;
        }

        if (lineUpgradeInfoText)
        {
            lineUpgradeInfoText.text =
                "MAX";
        }

        if (lineUpgradePriceText)
        {
            lineUpgradePriceText.text =
                "";
        }

        if (lineUpgradeValueText)
        {
            lineUpgradeValueText.text =
                "20 m - MAX";
        }

        if (lineUpgradeCoinIcon)
        {
            lineUpgradeCoinIcon.SetActive(false);
        }

        if (lineBuyButton)
        {
            lineBuyButton.gameObject.SetActive(false);
        }
    }

    private void UpdateSkinUpgradeState()
    {
        if (!fishingUpgradeState)
        {
            return;
        }

        if (!fishingUpgradeState.PurpleSkinPurchased)
        {
            if (skinsUpgradeInfoText)
            {
                skinsUpgradeInfoText.text =
                    "Purple Skin";
            }

            if (skinsUpgradePriceText)
            {
                skinsUpgradePriceText.text =
                    purpleSkinPrice.ToString();
            }

            if (skinsUpgradeValueText)
            {
                skinsUpgradeValueText.text =
                    "Purple Outfit";
            }

            if (skinsUpgradeCoinIcon)
            {
                skinsUpgradeCoinIcon.SetActive(true);
            }

            if (skinsBuyButton)
            {
                skinsBuyButton.gameObject.SetActive(true);
                skinsBuyButton.interactable = true;
            }

            return;
        }

        if (skinsUpgradeInfoText)
        {
            skinsUpgradeInfoText.text =
                "MAX";
        }

        if (skinsUpgradePriceText)
        {
            skinsUpgradePriceText.text = "";
        }

        if (skinsUpgradeValueText)
        {
            skinsUpgradeValueText.text =
                "Purple Outfit - MAX";
        }

        if (skinsUpgradeCoinIcon)
        {
            skinsUpgradeCoinIcon.SetActive(false);
        }

        if (skinsBuyButton)
        {
            skinsBuyButton.gameObject.SetActive(false);
        }
    }

    private void TryBuyPurpleSkin()
    {
        if (!fishingUpgradeState || !coinWallet || !playerSkinController ||
            selectedUpgradeIndex != 5)
        {
            return;
        }

        if (fishingUpgradeState.PurpleSkinPurchased)
        {
            UpdateSkinUpgradeState();
            return;
        }

        bool coinsWereSpent =
            coinWallet.SpendCoins(
                purpleSkinPrice
            );

        if (!coinsWereSpent)
        {
            Debug.Log(
                "Nicht genügend Münzen für den Purple Skin.",
                this
            );

            return;
        }

        fishingUpgradeState.PurchasePurpleSkin();
        playerSkinController.UsePurpleSkin();

        PlayUpgradeBuySound();

        if (sellerInteraction)
        {
            sellerInteraction
                .QueuePurpleSkinDialogue();
        }

        UpdateSellerCoinDisplay();
        UpdateSkinUpgradeState();

        upgradeActionFocused = false;

        UpdateUpgradeSelection();
        UpdateUpgradeBuyButtonVisuals();
    }

    private void ApplyCurrentLineDepth()
    {
        if (!fishingUpgradeState || !fishingHook)
        {
            return;
        }

        float targetDepth =
            fishingUpgradeState.LineLevel == 1
                ? lineLevel1Depth
                : lineLevel2Depth;

        fishingHook.SetMaximumLineDepth(
            targetDepth
        );
    }

    private void TryBuyLineUpgrade()
    {
        if (!fishingUpgradeState || !fishingHook || !coinWallet ||
            selectedUpgradeIndex != 1)
        {
            return;
        }

        if (fishingUpgradeState.LineLevel >= 2)
        {
            UpdateLineUpgradeState();
            return;
        }

        bool coinsWereSpent =
            coinWallet.SpendCoins(
                lineUpgradeLevel2Price
            );

        if (!coinsWereSpent)
        {
            Debug.Log(
                "Nicht genügend Münzen für das Schnurlängen-Upgrade.",
                this
            );

            return;
        }

        fishingUpgradeState.SetLineLevel(2);
        ApplyCurrentLineDepth();

        PlayUpgradeBuySound();

        if (sellerInteraction)
        {
            sellerInteraction
                .QueueLineUpgradeDialogue();
        }

        UpdateSellerCoinDisplay();
        UpdateLineUpgradeState();

        upgradeActionFocused = false;


        UpdateUpgradeSelection();
        UpdateUpgradeBuyButtonVisuals();

        Debug.Log(
            "Schnurlänge verbessert. Neue maximale Tiefe: " +
            lineLevel2Depth,
            this
        );
    }

    // =========================================================
    // HAKEN UPGRADE
    // =========================================================

    private void UpdateHookUpgradeState()
    {
        if (!fishingUpgradeState)
        {
            return;
        }

        int hookLevel =
            fishingUpgradeState.HookLevel;

        if (hookLevel == 1)
        {
            if (hookUpgradeInfoText)
            {
                hookUpgradeInfoText.text =
                    "Stufe 1 → 2";
            }

            if (hookUpgradePriceText)
            {
                hookUpgradePriceText.text =
                    hookUpgradeLevel2Price.ToString();
            }

            if (hookUpgradeValueText)
            {
                hookUpgradeValueText.text =
                    "Stufe 1 → 2";
            }

            if (hookUpgradeCoinIcon)
            {
                hookUpgradeCoinIcon.SetActive(true);
            }

            if (hookBuyButton)
            {
                hookBuyButton.gameObject.SetActive(true);
                hookBuyButton.interactable = true;
            }

            return;
        }

        if (hookLevel == 2)
        {
            if (hookUpgradeInfoText)
            {
                hookUpgradeInfoText.text =
                    "Stufe 2 → 3";
            }

            if (hookUpgradePriceText)
            {
                hookUpgradePriceText.text =
                    hookUpgradeLevel3Price.ToString();
            }

            if (hookUpgradeValueText)
            {
                hookUpgradeValueText.text =
                    "Stufe 2 → 3";
            }

            if (hookUpgradeCoinIcon)
            {
                hookUpgradeCoinIcon.SetActive(true);
            }

            if (hookBuyButton)
            {
                hookBuyButton.gameObject.SetActive(true);
                hookBuyButton.interactable = true;
            }

            return;
        }

        if (hookUpgradeInfoText)
        {
            hookUpgradeInfoText.text =
                "MAX";
        }

        if (hookUpgradePriceText)
        {
            hookUpgradePriceText.text =
                "";
        }

        if (hookUpgradeValueText)
        {
            hookUpgradeValueText.text =
                "Stufe 3 - MAX";
        }

        if (hookUpgradeCoinIcon)
        {
            hookUpgradeCoinIcon.SetActive(false);
        }

        if (hookBuyButton)
        {
            hookBuyButton.gameObject.SetActive(false);
        }
    }

    private void UpdateCoinBagUpgradeState()
    {
        if (!fishingUpgradeState)
        {
            return;
        }

        int coinBagLevel =
            fishingUpgradeState.CoinBagLevel;

        if (coinBagLevel == 1)
        {
            if (coinBagUpgradeInfoText)
            {
                coinBagUpgradeInfoText.text =
                    "10 → 30";
            }

            if (coinBagUpgradePriceText)
            {
                coinBagUpgradePriceText.text =
                    coinBagUpgradeLevel2Price.ToString();
            }

            if (coinBagUpgradeValueText)
            {
                coinBagUpgradeValueText.text =
                    "10 → 30 Münzen";
            }

            if (coinBagUpgradeCoinIcon)
            {
                coinBagUpgradeCoinIcon.SetActive(true);
            }

            if (coinBagBuyButton)
            {
                coinBagBuyButton.gameObject.SetActive(true);
                coinBagBuyButton.interactable = true;
            }

            return;
        }

        if (coinBagLevel == 2)
        {
            if (coinBagUpgradeInfoText)
            {
                coinBagUpgradeInfoText.text =
                    "30 → 50";
            }

            if (coinBagUpgradePriceText)
            {
                coinBagUpgradePriceText.text =
                    coinBagUpgradeLevel3Price.ToString();
            }

            if (coinBagUpgradeValueText)
            {
                coinBagUpgradeValueText.text =
                    "30 → 50 Münzen";
            }

            if (coinBagUpgradeCoinIcon)
            {
                coinBagUpgradeCoinIcon.SetActive(true);
            }

            if (coinBagBuyButton)
            {
                coinBagBuyButton.gameObject.SetActive(true);
                coinBagBuyButton.interactable = true;
            }

            return;
        }

        if (coinBagUpgradeInfoText)
        {
            coinBagUpgradeInfoText.text =
                "MAX";
        }

        if (coinBagUpgradePriceText)
        {
            coinBagUpgradePriceText.text =
                "";
        }

        if (coinBagUpgradeValueText)
        {
            coinBagUpgradeValueText.text =
                "50 Münzen - MAX";
        }

        if (coinBagUpgradeCoinIcon)
        {
            coinBagUpgradeCoinIcon.SetActive(false);
        }

        if (coinBagBuyButton)
        {
            coinBagBuyButton.gameObject.SetActive(false);
        }
    }

    private void ApplyCurrentCoinBagCapacity()
    {
        if (!fishingUpgradeState || !coinWallet)
        {
            return;
        }

        int targetCapacity;

        switch (fishingUpgradeState.CoinBagLevel)
        {
            case 1:
                targetCapacity = coinBagLevel1Capacity;
                break;

            case 2:
                targetCapacity = coinBagLevel2Capacity;
                break;

            default:
                targetCapacity = coinBagLevel3Capacity;
                break;
        }

        coinWallet.SetMaximumCoins(targetCapacity);
    }

    private void TryBuyCoinBagUpgrade()
    {
        if (
            !fishingUpgradeState ||
            !coinWallet ||
            selectedUpgradeIndex != 4
        )
        {
            return;
        }

        int currentLevel =
            fishingUpgradeState.CoinBagLevel;

        int targetLevel;
        int price;

        if (currentLevel == 1)
        {
            targetLevel = 2;
            price = coinBagUpgradeLevel2Price;
        }
        else if (currentLevel == 2)
        {
            targetLevel = 3;
            price = coinBagUpgradeLevel3Price;
        }
        else
        {
            UpdateCoinBagUpgradeState();
            return;
        }

        bool coinsWereSpent =
            coinWallet.SpendCoins(
                price
            );

        if (!coinsWereSpent)
        {
            Debug.Log("Nicht genügend Münzen für das Münzsack-Upgrade.",
                this
            );

            return;
        }

        fishingUpgradeState.SetCoinBagLevel(
            targetLevel
        );

        ApplyCurrentCoinBagCapacity();

        PlayUpgradeBuySound();

        UpdateSellerCoinDisplay();
        UpdateCoinBagUpgradeState();

        upgradeActionFocused = false;

        UpdateUpgradeSelection();
        UpdateUpgradeBuyButtonVisuals();
    }

    private void UpdateHookSpeedUpgradeState()
    {
        if (!fishingUpgradeState)
        {
            return;
        }

        int hookSpeedLevel =
            fishingUpgradeState.HookSpeedLevel;

        if (hookSpeedLevel == 1)
        {
            if (hookSpeedUpgradeInfoText)
            {
                hookSpeedUpgradeInfoText.text =
                    "Stufe 1 → 2";
            }

            if (hookSpeedUpgradePriceText)
            {
                hookSpeedUpgradePriceText.text =
                    hookSpeedUpgradeLevel2Price.ToString();
            }

            if (hookSpeedUpgradeValueText)
            {
                hookSpeedUpgradeValueText.text =
                    "Stufe 1 → 2";
            }

            if (hookSpeedUpgradeCoinIcon)
            {
                hookSpeedUpgradeCoinIcon.SetActive(true);
            }

            if (hookSpeedBuyButton)
            {
                hookSpeedBuyButton.gameObject.SetActive(true);
                hookSpeedBuyButton.interactable = true;
            }

            return;
        }

        if (hookSpeedLevel == 2)
        {
            if (hookSpeedUpgradeInfoText)
            {
                hookSpeedUpgradeInfoText.text =
                    "Stufe 2 → 3";
            }

            if (hookSpeedUpgradePriceText)
            {
                hookSpeedUpgradePriceText.text =
                    hookSpeedUpgradeLevel3Price.ToString();
            }

            if (hookSpeedUpgradeValueText)
            {
                hookSpeedUpgradeValueText.text =
                    "Stufe 2 → 3";
            }

            if (hookSpeedUpgradeCoinIcon)
            {
                hookSpeedUpgradeCoinIcon.SetActive(true);
            }

            if (hookSpeedBuyButton)
            {
                hookSpeedBuyButton.gameObject.SetActive(true);
                hookSpeedBuyButton.interactable = true;
            }

            return;
        }

        if (hookSpeedUpgradeInfoText)
        {
            hookSpeedUpgradeInfoText.text =
                "MAX";
        }

        if (hookSpeedUpgradePriceText)
        {
            hookSpeedUpgradePriceText.text =
                "";
        }

        if (hookSpeedUpgradeValueText)
        {
            hookSpeedUpgradeValueText.text =
                "Stufe 3 - MAX";
        }

        if (hookSpeedUpgradeCoinIcon)
        {
            hookSpeedUpgradeCoinIcon.SetActive(false);
        }

        if (hookSpeedBuyButton)
        {
            hookSpeedBuyButton.gameObject.SetActive(false);
        }
    }

    private void ApplyCurrentHookSpeed()
    {
        if (!fishingUpgradeState || !fishingHook)
        {
            return;
        }

        float targetSpeed;

        switch (fishingUpgradeState.HookSpeedLevel)
        {
            case 1:
                targetSpeed = hookSpeedLevel1;
                break;

            case 2:
                targetSpeed = hookSpeedLevel2;
                break;

            default:
                targetSpeed = hookSpeedLevel3;
                break;
        }

        fishingHook.SetMovementSpeed(
            targetSpeed
        );
    }

    private void TryBuyHookSpeedUpgrade()
    {
        if (!fishingUpgradeState || !fishingHook || !coinWallet ||
            selectedUpgradeIndex != 3
        )
        {
            return;
        }

        int currentLevel = fishingUpgradeState.HookSpeedLevel;

        int targetLevel;
        int price;

        if (currentLevel == 1)
        {
            targetLevel = 2;
            price =
                hookSpeedUpgradeLevel2Price;
        }
        else if (currentLevel == 2)
        {
            targetLevel = 3;
            price =
                hookSpeedUpgradeLevel3Price;
        }
        else
        {
            UpdateHookSpeedUpgradeState();
            return;
        }

        bool coinsWereSpent =
            coinWallet.SpendCoins(
                price
            );

        if (!coinsWereSpent)
        {
            Debug.Log(
                "Nicht genügend Münzen für das Haken-Geschwindigkeits-Upgrade.",
                this
            );

            return;
        }

        fishingUpgradeState.SetHookSpeedLevel(
            targetLevel
        );

        ApplyCurrentHookSpeed();

        PlayUpgradeBuySound();

        UpdateSellerCoinDisplay();
        UpdateHookSpeedUpgradeState();

        upgradeActionFocused = false;

        UpdateUpgradeSelection();
        UpdateUpgradeBuyButtonVisuals();
    }

    private void TryBuyInventoryUpgrade()
    {
        if (
            !fishInventory || !coinWallet ||
            selectedUpgradeIndex != 0
        )
        {
            return;
        }

        int currentMaximum =
            fishInventory.MaximumFishCount;

        int targetSlots;
        int price;

        if (currentMaximum < 5)
        {
            targetSlots = 5;
            price = inventoryUpgradeLevel2Price;
        }
        else if (currentMaximum < 10)
        {
            targetSlots = 10;
            price = inventoryUpgradeLevel3Price;
        }
        else
        {
            UpdateInventoryUpgradeState();
            return;
        }

        bool coinsWereSpent =
            coinWallet.SpendCoins(price);

        if (!coinsWereSpent)
        {
            Debug.Log(
                "Nicht genügend Münzen für das Inventar-Upgrade.",
                this
            );

            return;
        }

        fishInventory.SetMaximumFishCount(
            targetSlots
        );

        PlayUpgradeBuySound();

        UpdateSellerCoinDisplay();
        UpdateInventoryUpgradeState();

        upgradeActionFocused = false;

        UpdateUpgradeSelection();
        UpdateUpgradeBuyButtonVisuals();
    }

    private void TryBuyHookUpgrade()
    {
        if (
            !fishingUpgradeState || !coinWallet ||
            selectedUpgradeIndex != 2
        )
        {
            return;
        }

        int currentHookLevel =
            fishingUpgradeState.HookLevel;

        int targetLevel;
        int price;

        if (currentHookLevel == 1)
        {
            targetLevel = 2;
            price = hookUpgradeLevel2Price;
        }
        else if (currentHookLevel == 2)
        {
            targetLevel = 3;
            price = hookUpgradeLevel3Price;
        }
        else
        {
            UpdateHookUpgradeState();
            return;
        }

        bool coinsWereSpent =
            coinWallet.SpendCoins(price);

        if (!coinsWereSpent)
        {
            Debug.Log(
                "Nicht genügend Münzen für das Haken-Upgrade.",
                this
            );

            return;
        }

        fishingUpgradeState.SetHookLevel(targetLevel);

        PlayUpgradeBuySound();

        if (sellerInteraction)
        {
            sellerInteraction
                .QueueHookUpgradeDialogue();
        }

        UpdateSellerCoinDisplay();
        UpdateHookUpgradeState();

        upgradeActionFocused = false;

        UpdateUpgradeSelection();
        UpdateUpgradeBuyButtonVisuals();
    }

    private void PlayUpgradeBuySound()
    {
        if (audioSource && upgradeBuyClip)
        {
            audioSource.PlayOneShot(upgradeBuyClip);
        }
    }

    // =========================================================
    // SELL INVENTORY
    // =========================================================

    private void UpdateSellInventoryDisplay()
    {
        if (!fishInventory)
        {
            return;
        }

        bool hasPerch =
            fishInventory.HasPerch;

        bool hasCatshark =
            fishInventory.HasCatshark;

        bool hasCrystalEel =
            fishInventory.HasCrystalEel;

        bool hasPrismTrout =
            fishInventory.HasPrismTrout;

            UpdateSellEntryOrder();

        if (perchEntry)
        {
            perchEntry.SetActive(hasPerch);
        }

        if (catsharkEntry)
        {
            catsharkEntry.SetActive(hasCatshark);
        }

        if (crystalEelEntry)
        {
            crystalEelEntry.SetActive(
                hasCrystalEel
            );
        }

        if (prismTroutEntry)
        {
            prismTroutEntry.SetActive(
                hasPrismTrout
            );
        }

        if (perchCountText)
        {
            perchCountText.text =
                "x" +
                fishInventory.PerchCount;
        }

        if (catsharkCountText)
        {
            catsharkCountText.text =
                "x" +
                fishInventory.CatsharkCount;
        }

        if (crystalEelCountText)
        {
            crystalEelCountText.text =
                "x" +
                fishInventory.CrystalEelCount;
        }

        if (prismTroutCountText)
        {
            prismTroutCountText.text =
                "x" +
                fishInventory.PrismTroutCount;
        }

        EnsureValidFishSelection();

        UpdateSellFishSelection();
        UpdateSellActionButtonVisual();

        if (!hasPerch && !hasCatshark && !hasCrystalEel && !hasPrismTrout && contentFocused && selectedNavigationIndex == 0)
        {
            FocusNavigation();
        }
    }

    private void UpdateSellerCoinDisplay()
    {
        if (!coinWallet || !sellerCoinText)
        {
            return;
        }

        sellerCoinText.text =
            coinWallet.Coins.ToString();
    }

    // =========================================================
    // FOCUS
    // =========================================================

    private void FocusNavigation()
    {
        contentFocused = false;
        sellActionFocused = false;
        upgradeActionFocused = false;

        if (fishCollectionUI)
        {
            fishCollectionUI.SetFocused(false);
        }

        ResetSellActionButtonVisual();
        ResetSellButtonScale();

        UpdateSellFishSelection();
        UpdateUpgradeSelection();
        UpdateUpgradeBuyButtonVisuals();

        if (
            navigationButtons == null ||
            selectedNavigationIndex < 0 ||
            selectedNavigationIndex >=
            navigationButtons.Length
        )
        {
            return;
        }

        Button button =
            navigationButtons[
                selectedNavigationIndex
            ];

        if (!button)
        {
            return;
        }

        if (EventSystem.current)
        {
            EventSystem.current.SetSelectedGameObject(
                button.gameObject
            );
        }
    }

    private void FocusContent()
    {
        if (selectedNavigationIndex == 0)
        {
            if (!fishInventory || !fishInventory.HasFish)
            {
                return;
            }

            FocusSellList();
            return;
        }

        if (selectedNavigationIndex == 1)
        {
            FocusUpgradeList();
            return;
        }

        if (selectedNavigationIndex == 2)
        {
            contentFocused = true;

            if (fishCollectionUI)
            {
                fishCollectionUI.SetFocused(true);
            }

            if (EventSystem.current)
            {
                EventSystem.current.SetSelectedGameObject(
                    null
                );
            }

            return;
        }

        contentFocused = true;
    }

    // =========================================================
    // MOUSE NAVIGATION
    // =========================================================

    public void HoverSellButton()
    {
        SelectNavigationButton(0);
    }

    public void HoverUpgradeButton()
    {
        SelectNavigationButton(1);
    }

    public void HoverCollectionButton()
    {
        SelectNavigationButton(2);
    }

    public void HoverPerchEntry()
    {
        SelectFishFromMouse(0);
    }

    public void HoverCatsharkEntry()
    {
        SelectFishFromMouse(1);
    }

    public void HoverCrystalEelEntry()
    {
        SelectFishFromMouse(2);
    }

    public void HoverPrismTroutEntry()
    {
        SelectFishFromMouse(3);
    }

    private void SelectFishFromMouse(
        int index
    )
    {
        if (!IsOpen || selectedNavigationIndex != 0 || !fishInventory)
        {
            return;
        }

        if (index == 0 &&!fishInventory.HasPerch)
        {
            return;
        }

        if (index == 1 &&!fishInventory.HasCatshark)
        {
            return;
        }

        if (index == 2 &&!fishInventory.HasCrystalEel)
        {
            return;
        }

        if (index == 3 && !fishInventory.HasPrismTrout)
        {
            return;
        }

        selectedFishIndex = index;

        contentFocused = true;
        sellActionFocused = false;

        if (EventSystem.current)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }

        UpdateSellFishSelection();
        UpdateSellActionButtonVisual();
    }

    public void HoverSellActionButton()
    {
        FocusSellActionButton();
    }

    public void HoverPerchSellActionButton()
    {
        if (!fishInventory || !fishInventory.HasPerch)
        {
            return;
        }

        selectedFishIndex = 0;
        FocusSellActionButton();
    }

    public void HoverCatsharkSellActionButton()
    {
        if (!fishInventory || !fishInventory.HasCatshark)
        {
            return;
        }

        selectedFishIndex = 1;
        FocusSellActionButton();
    }

    public void HoverCrystalEelSellActionButton()
    {
        if (!fishInventory || !fishInventory.HasCrystalEel)
        {
            return;
        }

        selectedFishIndex = 2;
        FocusSellActionButton();
    }

    public void HoverPrismTroutSellActionButton()
    {
        if (
            !fishInventory ||
            !fishInventory.HasPrismTrout
        )
        {
            return;
        }

        selectedFishIndex = 3;
        FocusSellActionButton();
    }

    public void ExitSellActionButton()
    {
        if (
            selectedNavigationIndex != 0 ||
            !contentFocused
        )
        {
            return;
        }

        sellActionFocused = false;

        if (EventSystem.current)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }

        UpdateSellFishSelection();
        UpdateSellActionButtonVisual();
    }

    public void HoverInventoryUpgrade()
    {
        SelectUpgradeFromMouse(0);
    }

    public void HoverLineUpgrade()
    {
        SelectUpgradeFromMouse(1);
    }

    public void HoverHookUpgrade()
    {
        SelectUpgradeFromMouse(2);
    }

    public void HoverHookSpeedUpgrade()
    {
        SelectUpgradeFromMouse(3);
    }

    public void HoverCoinBagUpgrade()
    {
        SelectUpgradeFromMouse(4);
    }

    public void HoverSkinsUpgrade()
    {
        SelectUpgradeFromMouse(5);
    }

    private void SelectUpgradeFromMouse(
        int index
    )
    {
        if (
            !IsOpen ||
            selectedNavigationIndex != 1 ||
            upgradeSelectionArrows == null ||
            index < 0 ||
            index >= upgradeSelectionArrows.Length
        )
        {
            return;
        }

        selectedUpgradeIndex = index;

        contentFocused = true;
        upgradeActionFocused = false;

        if (EventSystem.current)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }

        UpdateUpgradeSelection();
        UpdateUpgradeBuyButtonVisuals();
    }

    public void HoverUpgradeBuyButton()
    {
        FocusUpgradeBuyButton();
    }

    public void ExitUpgradeBuyButton()
    {
        if (
            selectedNavigationIndex != 1 ||
            !contentFocused
        )
        {
            return;
        }

        upgradeActionFocused = false;

        if (EventSystem.current)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }

        UpdateUpgradeSelection();
        UpdateUpgradeBuyButtonVisuals();
    }

    // =========================================================
    // CLOSE BUTTON
    // =========================================================

    public void HoverCloseButton()
    {
        if (!closeButton)
        {
            return;
        }

        closeButton.transform.localScale =
            closeButtonOriginalScale *
            closeButtonHoverScale;
    }

    public void ExitCloseButton()
    {
        if (!closeButton)
        {
            return;
        }

        if (closeButtonPressCoroutine != null)
        {
            return;
        }

        closeButton.transform.localScale =
            closeButtonOriginalScale;
    }

    // =========================================================
    // SELL BUTTON VISUALS
    // =========================================================

    private void UpdateSellActionButtonVisual()
    {
        bool perchSelected =
            selectedNavigationIndex == 0 &&
            contentFocused &&
            sellActionFocused &&
            selectedFishIndex == 0;

        bool catsharkSelected =
            selectedNavigationIndex == 0 &&
            contentFocused &&
            sellActionFocused &&
            selectedFishIndex == 1;

        bool crystalEelSelected =
            selectedNavigationIndex == 0 &&
            contentFocused &&
            sellActionFocused &&
            selectedFishIndex == 2;

        bool prismTroutSelected =
            selectedNavigationIndex == 0 &&
            contentFocused &&
            sellActionFocused &&
            selectedFishIndex == 3;

        if (sellActionButtonImage)
        {
            sellActionButtonImage.sprite = perchSelected
                    ? sellActionButtonSelected
                    : sellActionButtonNormal;
        }

        if (catsharkSellActionButtonImage)
        {
            catsharkSellActionButtonImage.sprite = catsharkSelected
                    ? sellActionButtonSelected
                    : sellActionButtonNormal;
        }

        if (crystalEelSellActionButtonImage)
        {
            crystalEelSellActionButtonImage.sprite =
                crystalEelSelected
                    ? sellActionButtonSelected
                    : sellActionButtonNormal;
        }

        if (prismTroutSellActionButtonImage)
        {
            prismTroutSellActionButtonImage.sprite =
                prismTroutSelected
                    ? sellActionButtonSelected
                    : sellActionButtonNormal;
        }
    }

    private void ResetSellActionButtonVisual()
    {
        if (sellActionButtonImage)
        {
            sellActionButtonImage.sprite =
                sellActionButtonNormal;
        }

        if (catsharkSellActionButtonImage)
        {
            catsharkSellActionButtonImage.sprite =
                sellActionButtonNormal;
        }

        if (crystalEelSellActionButtonImage)
        {
            crystalEelSellActionButtonImage.sprite =
                sellActionButtonNormal;
        }

        if (prismTroutSellActionButtonImage)
        {
            prismTroutSellActionButtonImage.sprite =
                sellActionButtonNormal;
        }
    }

    // =========================================================
    // VERKAUF
    // =========================================================

    public void HandleSellButtonPressed()
    {
        if (
            selectedNavigationIndex != 0 ||
            !fishInventory ||
            !coinWallet ||
            !IsSelectedFishAvailable()
        )
        {
            return;
        }

        Button pressedButton =
            GetSelectedSellButton();

        int sellValue;

        switch (selectedFishIndex)
        {
            case 0:
                sellValue = perchSellValue;
                break;

            case 1:
                sellValue = catsharkSellValue;
                break;

            case 2:
                sellValue = crystalEelSellValue;
                break;

            case 3:
                sellValue = prismTroutSellValue;
                break;

            default:
                return;
        }

        if (!coinWallet.CanAddCoins(sellValue))
        {
            Debug.Log(
                "Der Münzsack ist zu voll für diesen Verkauf.",
                this
            );

            return;
        }

        bool fishWasRemoved;

        if (selectedFishIndex == 0)
        {
            fishWasRemoved =
                fishInventory.TryRemovePerch();
        }
        else if (selectedFishIndex == 1)
        {
            fishWasRemoved =
                fishInventory.TryRemoveCatshark();
        }
        else if (selectedFishIndex == 2)
        {
            fishWasRemoved =
                fishInventory.TryRemoveCrystalEel();
        }
        else
        {
            fishWasRemoved =
                fishInventory.TryRemovePrismTrout();
        }

        if (!fishWasRemoved)
        {
            return;
        }

        coinWallet.AddCoins(sellValue);

        if (audioSource && coinSellClip)
        {
            audioSource.PlayOneShot(coinSellClip);
        }

        UpdateSellerCoinDisplay();

        if (sellButtonPressCoroutine != null)
        {
            StopCoroutine(
                sellButtonPressCoroutine
            );
        }

        if (pressedButton)
        {
            sellButtonPressCoroutine =
                StartCoroutine(
                    PlaySellButtonPress(
                        pressedButton
                    )
                );
        }
    }

    private IEnumerator PlaySellButtonPress(
        Button button
    )
    {
        if (!button)
        {
            yield break;
        }

        Vector3 originalScale;

        if (button == sellContentButton)
        {
            originalScale =
                perchSellButtonOriginalScale;
        }
        else if (
            button == catsharkSellContentButton
        )
        {
            originalScale =
                catsharkSellButtonOriginalScale;
        }
        else if (
            button == crystalEelSellContentButton
        )
        {
            originalScale =
                crystalEelSellButtonOriginalScale;
        }
        else if (
            button == prismTroutSellContentButton
        )
        {
            originalScale =
                prismTroutSellButtonOriginalScale;
        }
        else
        {
            yield break;
        }

        button.transform.localScale =
            originalScale *
            pressedScale;

        yield return new WaitForSecondsRealtime(
            pressDuration
        );

        if (button)
        {
            button.transform.localScale =
                originalScale;
        }

        sellButtonPressCoroutine = null;
    }

    // =========================================================
    // UPGRADE BUTTON
    // =========================================================

    public void HandleUpgradeBuyButtonPressed()
    {
        if (
            selectedNavigationIndex != 1 ||
            !upgradeActionFocused
        )
        {
            return;
        }

        if (
            upgradeBuyButtons == null ||
            selectedUpgradeIndex < 0 ||
            selectedUpgradeIndex >=
            upgradeBuyButtons.Length
        )
        {
            return;
        }

        Button button =
            upgradeBuyButtons[
                selectedUpgradeIndex
            ];

        if (!button || !button.interactable)
        {
            return;
        }

        if (
            upgradeBuyButtonPressCoroutine != null
        )
        {
            StopCoroutine(
                upgradeBuyButtonPressCoroutine
            );
        }

        upgradeBuyButtonPressCoroutine =
            StartCoroutine(
                PlayUpgradeBuyButtonPress()
            );

        if (selectedUpgradeIndex == 0)
        {
            TryBuyInventoryUpgrade();
        }
        else if (selectedUpgradeIndex == 1)
        {
            TryBuyLineUpgrade();
        }
        else if (selectedUpgradeIndex == 2)
        {
            TryBuyHookUpgrade();
        }
        else if (selectedUpgradeIndex == 3)
        {
            TryBuyHookSpeedUpgrade();
        }
        else if (selectedUpgradeIndex == 4)
        {
            TryBuyCoinBagUpgrade();
        }
        else if (selectedUpgradeIndex == 5)
        {
            TryBuyPurpleSkin();
        }
    }

    private IEnumerator PlayUpgradeBuyButtonPress()
    {
        if (
            upgradeBuyButtons == null ||
            selectedUpgradeIndex < 0 ||
            selectedUpgradeIndex >=
            upgradeBuyButtons.Length
        )
        {
            yield break;
        }

        Button button =
            upgradeBuyButtons[
                selectedUpgradeIndex
            ];

        if (!button)
        {
            yield break;
        }

        Vector3 originalScale =
            button.transform.localScale;

        button.transform.localScale =
            originalScale *
            pressedScale;

        yield return new WaitForSecondsRealtime(
            pressDuration
        );

        if (button)
        {
            button.transform.localScale =
                originalScale;
        }

        upgradeBuyButtonPressCoroutine = null;
    }

    // =========================================================
    // CLOSE BUTTON PRESS
    // =========================================================

    public void HandleCloseButtonPressed()
    {
        if (
            closeButtonPressCoroutine != null
        )
        {
            StopCoroutine(
                closeButtonPressCoroutine
            );
        }

        closeButtonPressCoroutine =
            StartCoroutine(
                PlayCloseButtonPress()
            );
    }

    private IEnumerator PlayCloseButtonPress()
    {
        if (!closeButton)
        {
            yield break;
        }

        closeButton.transform.localScale =
            closeButtonOriginalScale *
            closeButtonPressedScale;

        yield return new WaitForSecondsRealtime(
            closeButtonPressDuration
        );

        closeButton.transform.localScale =
            closeButtonOriginalScale;

        closeButtonPressCoroutine = null;

        CloseMenu();
    }

    // =========================================================
    // RESET
    // =========================================================

    private void ResetSellButtonScale()
    {
        if (sellButtonPressCoroutine != null)
        {
            StopCoroutine(
                sellButtonPressCoroutine
            );

            sellButtonPressCoroutine = null;
        }

        if (sellContentButton)
        {
            sellContentButton.transform.localScale =
                perchSellButtonOriginalScale;
        }

        if (catsharkSellContentButton)
        {
            catsharkSellContentButton.transform.localScale =
                catsharkSellButtonOriginalScale;
        }

        if (crystalEelSellContentButton)
        {
            crystalEelSellContentButton.transform.localScale =
                crystalEelSellButtonOriginalScale;
        }

        if (prismTroutSellContentButton)
        {
            prismTroutSellContentButton.transform.localScale =
                prismTroutSellButtonOriginalScale;
        }
    }

    private void ResetUpgradeBuyButtonScale()
    {
        if (
            upgradeBuyButtonPressCoroutine != null
        )
        {
            StopCoroutine(
                upgradeBuyButtonPressCoroutine
            );

            upgradeBuyButtonPressCoroutine = null;
        }
    }

    private void ResetCloseButtonScale()
    {
        if (!closeButton)
        {
            return;
        }

        if (closeButtonPressCoroutine != null)
        {
            StopCoroutine(
                closeButtonPressCoroutine
            );

            closeButtonPressCoroutine = null;
        }

        closeButton.transform.localScale =
            closeButtonOriginalScale;
    }
}