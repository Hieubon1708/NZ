using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameController;

public class UIBoss : MonoBehaviour
{
    public static UIBoss instance;

    public int level;
    public int progress;
    public BossHandler bossHandler;
    public TextMeshProUGUI textStart;
    public TextMeshProUGUI textTitle;
    public TextMeshProUGUI textProgress;
    public GameObject buttonBack;
    public GameObject progressBoss;
    public Image progressFill;

    public TextMeshProUGUI titlePanel;
    public TextMeshProUGUI key;
    public TextMeshProUGUI textProgressInPanel;
    public Image progressFillInPanel;

    private void Awake()
    {
        instance = this;
    }

    void ActiveMode(bool isActive)
    {
        GameController.instance.boss.SetActive(isActive);
        buttonBack.SetActive(isActive);
        textTitle.gameObject.SetActive(isActive);
        UIHandler.instance.menu.battleGameplay.SetActive(isActive);
        UIHandler.instance.menu.battle.SetActive(isActive);
        UIHandler.instance.menu.battleWorld.SetActive(isActive);
        UIHandler.instance.daily.daily.SetActive(!isActive);
        UIHandler.instance.mapInfo.transform.parent.gameObject.SetActive(!isActive);
        UIHandler.instance.gem.SetActive(!isActive);
        UIHandler.instance.menu.gameObject.SetActive(!isActive);
        progressBoss.SetActive(isActive);
        GameController.instance.main.SetActive(!isActive);
        UIHandler.instance.menu.boss.SetActive(!isActive);
        textStart.text = isActive ? "Boss" : "Start";
    }

    public void ActiveButtonBack(bool isActive)
    {
        buttonBack.SetActive(isActive);
    }

    public void LoadData()
    {
        if (DataManager.instance.dataStorage.bossDataStorage != null)
        {
            level = DataManager.instance.dataStorage.bossDataStorage.level;
            progress = DataManager.instance.dataStorage.bossDataStorage.progress;
        }
        else
        {
            DataManager.instance.dataStorage.bossDataStorage = new BossDataStorage(0, 0, 0, new DateTime(), 500, 0, null, null);
        }
        UpdatePanel();
        CheckNotif();
    }

    public void SetUp()
    {
        BossDataStorage dataStorage = DataManager.instance.dataStorage.bossDataStorage;
        if (dataStorage != null)
        {
            if (dataStorage.blockDataStorage != null)
            {
                for (int i = 0; i < dataStorage.blockDataStorage.Length; i++)
                {
                    GameObject block = BlockController.instance.blockPools[0];
                    BlockController.instance.blockPools.Remove(block);
                    block.transform.localPosition = new Vector2(block.transform.localPosition.x, BlockController.instance.startY + BlockController.instance.distance * BlockController.instance.blocks.Count);
                    BlockController.instance.blocks.Add(block);
                    block.SetActive(true);
                    Block scBlock = BlockController.instance.GetScBlock(block);
                    scBlock.sellingPrice = dataStorage.blockDataStorage[i].sellingPrice;

                    BlockUpgradeHandler blockUpgradeHandler = BlockController.instance.GetScBlock(block).blockUpgradeHandler;

                    int blockLevel = dataStorage.blockDataStorage[i].level;
                    WEAPON weaponType = dataStorage.blockDataStorage[i].weaponDataStorage.weaponType;
                    int weaponLevel = dataStorage.blockDataStorage[i].weaponDataStorage.weaponLevel;
                    int weaponLevelUpgrade = dataStorage.blockDataStorage[i].weaponDataStorage.weaponLevelUpgrade;

                    blockUpgradeHandler.LoadData(blockLevel, weaponType, weaponLevel, weaponLevelUpgrade);
                    if (blockUpgradeHandler.weaponUpgradeHandler.weaponShoter != null) blockUpgradeHandler.weaponUpgradeHandler.weaponShoter.LoadData();
                }
            }
        }
        BlockController.instance.player.transform.localPosition = new Vector2(BlockController.instance.player.transform.localPosition.x, BlockController.instance.startYPlayer + BlockController.instance.distance * BlockController.instance.blocks.Count);

        UIHandler.instance.lastRewardTime = DataManager.instance.dataStorage.bossDataStorage.lastRewardTime;
        UIHandler.instance.goldRewardHighest = DataManager.instance.dataStorage.bossDataStorage.goldRewardHighest;
        BlockController.instance.energyUpgradee.level = DataManager.instance.dataStorage.bossDataStorage.levelEnergy;
        PlayerController.instance.player.gold = DataManager.instance.dataStorage.bossDataStorage.gold;

        UpgradeEvolutionController.instance.SetData(DataManager.instance.dataStorage.bossDataStorage.weaponEvolutionDataStorge);
        UIHandler.instance.GoldUpdatee();
        BlockController.instance.energyUpgradee.UpgradeHandle();
        UIHandler.instance.CheckRewardGold();
        BlockController.instance.CheckButtonStateAll();
        textTitle.text = DataManager.instance.bossConfig.title[level];
    }

