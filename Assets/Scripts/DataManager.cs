using Newtonsoft.Json;
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
    public SawData sawData;
    public FlameData flameData;
    public MachineGunData machineGunData;
    public EnergyData energyData;
    public PlayerData playerData;
    public IngameData[] ingameDatas;

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
        TextAsset jsSaw = Resources.Load<TextAsset>("Datas/SawData");
        TextAsset jsFlame = Resources.Load<TextAsset>("Datas/FlameData");
        TextAsset jsMachineGun = Resources.Load<TextAsset>("Datas/MachineGunData");
        TextAsset jsEnergy= Resources.Load<TextAsset>("Datas/EnergyData");

        blockData = JsonConvert.DeserializeObject<BlockData>(jsBlock.text);
        sawData = JsonConvert.DeserializeObject<SawData>(jsSaw.text);
        flameData = JsonConvert.DeserializeObject<FlameData>(jsFlame.text);
        machineGunData = JsonConvert.DeserializeObject<MachineGunData>(jsMachineGun.text);
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
            playerData = new PlayerData();
        }
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
public class SawData
{
    public int price;
    public int[][] priceUpgrades;
    public int[][] damages;
}

[System.Serializable]
public class FlameData
{
    public int price;
    public int[][] priceUpgrades;
    public int[][] damages;
}

[System.Serializable]
public class MachineGunData
{
    public int price;
    public int[][] priceUpgrades;
    public int[][] damages;
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
    public int gameLevel;
    public int gold;
    public int indexEnergy;
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

