using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory instance;

    public int gem;
    public int dush;

    public int capLevel;
    public int clothesLevel;
    public int gunLevel;
    public int boomLevel;

    public int capLevelUpgrade;
    public int clothesLevelUpgrade;
    public int gunLevelUpgrade;
    public int boomLevelUpgrade;

    public int amoutGunDesign;
    public int amoutBoomDesign;
    public int amoutCapDesign;
    public int amoutClothesDesign;

    private void Awake()
    {
        instance = this;
    }

    public void LoadData()
    {
        if(DataManager.instance.dataStorage.pLayerDataStorage != null)
        {
            gunLevel = DataManager.instance.dataStorage.pLayerDataStorage.gunLevel;
            boomLevel = DataManager.instance.dataStorage.pLayerDataStorage.boomLevel;
            capLevel = DataManager.instance.dataStorage.pLayerDataStorage.capLevel;
            clothesLevel = DataManager.instance.dataStorage.pLayerDataStorage.clothesLevel;

            gunLevelUpgrade = DataManager.instance.dataStorage.pLayerDataStorage.equipmentUpgradeDataStorages.gunLevelUpgrade;
            boomLevelUpgrade = DataManager.instance.dataStorage.pLayerDataStorage.equipmentUpgradeDataStorages.boomLevelUpgrade;
            capLevelUpgrade = DataManager.instance.dataStorage.pLayerDataStorage.equipmentUpgradeDataStorages.capLevelUpgrade;
            clothesLevelUpgrade = DataManager.instance.dataStorage.pLayerDataStorage.equipmentUpgradeDataStorages.clothesLevelUpgrade;

            amoutGunDesign = DataManager.instance.dataStorage.pLayerDataStorage.designDataStorage.gunAmout;
            amoutBoomDesign = DataManager.instance.dataStorage.pLayerDataStorage.designDataStorage.boomAmout;
            amoutCapDesign = DataManager.instance.dataStorage.pLayerDataStorage.designDataStorage.capAmout;
            amoutClothesDesign = DataManager.instance.dataStorage.pLayerDataStorage.designDataStorage.clothesAmout;
        }
    }
}
