﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.U2D;
using static GameController;
using static UpgradeEvolutionController;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    Sprite[] blockSpritesLv1 = new Sprite[5];
    Sprite[] blockSpritesLv2 = new Sprite[5];

    Sprite[] carSpritesLv1 = new Sprite[6];
    Sprite[] carSpritesLv2 = new Sprite[6];

    public Sprite[] blockSprites;

    public DataStorage dataStorage;
    public DailyDataStorage dailyDataStorage;
    public TutorialDataStorage tutorialDataStorage;
    public PlayerDataStorage playerDataStorage;
    public BlockDataStorage[] blockDataStorage;
    public EnergyDataStorage energyDataStorage;
    public ChanceDataStorage chanceDataStorage;
    public WeaponEvolutionDataStorge weaponEvolutionDataStorge;
    public BossDataStorage bossDataStorage;

    public BlockConfig blockConfig;
    public BossConfig bossConfig;
    public EnergyConfig energyConfig;
    public ChanceConfig chanceConfig;
    public RewardConfig[] rewardConfigs;
    public DailyConfig[] dailyConfigs;
    public EquipmentConfig equipmentConfig;
    public WeaponConfig[] weaponConfigs;

    public SpriteAtlas[] spriteAtlas;

    public void Awake()
    {
        instance = this;
        // GenerateWeaponConfigs();
        LoadSprites();
        DataReader();
    }

    void LoadSprites()
    {
        LoadBlockNCarSprites(blockSpritesLv1, carSpritesLv1, spriteAtlas[0]);
        LoadBlockNCarSprites(blockSpritesLv2, carSpritesLv2, spriteAtlas[1]);
    }

    void LoadBlockNCarSprites(Sprite[] blockSps, Sprite[] carSps, SpriteAtlas atlas)
    {
        for (int i = 0; i < blockSps.Length; i++)
        {
            blockSps[i] = atlas.GetSprite("Tower_Block_" + (i + 1));
        }
        carSps[0] = atlas.GetSprite("Umbrella_Pannel");
        carSps[1] = atlas.GetSprite("Umbrella_Pole_2");
        carSps[2] = atlas.GetSprite("Umbrella_Pole_1");
        carSps[3] = atlas.GetSprite("Tower_Chassis");
        carSps[4] = atlas.GetSprite("Tower_Wheel_1");
        carSps[5] = atlas.GetSprite("Tower_Shadow");
    }

    public void SetBlockSprites(int level)
    {
        if (level == 0) blockSprites = blockSpritesLv1;
        if (level == 1) blockSprites = blockSpritesLv2;
    }

    public Sprite[] GetCarSprites(int level)
    {
        if (level == 0) return carSpritesLv1;
        if (level == 1) return carSpritesLv2;

        return null;
    }

    void DataReader()
    {
        TextAsset blockConfigJs = Resources.Load<TextAsset>("Datas/BlockConfig");
        TextAsset energyConfigJs = Resources.Load<TextAsset>("Datas/EnergyConfig");
        TextAsset weaponConfigJs = Resources.Load<TextAsset>("Datas/WeaponConfig");
        TextAsset chanceConfigJs = Resources.Load<TextAsset>("Datas/ChanceConfig");
        TextAsset equipmentConfigJs = Resources.Load<TextAsset>("Datas/EquipmentConfig");
        TextAsset rewardConfigJs = Resources.Load<TextAsset>("Datas/RewardConfig");
        TextAsset dailyConfigJs = Resources.Load<TextAsset>("Datas/DailyConfig");
        TextAsset bossConfigJs = Resources.Load<TextAsset>("Datas/BossConfig");

        blockConfig = JsonConvert.DeserializeObject<BlockConfig>(blockConfigJs.text);
        bossConfig = JsonConvert.DeserializeObject<BossConfig>(bossConfigJs.text);
        energyConfig = JsonConvert.DeserializeObject<EnergyConfig>(energyConfigJs.text);
        chanceConfig = JsonConvert.DeserializeObject<ChanceConfig>(chanceConfigJs.text);
        equipmentConfig = JsonConvert.DeserializeObject<EquipmentConfig>(equipmentConfigJs.text);
        weaponConfigs = JsonConvert.DeserializeObject<WeaponConfig[]>(weaponConfigJs.text);
        rewardConfigs = JsonConvert.DeserializeObject<RewardConfig[]>(rewardConfigJs.text);
        dailyConfigs = JsonConvert.DeserializeObject<DailyConfig[]>(dailyConfigJs.text);

        string dataJs = Path.Combine(Application.persistentDataPath, "DataStorage.json");
        if (File.Exists(dataJs))
        {
            string DataStorageContent = File.ReadAllText(dataJs);
            dataStorage = JsonConvert.DeserializeObject<DataStorage>(DataStorageContent);
        }
        string dailyJs = Path.Combine(Application.persistentDataPath, "DailyDataStorage.json");
        if (File.Exists(dailyJs))
        {
            string js = File.ReadAllText(dailyJs);
            dailyDataStorage = JsonConvert.DeserializeObject<DailyDataStorage>(js);
        }
        string chanceJs = Path.Combine(Application.persistentDataPath, "ChanceDataStorage.json");
        if (File.Exists(chanceJs))
        {
            string js = File.ReadAllText(chanceJs);
            chanceDataStorage = JsonConvert.DeserializeObject<ChanceDataStorage>(js);
        }
        string evoJs = Path.Combine(Application.persistentDataPath, "EvoDataStorage.json");
        if (File.Exists(evoJs))
        {
            string js = File.ReadAllText(evoJs);
            weaponEvolutionDataStorge = JsonConvert.DeserializeObject<WeaponEvolutionDataStorge>(js);
        }
        string playerJs = Path.Combine(Application.persistentDataPath, "PlayerDataStorage.json");
        if (File.Exists(playerJs))
        {
            string js = File.ReadAllText(playerJs);
            playerDataStorage = JsonConvert.DeserializeObject<PlayerDataStorage>(js);
        }
        string tutorialJs = Path.Combine(Application.persistentDataPath, "TutorialDataStorage.json");
        if (File.Exists(tutorialJs))
        {
            string js = File.ReadAllText(tutorialJs);
            tutorialDataStorage = JsonConvert.DeserializeObject<TutorialDataStorage>(js);
        }
        string energyJs = Path.Combine(Application.persistentDataPath, "EnergyDataStorage.json");
        if (File.Exists(energyJs))
        {
            string js = File.ReadAllText(energyJs);
            energyDataStorage = JsonConvert.DeserializeObject<EnergyDataStorage>(js);
        }
        string blockJs = Path.Combine(Application.persistentDataPath, "BlockDataStorage.json");
        if (File.Exists(blockJs))
        {
            string js = File.ReadAllText(blockJs);
            blockDataStorage = JsonConvert.DeserializeObject<BlockDataStorage[]>(js);
        }
    }

    public WeaponConfig FindWeaponConfigByType(WEAPON type)
    {
        for (int i = 0; i < weaponConfigs.Length; i++)
        {
            if (weaponConfigs[i].weaponType == type)
            {
                return weaponConfigs[i];
            }
        }
        return null;
    }

    public int GetUpgradePriceWeaponConfig(int level, int levelUpgrade, WeaponConfig weaponConfig)
    {
        PriceConfig priceConfig = weaponConfig.weaponLevelConfigs[level].priceConfig;
        return Mathf.RoundToInt(priceConfig.startPrice * Mathf.Pow(priceConfig.upgradeCoef, levelUpgrade));
    }

    public int GetEvolutionPriceWeaponConfig(int level, WeaponConfig weaponConfig)
    {
        return weaponConfig.weaponLevelConfigs[level].priceConfig.evolutionPrice;
    }

    public int GetPriceWeaponConfig(WEAPON weaponType)
    {
        WeaponConfig weaponConfig = FindWeaponConfigByType(weaponType);
        return weaponConfig != null ? weaponConfig.price : 0;
    }

    public int GetLengthUpgradePriceWeaponConfig(int level, WeaponConfig weaponConfig)
    {
        return weaponConfig.weaponLevelConfigs.Length;
    }

    public int GetDamageWeaponConfig(int level, int levelUpgrade, WeaponConfig weaponConfig)
    {
        AttackConfig attackConfig = weaponConfig.weaponLevelConfigs[level].attackConfig;
        return Mathf.RoundToInt(attackConfig.startAttackPower * Mathf.Pow(attackConfig.upgradeCoef, levelUpgrade));
    }

    public int GetDamageBoosterWeaponConfig(int level, WeaponConfig weaponConfig)
    {
        return weaponConfig.weaponLevelConfigs[level].attackConfig.damageBooster;
    }

    public int GetPriceUpgradeEnergyConfig(int level)
    {
        return Mathf.RoundToInt(energyConfig.startPrice * Mathf.Pow(energyConfig.upgradePriceCoef, level));
    }

    public float GetSecondsUpgradeEnergyConfig(int level)
    {
        return energyConfig.startSeconds + (energyConfig.upgradeSecondsCoef * level);
    }

    void GenerateWeaponConfigs()
    {
        WeaponConfig[] weaponConfigs = new WeaponConfig[4];

        WeaponLevelConfig[] sawLevelConfigs = new WeaponLevelConfig[7];

        sawLevelConfigs[0] = SetWeaponLevelConfig(200, 250, 1.03f, 600, 7, 0.2f, 1.5f, 1.11f, 2, 1);
        sawLevelConfigs[1] = SetWeaponLevelConfig(310, 400, 1.03f, 1286, 15, 0.2f, 1.5f, 1.11f, 2, 1);
        sawLevelConfigs[2] = SetWeaponLevelConfig(481, 600, 1.03f, 2914, 34, 0.2f, 1.5f, 1.11f, 2, 1);
        sawLevelConfigs[3] = SetWeaponLevelConfig(746, 900, 1.03f, 6429, 75, 0.2f, 1.5f, 1.11f, 2, 1);
        sawLevelConfigs[4] = SetWeaponLevelConfig(1156, 1400, 1.03f, 14057, 164, 0.2f, 1.5f, 1.11f, 2, 1);
        sawLevelConfigs[5] = SetWeaponLevelConfig(1792, 2250, 1.03f, 30943, 361, 0.2f, 1.5f, 1.11f, 2, 1);
        sawLevelConfigs[6] = SetWeaponLevelConfig(2778, 0, 1.03f, 68057, 794, 0.2f, 1.5f, 1.11f, 2, 1);

        weaponConfigs[0] = new WeaponConfig(WEAPON.SAW, 90, 2, sawLevelConfigs);

        WeaponLevelConfig[] flameLevelConfigs = new WeaponLevelConfig[7];

        flameLevelConfigs[0] = SetWeaponLevelConfig(400, 500, 1.03f, 50, 9, 0.2f, 1.5f, 1.11f, 2, 1);
        flameLevelConfigs[1] = SetWeaponLevelConfig(564, 700, 1.03f, 111, 20, 0.2f, 1.5f, 1.11f, 2, 1);
        flameLevelConfigs[2] = SetWeaponLevelConfig(795, 1000, 1.03f, 256, 46, 0.2f, 1.5f, 1.11f, 2, 1);
        flameLevelConfigs[3] = SetWeaponLevelConfig(1121, 1500, 1.03f, 572, 103, 0.2f, 1.5f, 1.11f, 2, 1);
        flameLevelConfigs[4] = SetWeaponLevelConfig(1581, 2000, 1.03f, 1283, 231, 0.2f, 1.5f, 1.11f, 2, 1);
        flameLevelConfigs[5] = SetWeaponLevelConfig(2229, 3000, 1.03f, 2884, 519, 0.2f, 1.5f, 1.11f, 2, 1);
        flameLevelConfigs[6] = SetWeaponLevelConfig(3143, 0, 1.03f, 6489, 1168, 0.2f, 1.5f, 1.11f, 2, 1);

        weaponConfigs[1] = new WeaponConfig(WEAPON.FLAME, 360, 2, flameLevelConfigs);

        WeaponLevelConfig[] machineGunLevelConfigs = new WeaponLevelConfig[7];

        machineGunLevelConfigs[0] = SetWeaponLevelConfig(500, 600, 1.03f, 300, 40, 0.2f, 1.5f, 1.11f, 2, 1);
        machineGunLevelConfigs[1] = SetWeaponLevelConfig(700, 800, 1.03f, 705, 94, 0.2f, 1.5f, 1.11f, 2, 1);
        machineGunLevelConfigs[2] = SetWeaponLevelConfig(980, 1200, 1.03f, 1658, 221, 0.2f, 1.5f, 1.11f, 2, 1);
        machineGunLevelConfigs[3] = SetWeaponLevelConfig(1372, 1800, 1.03f, 3892, 519, 0.2f, 1.5f, 1.11f, 2, 1);
        machineGunLevelConfigs[4] = SetWeaponLevelConfig(1921, 2250, 1.03f, 9150, 1220, 0.2f, 1.5f, 1.11f, 2, 1);
        machineGunLevelConfigs[5] = SetWeaponLevelConfig(2689, 3250, 1.03f, 21502, 2867, 0.2f, 1.5f, 1.11f, 2, 1);
        machineGunLevelConfigs[6] = SetWeaponLevelConfig(3765, 0, 1.03f, 50528, 6737, 0.2f, 1.5f, 1.11f, 2, 1);

        weaponConfigs[2] = new WeaponConfig(WEAPON.MACHINE_GUN, 750, 2, machineGunLevelConfigs);

        WeaponLevelConfig[] shockerLevelConfigs = new WeaponLevelConfig[7];

        shockerLevelConfigs[0] = SetWeaponLevelConfig(200, 250, 1.03f, 600, 7, 0.2f, 1.5f, 1.11f, 2, 1);
        shockerLevelConfigs[1] = SetWeaponLevelConfig(310, 400, 1.03f, 1286, 15, 0.2f, 1.5f, 1.11f, 2, 1);
        shockerLevelConfigs[2] = SetWeaponLevelConfig(481, 600, 1.03f, 2914, 34, 0.2f, 1.5f, 1.11f, 2, 1);
        shockerLevelConfigs[3] = SetWeaponLevelConfig(746, 900, 1.03f, 6429, 75, 0.2f, 1.5f, 1.11f, 2, 1);
        shockerLevelConfigs[4] = SetWeaponLevelConfig(1156, 1400, 1.03f, 14057, 164, 0.2f, 1.5f, 1.11f, 2, 1);
        shockerLevelConfigs[5] = SetWeaponLevelConfig(1792, 2250, 1.03f, 30943, 361, 0.2f, 1.5f, 1.11f, 2, 1);
        shockerLevelConfigs[6] = SetWeaponLevelConfig(2778, 0, 1.03f, 68057, 794, 0.2f, 1.5f, 1.11f, 2, 1);

        weaponConfigs[3] = new WeaponConfig(WEAPON.SHOCKER, 90, 2, shockerLevelConfigs);

        string js = JsonConvert.SerializeObject(weaponConfigs);
        string path = Path.Combine(Application.dataPath, "Resources/Datas/WeaponConfig.json");
        File.WriteAllText(path, js);
    }

    WeaponLevelConfig SetWeaponLevelConfig(int startPrice, int evolutionPrice, float upgradeCoefPrice
        , int damageBooster, int startAttackPower, float critChance, float critDamageScale, float upgradeCoefAttack, float attackTime, float reloadTime)
    {
        PriceConfig priceConfig = new PriceConfig(startPrice, evolutionPrice, upgradeCoefPrice);
        AttackConfig attackConfig = new AttackConfig(damageBooster, startAttackPower, critChance, critDamageScale, upgradeCoefAttack, attackTime, reloadTime);
        return new WeaponLevelConfig(priceConfig, attackConfig);
    }

    public void SaveFile(string dataJs, string path)
    {
        File.WriteAllText(Path.Combine(Application.persistentDataPath, path), dataJs);
    }

    public void SaveData()
    {
        DataStorage dataStorage = new DataStorage(GameController.instance.level
            , this.dataStorage.isSoundActive
            , this.dataStorage.isMusicActive
            , UIHandler.instance.lastRewardTime, UIHandler.instance.goldRewardHighest
            , UIHandler.instance.progressHandler.progresses.ToArray());
        SaveFile(JsonConvert.SerializeObject(dataStorage), "DataStorage.json");
    }
    
    public void SaveDaily()
    {
        DailyDataStorage dailyDataStorage = UIHandler.instance.daily.GetData();
        SaveFile(JsonConvert.SerializeObject(dailyDataStorage), "DailyDataStorage.json");
    }
    
    public void SaveChance()
    {
        ChanceDataStorage chanceDataStorage = new ChanceDataStorage(UIHandler.instance.summonEquipment.level, UIHandler.instance.summonEquipment.amout);
        SaveFile(JsonConvert.SerializeObject(chanceDataStorage), "ChanceDataStorage.json");
    }
    
    public void SaveTutorial()
    {
        TutorialDataStorage tutorialDataStorage = UIHandler.instance.tutorial.GetData();
        SaveFile(JsonConvert.SerializeObject(tutorialDataStorage), "TutorialDataStorage.json");
    }
    
    public void SaveEnergy()
    {
        EnergyDataStorage energyDataStorage = new EnergyDataStorage(BlockController.instance.energyUpgradee.level);
        SaveFile(JsonConvert.SerializeObject(energyDataStorage), "EnergyDataStorage.json");
    }

    public void SaveBlock()
    {
        BlockDataStorage[] blockDataStorages = BlockController.instance.GetBlocks();
        SaveFile(JsonConvert.SerializeObject(blockDataStorages), "BlockDataStorage.json");
    }
    
    public void SaveEvo()
    {
        WeaponEvolutionDataStorge weaponEvolutionDataStorge = UpgradeEvolutionController.instance.GetData();
        SaveFile(JsonConvert.SerializeObject(weaponEvolutionDataStorge), "EvoDataStorage.json");
    }

    public void SavePlayer()
    {
        EquipmentDataStorage[] equipmentConfigs = new EquipmentDataStorage[EquipmentController.instance.amoutEquip];
        EquipmentUpgradeDataStorage equipmentUpgradeDataStorage = new EquipmentUpgradeDataStorage(
            EquipmentController.instance.playerInventory.gunLevelUpgrade
            , EquipmentController.instance.playerInventory.boomLevelUpgrade
            , EquipmentController.instance.playerInventory.capLevelUpgrade
            , EquipmentController.instance.playerInventory.clothesLevelUpgrade);

        DesignDataStorage designDataStorage = new DesignDataStorage(
            EquipmentController.instance.playerInventory.amoutGunDesign
            , EquipmentController.instance.playerInventory.amoutCapDesign
            , EquipmentController.instance.playerInventory.amoutBoomDesign
            , EquipmentController.instance.playerInventory.amoutClothesDesign);

        for (int i = 0; i < equipmentConfigs.Length; i++)
        {
            equipmentConfigs[i] = new EquipmentDataStorage((int)EquipmentController.instance.equipments[i].type, (int)EquipmentController.instance.equipments[i].level);
        }
        PlayerDataStorage playerDataStorage = new PlayerDataStorage(
            PlayerController.instance.player.gold
            , EquipmentController.instance.playerInventory.gem
            , EquipmentController.instance.playerInventory.dush
            , EquipmentController.instance.playerInventory.key
            , EquipmentController.instance.playerInventory.cogwheel
            , EquipmentController.instance.playerInventory.gunLevel
            , EquipmentController.instance.playerInventory.boomLevel
            , EquipmentController.instance.playerInventory.clothesLevel
            , EquipmentController.instance.playerInventory.clothesLevel
            , equipmentConfigs, equipmentUpgradeDataStorage, designDataStorage);

        SaveFile(JsonConvert.SerializeObject(playerDataStorage), "PlayerDataStorage.json");
    }
}


