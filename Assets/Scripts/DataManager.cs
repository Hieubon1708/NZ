using Newtonsoft.Json;
using System.IO;
using UnityEngine;
using static GameController;
using static UpgradeEvolutionController;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    public Sprite[] blockSpritesLv1;
    public Sprite[] blockSpritesLv2;
    public Sprite[] blockSpritesLv3;
    public Sprite[] blockSpritesLv4;
    public Sprite[] blockSpritesLv5;
    public Sprite[] blockSpritesLv6;

    public Sprite[] carSpritesLv1;
    public Sprite[] carSpritesLv2;
    public Sprite[] carSpritesLv3;
    public Sprite[] carSpritesLv4;
    public Sprite[] carSpritesLv5;
    public Sprite[] carSpritesLv6;

    public Sprite[] blockSprites;

    public DataStorage dataStorage;
    public BlockConfig blockConfig;
    public EnergyConfig energyConfig;
    public ChanceConfig chanceConfig;
    public RewardConfig[] rewardConfigs;
    public EquipmentConfig equipmentConfig;
    public WeaponConfig[] weaponConfigs;

    public void Awake()
    {
        instance = this;
        // GenerateWeaponConfigs();
        DataReader();
    }

    public void SetBlockSprites(int level)
    {
        if (level == 0) blockSprites = blockSpritesLv1;
        if (level == 1) blockSprites = blockSpritesLv2;
        if (level == 2) blockSprites = blockSpritesLv3;
        if (level == 3) blockSprites = blockSpritesLv4;
        if (level == 4) blockSprites = blockSpritesLv5;
        if (level == 5) blockSprites = blockSpritesLv6;
    }

    public Sprite[] GetCarSprites(int level)
    {
        if (level == 0) return carSpritesLv1;
        if (level == 1) return carSpritesLv2;
        if (level == 2) return carSpritesLv3;
        if (level == 3) return carSpritesLv4;
        if (level == 4) return carSpritesLv5;
        if (level == 5) return carSpritesLv6;

        return null;
    }

    void DataReader()
    {
        TextAsset blockConfigJs = Resources.Load<TextAsset>("Datas/BlockConfig");
        TextAsset energyConfigJs = Resources.Load<TextAsset>("Datas/EnergyConfig");
        TextAsset weaponConfigJs = Resources.Load<TextAsset>("Datas/WeaponConfig");
        TextAsset playerConfigJs = Resources.Load<TextAsset>("Datas/PlayerConfig");
        TextAsset chanceConfigJs = Resources.Load<TextAsset>("Datas/ChanceConfig");
        TextAsset equipmentConfigJs = Resources.Load<TextAsset>("Datas/EquipmentConfig");
        TextAsset rewardConfigJs = Resources.Load<TextAsset>("Datas/RewardConfig");

        blockConfig = JsonConvert.DeserializeObject<BlockConfig>(blockConfigJs.text);
        energyConfig = JsonConvert.DeserializeObject<EnergyConfig>(energyConfigJs.text);
        chanceConfig = JsonConvert.DeserializeObject<ChanceConfig>(chanceConfigJs.text);
        equipmentConfig = JsonConvert.DeserializeObject<EquipmentConfig>(equipmentConfigJs.text);
        weaponConfigs = JsonConvert.DeserializeObject<WeaponConfig[]>(weaponConfigJs.text);
        rewardConfigs = JsonConvert.DeserializeObject<RewardConfig[]>(rewardConfigJs.text);

        string dataStorageJs = Path.Combine(Application.persistentDataPath, "DataStorage.json");
        if (File.Exists(dataStorageJs))
        {
            string DataStorageContent = File.ReadAllText(dataStorageJs);
            dataStorage = JsonConvert.DeserializeObject<DataStorage>(DataStorageContent);
        }
        else
        {
            Debug.LogWarning("File not found: " + dataStorageJs);
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
}

[System.Serializable]
public class BlockConfig
{
    public int startPrice;
    public int[] hpUpgrades;
    public int[] priceUpgrades;
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

public class RewardLevelConfig
{
    public EquipRewardConfig[] equips;
    public int[] desgins;
    public int dush;
    public int gem;
    public int key;
    public int cogwheel;
}

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

public class GunConfig
{
    public int startDamage;
    public float coefBylevel;
    public float coefByRarity;
}

public class BoomConfig
{
    public int startDamage;
    public float coefBylevel;
    public float coefByRarity;
}

public class CapConfig
{
    public int startHp;
    public float coefBylevel;
    public float coefByRarity;
}

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
public class DataStorage
{
    public int level;
    public bool isSoundActive;
    public bool isMusicActive;
    public int[] progresses;

    public TutorialDataStorage tutorialDataStorage;
    public playerDataStorage playerDataStorage;
    public BlockDataStorage[] blockDataStorage;
    public EnergyDataStorage energyDataStorage;
    public ChanceDataStorage chanceDataStorage;
    public WeaponEvolutionDataStorge weaponEvolutionDataStorge;

    public DataStorage() { }

    public DataStorage(int level, bool isSoundActive, bool isMusicActive, playerDataStorage playerDataStorage, BlockDataStorage[] blockDataStorage, EnergyDataStorage energyDataStorage, WeaponEvolutionDataStorge weaponEvolutionDataStorge, ChanceDataStorage chanceDataStorage, int[] progresses, TutorialDataStorage tutorialDataStorage)
    {
        this.level = level;
        this.isSoundActive = isSoundActive;
        this.isMusicActive = isMusicActive;
        this.playerDataStorage = playerDataStorage;
        this.blockDataStorage = blockDataStorage;
        this.energyDataStorage = energyDataStorage;
        this.weaponEvolutionDataStorge = weaponEvolutionDataStorge;
        this.chanceDataStorage = chanceDataStorage;
        this.progresses = progresses;
        this.tutorialDataStorage = tutorialDataStorage;
    }
}

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

public class TutorialDataStorage
{
    public int isFirstTimePlay;
    public int isFirstTimeClickButtonBuyBlock;
    public int isFirstTimeClickButtonBuyWeapon;
    public int isFirstTimeClickBooster;
}

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

public class playerDataStorage
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

    public playerDataStorage(int gold, int gem, int dush, int key, int cogwheel, int gunLevel, int boomLevel, int capLevel, int clothesLevel, EquipmentDataStorage[] equipmentDataStorages, EquipmentUpgradeDataStorage equipmentUpgradeDataStorages, DesignDataStorage designDataStorage)
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

public class EnergyDataStorage
{
    public int level;

    public EnergyDataStorage(int level)
    {
        this.level = level;
    }
}

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

