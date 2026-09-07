using UnityEngine;

public class GameSaveController : MonoBehaviour
{
    [Header("Save-Systeme")]
    [SerializeField] private CoinWallet coinWallet;
    [SerializeField] private FishInventory fishInventory;
    [SerializeField] private FishingUpgradeState fishingUpgradeState;
    [SerializeField] private FishCollection fishCollection;
    [SerializeField] private PlayerSkinController playerSkinController;

    [Header("Fortschritt")]
    [SerializeField] private GameObject sellerObject;
    [SerializeField] private SellerInteraction sellerInteraction;

    private void Start()
    {
        if (SaveManager.LoadGameRequested)
        {
            LoadCurrentGame();
        }
        else if (SaveManager.StartNewGameRequested)
        {
            SaveCurrentGame();
        }

        SaveManager.ClearSceneRequest();
    }
    public void SaveCurrentGame()
    {
        if (
            !coinWallet ||
            !fishInventory ||
            !fishingUpgradeState ||
            !fishCollection ||
            !playerSkinController
        )
        {
            Debug.LogError(
                "GameSaveController: Es fehlen Referenzen.",
                this
            );

            return;
        }

        SaveData data =
            new SaveData();

        data.coins =
            coinWallet.Coins;

        data.maximumCoins =
            coinWallet.MaximumCoins;

        data.maximumFishCount =
            fishInventory.MaximumFishCount;

        data.perchCount =
            fishInventory.PerchCount;

        data.catsharkCount =
            fishInventory.CatsharkCount;

        data.crystalEelCount =
            fishInventory.CrystalEelCount;

        data.prismTroutCount =
            fishInventory.PrismTroutCount;

        data.hookLevel =
            fishingUpgradeState.HookLevel;

        data.lineLevel =
            fishingUpgradeState.LineLevel;

        data.hookSpeedLevel =
            fishingUpgradeState.HookSpeedLevel;

        data.coinBagLevel =
            fishingUpgradeState.CoinBagLevel;

        data.purpleSkinPurchased =
            fishingUpgradeState.PurpleSkinPurchased;

        data.usingPurpleSkin =
            playerSkinController.UsingPurpleSkin;

        data.perchCaughtCount =
            fishCollection.PerchCaughtCount;

        data.catsharkCaughtCount =
            fishCollection.CatsharkCaughtCount;

        data.crystalEelCaughtCount =
            fishCollection.CrystalEelCaughtCount;

        data.prismTroutCaughtCount =
            fishCollection.PrismTroutCaughtCount;

        data.sellerUnlocked =
            sellerObject != null &&
            sellerObject.activeSelf;

        data.introductionCompleted =
            sellerInteraction.IntroductionCompleted;

        data.catsharkDialogueCompleted =
            sellerInteraction.CatsharkDialogueCompleted;

        data.crystalEelDialogueCompleted =
            sellerInteraction.CrystalEelDialogueCompleted;

        data.prismTroutDialogueCompleted =
            sellerInteraction.PrismTroutDialogueCompleted;

        data.hookUpgradeDialogueCompleted =
            sellerInteraction.HookUpgradeDialogueCompleted;

        data.lineUpgradeDialogueCompleted =
            sellerInteraction.LineUpgradeDialogueCompleted;

        data.purpleSkinDialogueCompleted =
            sellerInteraction.PurpleSkinDialogueCompleted;

        SaveManager.SaveGame(data);
    }

    private void OnApplicationQuit()
    {
        SaveCurrentGame();
    }

    public void LoadCurrentGame()
    {
        SaveData data =
            SaveManager.LoadGame();

        if (data == null)
        {
            Debug.LogWarning(
                "Kein Spielstand vorhanden.",
                this
            );

            return;
        }

        fishingUpgradeState.SetUpgradeState(
            data.hookLevel,
            data.lineLevel,
            data.hookSpeedLevel,
            data.coinBagLevel,
            data.purpleSkinPurchased
        );

        coinWallet.SetMaximumCoins(
            data.maximumCoins
        );

        coinWallet.SetCoins(
            data.coins
        );

        fishInventory.SetInventoryState(
            data.maximumFishCount,
            data.perchCount,
            data.catsharkCount,
            data.crystalEelCount,
            data.prismTroutCount
        );

        fishingUpgradeState.SetUpgradeState(
            data.hookLevel,
            data.lineLevel,
            data.hookSpeedLevel,
            data.coinBagLevel,
            data.purpleSkinPurchased
        );

        fishCollection.SetCollectionState(
            data.perchCaughtCount,
            data.catsharkCaughtCount,
            data.crystalEelCaughtCount,
            data.prismTroutCaughtCount
        );

        if (
            data.usingPurpleSkin &&
            data.purpleSkinPurchased
        )
        {
            playerSkinController
                .UsePurpleSkin();
        }
        else
        {
            playerSkinController
                .UseDefaultSkin();
        }

        if (sellerObject)
        {
            sellerObject.SetActive(
                data.sellerUnlocked
            );
        }

        sellerInteraction.SetDialogueState(
            data.introductionCompleted,
            data.catsharkDialogueCompleted,
            data.crystalEelDialogueCompleted,
            data.prismTroutDialogueCompleted,
            data.hookUpgradeDialogueCompleted,
            data.lineUpgradeDialogueCompleted,
            data.purpleSkinDialogueCompleted
        );

        Debug.Log(
            "Spielstand geladen.",
            this
        );
    }
}