[System.Serializable]
public class BlockConfig
{
    public int startPrice;
    public int[] hpUpgrades;
    public int[] priceUpgrades;
    public WEAPON[][] weaponTypes;
}

[System.Serializable]
public class BossConfig
{
    public string[] title;
    public string[] titleInPanel;
    public int[] damage;
    public int[] key;
    public int[] targetProgress;
}

[System.Serializable]
public class ChanceConfig
{
    public float[][] chances;
    public int[] amoutUpgradeLevel;
}

[System.Serializable]
public class RewardConfig
{
    public RewardLevelConfig[] rewardLevelConfigs;
}

[System.Serializable]
public class DailyConfig
{
    public Daily.DailyType dailyType;
    public int amountTarget;
    public string content;
    public int gemReward;

    public DailyConfig(Daily.DailyType dailyType, int amountTarget, string content, int gemReward)
    {
        this.dailyType = dailyType;
        this.amountTarget = amountTarget;
        this.content = content;
        this.gemReward = gemReward;
    }
}

[System.Serializable]
public class RewardLevelConfig
{
    public EquipRewardConfig[] equips;
    public int[] desgins;
    public int dush;
    public int gem;
    public int key;
    public int cogwheel;
}

[System.Serializable]
public class EquipRewardConfig
{
    public int type;
    public int level;
}

