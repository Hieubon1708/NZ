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
        if (DataManager.instance.dataStorage.playerDataStorage != null)
        {
            gem = DataManager.instance.dataStorage.playerDataStorage.gem;
            dush = DataManager.instance.dataStorage.playerDataStorage.dush;
            cogwheel = DataManager.instance.dataStorage.playerDataStorage.cogwheel;

            gunLevel = DataManager.instance.dataStorage.playerDataStorage.gunLevel;
            boomLevel = DataManager.instance.dataStorage.playerDataStorage.boomLevel;
            capLevel = DataManager.instance.dataStorage.playerDataStorage.capLevel;
            clothesLevel = DataManager.instance.dataStorage.playerDataStorage.clothesLevel;

            gunLevelUpgrade = DataManager.instance.dataStorage.playerDataStorage.equipmentUpgradeDataStorages.gunLevelUpgrade;
            boomLevelUpgrade = DataManager.instance.dataStorage.playerDataStorage.equipmentUpgradeDataStorages.boomLevelUpgrade;
            capLevelUpgrade = DataManager.instance.dataStorage.playerDataStorage.equipmentUpgradeDataStorages.capLevelUpgrade;
            clothesLevelUpgrade = DataManager.instance.dataStorage.playerDataStorage.equipmentUpgradeDataStorages.clothesLevelUpgrade;

            amoutGunDesign = DataManager.instance.dataStorage.playerDataStorage.designDataStorage.gunAmout;
            amoutBoomDesign = DataManager.instance.dataStorage.playerDataStorage.designDataStorage.boomAmout;
            amoutCapDesign = DataManager.instance.dataStorage.playerDataStorage.designDataStorage.capAmout;
            amoutClothesDesign = DataManager.instance.dataStorage.playerDataStorage.designDataStorage.clothesAmout;
        }

        textDush.text = UIHandler.instance.ConvertNumberAbbreviation(dush);
        textCogwheel.text = UIHandler.instance.ConvertNumberAbbreviation(cogwheel);

        ChangeCap();
        ChangeClothes();
        ChangeGun();
    }

    public void ChangeCap()
    {
        cap.sprite = EquipmentController.instance.caps[capLevel];
        Vector2 pivot = PlayerController.instance.player.playerSkiner.caps[capLevel].pivot;
        Vector2 size = PlayerController.instance.player.playerSkiner.caps[capLevel].rect.size;
        rectCap.pivot = new Vector2(pivot.x / size.x, pivot.y / size.y);
    }

    public void RewardDesign(EquipmentController.EQUIPMENTTYPE type)
    {
        if (type == EquipmentController.EQUIPMENTTYPE.SHOTGUN) amoutGunDesign++;
        else if (type == EquipmentController.EQUIPMENTTYPE.GRENADE) amoutBoomDesign++;
        else if (type == EquipmentController.EQUIPMENTTYPE.CAP) amoutCapDesign++;
        else amoutClothesDesign++;
        dush += 10;
    }

    public void ChangeClothes()
    {
        clothes.sprite = EquipmentController.instance.clothess[clothesLevel];
    }

    public void ChangeGun()
    {
        gun.sprite = EquipmentController.instance.guns[gunLevel];
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