    public void CheckNotif()
    {
        if (EquipmentController.instance.playerInventory.key < DataManager.instance.bossConfig.key[level]) UIHandler.instance.menu.notifOptions[2].SetActive(false);
        else UIHandler.instance.menu.notifOptions[2].SetActive(true);
    }

    public void UpdatePanel()
    {
        progressFillInPanel.transform.localScale = new Vector3((float)progress / DataManager.instance.bossConfig.targetProgress[level], 1, 1);
        key.text = EquipmentController.instance.playerInventory.key + "/" + DataManager.instance.bossConfig.key[level];
        titlePanel.text = DataManager.instance.bossConfig.titleInPanel[level];
        textProgressInPanel.text = UIHandler.instance.ConvertNumberAbbreviation(progress) + "/" + UIHandler.instance.ConvertNumberAbbreviation(DataManager.instance.bossConfig.targetProgress[level]);
    }

    public BossDataStorage GetData()
    {
        return new BossDataStorage(level
            , DataManager.instance.dataStorage.bossDataStorage.gold
            , DataManager.instance.dataStorage.bossDataStorage.levelEnergy
            , DataManager.instance.dataStorage.bossDataStorage.lastRewardTime
            , DataManager.instance.dataStorage.bossDataStorage.goldRewardHighest
            , progress
            , DataManager.instance.dataStorage.bossDataStorage.blockDataStorage
            , DataManager.instance.dataStorage.bossDataStorage.weaponEvolutionDataStorge);
    }

    public void UpdateFillProgress()
    {
        progressFill.fillAmount = (float)progress / DataManager.instance.bossConfig.targetProgress[level];
    }

    public void UpdateTextInProgress()
    {
        textProgress.text = UIHandler.instance.ConvertNumberAbbreviation(progress) + "/" + UIHandler.instance.ConvertNumberAbbreviation(DataManager.instance.bossConfig.targetProgress[level]);
    }

    public void Play()
    {
        //if (EquipmentController.instance.playerInventory.key < DataManager.instance.bossConfig.key[level]) return;
        BlockController.instance.LoadWeaponBuyButtonInCurrentLevel(level);
        Booster.instance.StartGame();
        GameController.instance.ChangeBlockSprites(level);
        GameController.instance.ChangeCarSprites(level);
        GameController.instance.isPLayBoss = true;
        UIHandler.instance.tutorial.TutorialButtonPlayBoss(true);
        BlockController.instance.SaveData();
        BlockController.instance.ClearBlocks();
        SetUp();
        bossHandler.LoadData();
        ActiveMode(true);
    }

    public void Exit()
    {
        Booster.instance.StartGame();
        BlockController.instance.LoadWeaponBuyButtonInCurrentLevel(GameController.instance.level);
        GameController.instance.ChangeBlockSprites(GameController.instance.level);
        GameController.instance.ChangeCarSprites(GameController.instance.level);
        GameController.instance.isPLayBoss = false;
        UpdatePanel();
        SaveData();
        BlockController.instance.ClearBlocks();

        PlayerController.instance.player.gold = DataManager.instance.dataStorage.playerDataStorage.gold;
        BlockController.instance.energyUpgradee.level = DataManager.instance.dataStorage.energyDataStorage.level;
        UIHandler.instance.lastRewardTime = DataManager.instance.dataStorage.lastRewardTime;
        UIHandler.instance.goldRewardHighest = DataManager.instance.dataStorage.goldRewardHighest;

        UIHandler.instance.GoldUpdatee();
        UIHandler.instance.CheckRewardGold();
        BlockController.instance.LoadData();

        UpgradeEvolutionController.instance.SetData(DataManager.instance.dataStorage.weaponEvolutionDataStorge);
        ActiveMode(false);
    }

    public void SaveData()
    {
        DataManager.instance.dataStorage.bossDataStorage.blockDataStorage = BlockController.instance.GetBlocks();
        DataManager.instance.dataStorage.bossDataStorage.levelEnergy = BlockController.instance.energyUpgradee.level;
        DataManager.instance.dataStorage.bossDataStorage.gold = PlayerController.instance.player.gold;
        DataManager.instance.dataStorage.bossDataStorage.goldRewardHighest = UIHandler.instance.goldRewardHighest;
        DataManager.instance.dataStorage.bossDataStorage.lastRewardTime = UIHandler.instance.lastRewardTime;
    }
}