[System.Serializable]
public class EquipmentConfig
{
    public int[] dushUpgrades;
    public int[] desginUpgrades;
    public int dushStep;
    public int designStep;
    public int dushUpgradeStep;
    public int designUpgradeStep;

    public GunConfig gunConfig;
    public BoomConfig boomConfig;
    public CapConfig capConfig;
    public ClothesConfig clothesConfig;
}

[System.Serializable]
public class GunConfig
{
    public int startDamage;
    public float coefBylevel;
    public float coefByRarity;
}

[System.Serializable]
public class BoomConfig
{
    public int startDamage;
    public float coefBylevel;
    public float coefByRarity;
}

[System.Serializable]
public class CapConfig
{
    public int startHp;
    public float coefBylevel;
    public float coefByRarity;
}

[System.Serializable]
public class ClothesConfig
{
    public int startHp;
    public float coefBylevel;
    public float coefByRarity;
}


[System.Serializable]
public class WeaponConfig
{
    public WEAPON weaponType;
    public int price;
    public float distanceType;
    public WeaponLevelConfig[] weaponLevelConfigs;

    public WeaponConfig(WEAPON weaponType, int price, float distanceType, WeaponLevelConfig[] weaponLevelConfigs)
    {
        this.weaponType = weaponType;
        this.price = price;
        this.distanceType = distanceType;
        this.weaponLevelConfigs = weaponLevelConfigs;
    }
}

