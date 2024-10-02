using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static GameController;

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

    public BlockData blockData;
    public EnergyData energyData;
    public PlayerData playerData;
    public IngameData[] ingameDatas;
    public List<WeaponConfig> weaponConfigs;

    public void Awake()
    {
        instance = this;
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
        TextAsset jsBlock = Resources.Load<TextAsset>("Datas/BlockData");
        TextAsset jsEnergy = Resources.Load<TextAsset>("Datas/EnergyData");

        blockData = JsonConvert.DeserializeObject<BlockData>(jsBlock.text);
        energyData = JsonConvert.DeserializeObject<EnergyData>(jsEnergy.text);

        string jsIngame = Path.Combine(Application.persistentDataPath, "IngameData.json");
        if (File.Exists(jsIngame))
        {
            string jsonContent = File.ReadAllText(jsIngame);
            ingameDatas = JsonConvert.DeserializeObject<IngameData[]>(jsonContent);
        }
        else Debug.LogWarning("File not found: " + jsIngame);

        string jsPlayer = Path.Combine(Application.persistentDataPath, "PlayerData.json");
        if (File.Exists(jsPlayer))
        {
            string jsonContent = File.ReadAllText(jsPlayer);
            playerData = JsonConvert.DeserializeObject<PlayerData>(jsonContent);
        }
        else
        {
            Debug.LogWarning("File not found: " + jsPlayer);
            playerData = new PlayerData(4500, 0, 0, 0);
        }
        string weaponConfigJs = Path.Combine(Application.dataPath, "Resources/Datas/WeaponConfig.json");
        if (!File.Exists(weaponConfigJs))
        {
            GenerateWeaponConfigs();
        }
        string weaponConfigContent = File.ReadAllText(weaponConfigJs);
        weaponConfigs = JsonConvert.DeserializeObject<List<WeaponConfig>>(weaponConfigContent);
    }

    public WeaponConfig FindWeaponConfigByType(WEAPON type)
    {
        for (int i = 0; i < weaponConfigs.Count; i++)
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
        return (int)(priceConfig.startPrice * Mathf.Pow(priceConfig.upgradeCoef, levelUpgrade));
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
        return weaponConfig.weaponLevelConfigs.Count;
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

    void GenerateWeaponConfigs()
    {
        List<WeaponConfig> weaponConfigs = new List<WeaponConfig>();

        List<WeaponLevelConfig> sawLevelConfigs = new List<WeaponLevelConfig>();

        sawLevelConfigs.Add(SetWeaponLevelConfig(200, 250, 1.03f, 600, 7, 0.2f, 1.5f, 1.11f, 2, 1));
        sawLevelConfigs.Add(SetWeaponLevelConfig(310, 400, 1.03f, 1286, 15, 0.2f, 1.5f, 1.11f, 2, 1));
        sawLevelConfigs.Add(SetWeaponLevelConfig(481, 600, 1.03f, 2914, 34, 0.2f, 1.5f, 1.11f, 2, 1));
        sawLevelConfigs.Add(SetWeaponLevelConfig(746, 900, 1.03f, 6429, 75, 0.2f, 1.5f, 1.11f, 2, 1));
        sawLevelConfigs.Add(SetWeaponLevelConfig(1156, 1400, 1.03f, 14057, 164, 0.2f, 1.5f, 1.11f, 2, 1));
        sawLevelConfigs.Add(SetWeaponLevelConfig(1792, 2250, 1.03f, 30943, 361, 0.2f, 1.5f, 1.11f, 2, 1));
        sawLevelConfigs.Add(SetWeaponLevelConfig(2778, 0, 1.03f, 68057, 794, 0.2f, 1.5f, 1.11f, 2, 1));

        weaponConfigs.Add(new WeaponConfig(WEAPON.SAW, 90, 2, sawLevelConfigs));
        
        List<WeaponLevelConfig> machineGunLevelConfigs = new List<WeaponLevelConfig>();

        machineGunLevelConfigs.Add(SetWeaponLevelConfig(500, 600, 1.03f, 300, 40, 0.2f, 1.5f, 1.11f, 2, 1));
        machineGunLevelConfigs.Add(SetWeaponLevelConfig(700, 800, 1.03f, 705, 94, 0.2f, 1.5f, 1.11f, 2, 1));
        machineGunLevelConfigs.Add(SetWeaponLevelConfig(980, 1200, 1.03f, 1658, 221, 0.2f, 1.5f, 1.11f, 2, 1));
        machineGunLevelConfigs.Add(SetWeaponLevelConfig(1372, 1800, 1.03f, 3892, 519, 0.2f, 1.5f, 1.11f, 2, 1));
        machineGunLevelConfigs.Add(SetWeaponLevelConfig(1921, 2250, 1.03f, 9150, 1220, 0.2f, 1.5f, 1.11f, 2, 1));
        machineGunLevelConfigs.Add(SetWeaponLevelConfig(2689, 3250, 1.03f, 21502, 2867, 0.2f, 1.5f, 1.11f, 2, 1));
        machineGunLevelConfigs.Add(SetWeaponLevelConfig(3765, 0, 1.03f, 50528, 6737, 0.2f, 1.5f, 1.11f, 2, 1));

        weaponConfigs.Add(new WeaponConfig(WEAPON.MACHINE_GUN, 750, 2, machineGunLevelConfigs));

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
public class BlockData
{
    public int price;
    public int[] hps;
    public int[] priceUpgrades;
}

[System.Serializable]
public class WeaponConfig
{
    public WEAPON weaponType;
    public int price;
    public float distanceType;
    public List<WeaponLevelConfig> weaponLevelConfigs;

    public WeaponConfig(WEAPON weaponType, int price, float distanceType, List<WeaponLevelConfig> weaponLevelConfigs)
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
public class EnergyData
{
    public float[] times;
    public int[] priceUpgrades;
}

[System.Serializable]
public class PlayerData
{
    public float playerHp;
    public int gameLevel;
    public int gold;
    public int indexEnergy;

    public PlayerData(float playerHp, int gameLevel, int gold, int indexEnergy)
    {
        this.playerHp = playerHp;
        this.gameLevel = gameLevel;
        this.gold = gold;
        this.indexEnergy = indexEnergy;
    }
}

[System.Serializable]
public class IngameData
{
    public int blockLevel;
    public int blockGold;
    public WEAPON weaponType;
    public int weaponLevel;
    public int weaponLevelUpgrade;

    public IngameData(int blockLevel, int blockGold, WEAPON weaponType, int weaponLevel, int weaponLevelUpgrade)
    {
        this.blockLevel = blockLevel;
        this.blockGold = blockGold;
        this.weaponType = weaponType;
        this.weaponLevel = weaponLevel;
        this.weaponLevelUpgrade = weaponLevelUpgrade;
    }
}

