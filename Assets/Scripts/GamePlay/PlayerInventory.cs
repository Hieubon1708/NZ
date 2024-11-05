using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    public TextMeshProUGUI textDush;
    public TextMeshProUGUI textCogwheel;

    public RectTransform rectCap;

    public Image cap;
    public Image clothes;
    public Image gun;

    public int gem;
    public int dush;
    public int cogwheel;
    public int key;

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

    public void LoadData()
    {
        if (DataManager.instance.playerDataStorage != null)
        {
            gem = DataManager.instance.playerDataStorage.gem;
            dush = DataManager.instance.playerDataStorage.dush;
            key = DataManager.instance.playerDataStorage.key;
            cogwheel = DataManager.instance.playerDataStorage.cogwheel;

            gunLevel = DataManager.instance.playerDataStorage.gunLevel;
            boomLevel = DataManager.instance.playerDataStorage.boomLevel;
            capLevel = DataManager.instance.playerDataStorage.capLevel;
            clothesLevel = DataManager.instance.playerDataStorage.clothesLevel;

            gunLevelUpgrade = DataManager.instance.playerDataStorage.equipmentUpgradeDataStorages.gunLevelUpgrade;
            boomLevelUpgrade = DataManager.instance.playerDataStorage.equipmentUpgradeDataStorages.boomLevelUpgrade;
            capLevelUpgrade = DataManager.instance.playerDataStorage.equipmentUpgradeDataStorages.capLevelUpgrade;
            clothesLevelUpgrade = DataManager.instance.playerDataStorage.equipmentUpgradeDataStorages.clothesLevelUpgrade;

            amoutGunDesign = DataManager.instance.playerDataStorage.designDataStorage.gunAmout;
            amoutBoomDesign = DataManager.instance.playerDataStorage.designDataStorage.boomAmout;
            amoutCapDesign = DataManager.instance.playerDataStorage.designDataStorage.capAmout;
            amoutClothesDesign = DataManager.instance.playerDataStorage.designDataStorage.clothesAmout;
        }

        UpdateTextDush();
        UpdateTextCogwheel();
        EquipmentController.instance.ChangeCap();
        EquipmentController.instance.ChangeClothes();
        EquipmentController.instance.ChangeGun();
    }

    public void UpdateTextDush()
    {
        textDush.text = UIHandler.instance.ConvertNumberAbbreviation(dush);
    }
    
    public void UpdateTextCogwheel()
    {
        textCogwheel.text = UIHandler.instance.ConvertNumberAbbreviation(cogwheel);
    }

    public void ConvertGoldToDush()
    {
        dush += PlayerController.instance.player.gold / 1000;
        UIHandler.instance.goldTemp = PlayerController.instance.player.gold;
        PlayerController.instance.player.gold = 0;
    }

    public void SubtractGem(int gem)
    {
        this.gem -= gem;
    }

    public void PlusGem(int gem)
    {
        this.gem += gem;
    }

    public void SubtractCogwheel(int cogwheel)
    {
        this.cogwheel -= cogwheel;
    }

    public void PlusCogwheel(int cogwheel)
    {
        this.cogwheel += cogwheel;
    }
}