[System.Serializable]
public class WeaponLevelConfig
{
    public PriceConfig priceConfig;
    public AttackConfig attackConfig;

    public WeaponLevelConfig(PriceConfig priceConfig, AttackConfig attackConfig)
    {
        this.priceConfig = priceConfig;
        this.attackConfig = attackConfig;
    }
}

[System.Serializable]
public class PriceConfig
{
    public int startPrice;
    public int evolutionPrice;
    public float upgradeCoef;

    public PriceConfig(int startPrice, int evolutionPrice, float upgradeCoef)
    {
        this.startPrice = startPrice;
        this.evolutionPrice = evolutionPrice;
        this.upgradeCoef = upgradeCoef;
    }
}

[System.Serializable]
public class AttackConfig
{
    public int damageBooster;
    public int startAttackPower;
    public float critChance;
    public float critDamageScale;
    public float upgradeCoef;
    public float attackTime;
    public float reloadTime;

    public AttackConfig(int damageBooster, int startAttackPower, float critChance, float critDamageScale, float upgradeCoef, float attackTime, float reloadTime)
    {
        this.damageBooster = damageBooster;
        this.startAttackPower = startAttackPower;
        this.critChance = critChance;
        this.critDamageScale = critDamageScale;
        this.upgradeCoef = upgradeCoef;
        this.attackTime = attackTime;
        this.reloadTime = reloadTime;
    }
}

