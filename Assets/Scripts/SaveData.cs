using System;

[Serializable]
public class SaveData
{
    // Fortschritt
    public bool sellerUnlocked;
    // Münzen
    public int coins;
    public int maximumCoins;

    // Inventar
    public int maximumFishCount;

    public int perchCount;
    public int catsharkCount;
    public int crystalEelCount;
    public int prismTroutCount;

    // Upgrades
    public int hookLevel;
    public int lineLevel;
    public int hookSpeedLevel;
    public int coinBagLevel;

    // Skin
    public bool purpleSkinPurchased;
    public bool usingPurpleSkin;

    // Sammlung
    public int perchCaughtCount;
    public int catsharkCaughtCount;
    public int crystalEelCaughtCount;
    public int prismTroutCaughtCount;

    // Seller-Dialoge
    public bool introductionCompleted;
    public bool catsharkDialogueCompleted;
    public bool crystalEelDialogueCompleted;
    public bool prismTroutDialogueCompleted;
    public bool hookUpgradeDialogueCompleted;
    public bool lineUpgradeDialogueCompleted;
    public bool purpleSkinDialogueCompleted;
}