[System.Serializable]
public class EnergyConfig
{
    public int startPrice;
    public float startSeconds;
    public float upgradePriceCoef;
    public float upgradeSecondsCoef;
}

[System.Serializable]
public class ProgressConfig
{
    public int[] towerRewards;
}


[System.Serializable]
public class BossDataStorage
{
    public int level;
    public int gold;
    public int levelEnergy;
    public DateTime lastRewardTime;
    public int goldRewardHighest;
    public int progress;
    public BlockDataStorage[] blockDataStorage;
    public WeaponEvolutionDataStorge weaponEvolutionDataStorge;

    public BossDataStorage(int level, int gold, int levelEnergy, DateTime lastRewardTime
        , int goldRewardHighest, int progress, BlockDataStorage[] blockDataStorage
        , WeaponEvolutionDataStorge weaponEvolutionDataStorge)
    {
        this.level = level;
        this.gold = gold;
        this.levelEnergy = levelEnergy;
        this.lastRewardTime = lastRewardTime;
        this.goldRewardHighest = goldRewardHighest;
        this.progress = progress;
        this.blockDataStorage = blockDataStorage;
        this.weaponEvolutionDataStorge = weaponEvolutionDataStorge;
    }
}

[System.Serializable]
public class DataStorage
{
    public int level;
    public bool isSoundActive;
    public bool isMusicActive;
    public DateTime lastRewardTime;
    public int goldRewardHighest;
    public int[] progresses;

    public DataStorage(int level, bool isSoundActive, bool isMusicActive, DateTime lastRewardTime, int goldRewardHighest, int[] progresses)
    {
        this.level = level;
        this.isSoundActive = isSoundActive;
        this.isMusicActive = isMusicActive;
        this.lastRewardTime = lastRewardTime;
        this.goldRewardHighest = goldRewardHighest;
        this.progresses = progresses;
    }
}

[System.Serializable]
public class ChanceDataStorage
{
    public int level;
    public int amout;

    public ChanceDataStorage(int level, int amout)
    {
        this.level = level;
        this.amout = amout;
    }
}

[System.Serializable]
public class DailyDataStorage
{
    public List<DailyConfig> dailyOfDate;
    public DateTime lastUpdateDate;
    public int indexDaily;
    public int amount;

    public DailyDataStorage(List<DailyConfig> dailyOfDate, DateTime lastUpdateDate, int indexDaily, int amount)
    {
        this.dailyOfDate = dailyOfDate;
        this.lastUpdateDate = lastUpdateDate;
        this.indexDaily = indexDaily;
        this.amount = amount;
    }
}

[System.Serializable]
public class TutorialDataStorage
{
    public bool isFirstTimePlay;

    public bool isFirstTimeClickButtonBuyBlock;
    public bool isFirstTimeClickButtonBuyWeapon;
    public bool isFirstTimeClickButtonUpgradeEnergy;
    public bool isFirstTimeClickUpgradeWeaponEvo;

    public bool isFirstTimeClickBoosterBoom;
    public bool isFirstTimeClickBoosterSaw;
    public bool isFirstTimeClickBoosterFlame;
    public bool isFirstTimeClickBoosterMachineGun;

    public bool isFirstTimeDestroyTower;
    public bool isSecondTimeDestroyTower;
    public bool isFirstTimeDragBlock;

    public bool isUnlockInventory;
    public bool isUnlockShop;
    public bool isUnlockWeapon;
    public bool isUnlockBoss;

    public bool isFirstTimeClickButtonInventory;
    public bool isFirstTimeClickButtonEquipInventory;
    public bool isFirstTimeClickButtonSellInventory;
    public bool isFirstTimeClickButtonUpgradeInventory;
    public bool isFirstTimeClickButtonUpgradeLevelInventory;
    public bool isFirstTimeClickButtonShop;
    public bool isFirstTimeClickButtonRoll;
    public bool isFirstTimeClickButtonWeapon;
    public bool isFirstTimeClickButtonBoss;
    public bool isFirstTimeClickButtonPlayBoss;

    public TutorialDataStorage()
    {
    }

    public TutorialDataStorage(bool isFirstTimePlay, bool isFirstTimeClickButtonBuyBlock, bool isFirstTimeClickButtonBuyWeapon
        , bool isFirstTimeClickButtonUpgradeEnergy, bool isFirstTimeClickUpgradeWeaponEvo, bool isFirstTimeClickBoosterBoom
        , bool isFirstTimeClickBoosterSaw, bool isFirstTimeClickBoosterFlame, bool isFirstTimeClickBoosterMachineGun
        , bool isFirstTimeDestroyTower, bool isSecondTimeDestroyTower, bool isFirstTimeDragBlock, bool isUnlockInventory
        , bool isUnlockShop, bool isUnlockWeapon, bool isUnlockBoss, bool isFirstTimeClickButtonInventory, bool isFirstTimeClickButtonEquipInventory
        , bool isFirstTimeClickButtonSellInventory, bool isFirstTimeClickButtonUpgradeInventory, bool isFirstTimeClickButtonUpgradeLevelInventory
        , bool isFirstTimeClickButtonShop, bool isFirstTimeClickButtonRoll, bool isFirstTimeClickButtonWeapon, bool isFirstTimeClickButtonBoss, bool isFirstTimeClickButtonPlayBoss)
    {
        this.isFirstTimePlay = isFirstTimePlay;
        this.isFirstTimeClickButtonBuyBlock = isFirstTimeClickButtonBuyBlock;
        this.isFirstTimeClickButtonBuyWeapon = isFirstTimeClickButtonBuyWeapon;
        this.isFirstTimeClickButtonUpgradeEnergy = isFirstTimeClickButtonUpgradeEnergy;
        this.isFirstTimeClickUpgradeWeaponEvo = isFirstTimeClickUpgradeWeaponEvo;
        this.isFirstTimeClickBoosterBoom = isFirstTimeClickBoosterBoom;
        this.isFirstTimeClickBoosterSaw = isFirstTimeClickBoosterSaw;
        this.isFirstTimeClickBoosterFlame = isFirstTimeClickBoosterFlame;
        this.isFirstTimeClickBoosterMachineGun = isFirstTimeClickBoosterMachineGun;
        this.isFirstTimeDestroyTower = isFirstTimeDestroyTower;
        this.isSecondTimeDestroyTower = isSecondTimeDestroyTower;
        this.isFirstTimeDragBlock = isFirstTimeDragBlock;
        this.isUnlockInventory = isUnlockInventory;
        this.isUnlockShop = isUnlockShop;
        this.isUnlockWeapon = isUnlockWeapon;
        this.isUnlockBoss = isUnlockBoss;
        this.isFirstTimeClickButtonInventory = isFirstTimeClickButtonInventory;
        this.isFirstTimeClickButtonEquipInventory = isFirstTimeClickButtonEquipInventory;
        this.isFirstTimeClickButtonSellInventory = isFirstTimeClickButtonSellInventory;
        this.isFirstTimeClickButtonUpgradeInventory = isFirstTimeClickButtonUpgradeInventory;
        this.isFirstTimeClickButtonUpgradeLevelInventory = isFirstTimeClickButtonUpgradeLevelInventory;
        this.isFirstTimeClickButtonShop = isFirstTimeClickButtonShop;
        this.isFirstTimeClickButtonRoll = isFirstTimeClickButtonRoll;
        this.isFirstTimeClickButtonWeapon = isFirstTimeClickButtonWeapon;
        this.isFirstTimeClickButtonBoss = isFirstTimeClickButtonBoss;
        this.isFirstTimeClickButtonPlayBoss = isFirstTimeClickButtonPlayBoss;
    }
}

[System.Serializable]
public class EquipmentDataStorage
{
    public int type;
    public int level;

    public EquipmentDataStorage(int type, int level)
    {
        this.type = type;
        this.level = level;
    }
}

[System.Serializable]
public class EquipmentUpgradeDataStorage
{
    public int gunLevelUpgrade;
    public int boomLevelUpgrade;
    public int capLevelUpgrade;
    public int clothesLevelUpgrade;

    public EquipmentUpgradeDataStorage(int gunLevelUpgrade, int boomLevelUpgrade, int capLevelUpgrade, int clothesLevelUpgrade)
    {
        this.gunLevelUpgrade = gunLevelUpgrade;
        this.boomLevelUpgrade = boomLevelUpgrade;
        this.capLevelUpgrade = capLevelUpgrade;
        this.clothesLevelUpgrade = clothesLevelUpgrade;
    }
}

[System.Serializable]
public class DesignDataStorage
{
    public int gunAmout;
    public int capAmout;
    public int boomAmout;
    public int clothesAmout;

    public DesignDataStorage(int gunAmout, int capAmout, int boomAmout, int clothesAmout)
    {
        this.boomAmout = boomAmout;
        this.capAmout = capAmout;
        this.gunAmout = gunAmout;
        this.clothesAmout = clothesAmout;
    }
}

[System.Serializable]
public class PlayerDataStorage
{
    public int gold;
    public int gem;
    public int dush;
    public int key;
    public int cogwheel;

    public int gunLevel;
    public int boomLevel;
    public int capLevel;
    public int clothesLevel;

    public DesignDataStorage designDataStorage;
    public EquipmentUpgradeDataStorage equipmentUpgradeDataStorages;
    public EquipmentDataStorage[] equipmentDataStorages;

    public PlayerDataStorage(int gold, int gem, int dush, int key, int cogwheel, int gunLevel, int boomLevel, int capLevel, int clothesLevel, EquipmentDataStorage[] equipmentDataStorages, EquipmentUpgradeDataStorage equipmentUpgradeDataStorages, DesignDataStorage designDataStorage)
    {
        this.gold = gold;
        this.gem = gem;
        this.dush = dush;
        this.key = key;
        this.cogwheel = cogwheel;
        this.gunLevel = gunLevel;
        this.boomLevel = boomLevel;
        this.capLevel = capLevel;
        this.clothesLevel = clothesLevel;
        this.equipmentDataStorages = equipmentDataStorages;
        this.equipmentUpgradeDataStorages = equipmentUpgradeDataStorages;
        this.designDataStorage = designDataStorage;
    }
}

[System.Serializable]
public class EnergyDataStorage
{
    public int level;

    public EnergyDataStorage(int level)
    {
        this.level = level;
    }
}

[System.Serializable]
public class BlockDataStorage
{
    public int level;
    public int sellingPrice;
    public WeaponDataStorage weaponDataStorage;

    public BlockDataStorage(int level, int sellingPrice, WeaponDataStorage weaponDataStorage)
    {
        this.level = level;
        this.sellingPrice = sellingPrice;
        this.weaponDataStorage = weaponDataStorage;
    }
}

[System.Serializable]
public class WeaponDataStorage
{
    public WEAPON weaponType;
    public int weaponLevel;
    public int weaponLevelUpgrade;

    public WeaponDataStorage(WEAPON weaponType, int weaponLevel, int weaponLevelUpgrade)
    {
        this.weaponType = weaponType;
        this.weaponLevel = weaponLevel;
        this.weaponLevelUpgrade = weaponLevelUpgrade;
    }
}

[System.Serializable]
public class WeaponEvolutionDataStorge
{
    public SAWEVO[] sawEvos;
    public FLAMEEVO[] flameEvos;
    public MACHINEGUNEVO[] machineGunEvos;
    public SHOCKEREVO[] shockerEvos;

    public WeaponEvolutionDataStorge(SAWEVO[] sawEvos, FLAMEEVO[] flameEvos, MACHINEGUNEVO[] machineGunEvos, SHOCKEREVO[] shockerEvos)
    {
        this.sawEvos = sawEvos;
        this.flameEvos = flameEvos;
        this.machineGunEvos = machineGunEvos;
        this.shockerEvos = shockerEvos;
    }
}

