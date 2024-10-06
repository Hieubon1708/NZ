using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnergyUpgradeHandler : ButtonUpgradee
{
    public int level;
    public TextMeshProUGUI textTime;
    public Image lightling;

    public void LoadData()
    {
        level = DataManager.instance.dataStorage.energyDataStorage != null ? DataManager.instance.dataStorage.energyDataStorage.level : 0;
        UpgradeHandle();
    }

    public override void CheckButtonState()
    {
        if (PlayerHandler.instance.playerInfo.gold < DataManager.instance.GetPriceUpgradeEnergyConfig(level))
        {
            UIHandler.instance.ChangeSpriteWeaponLastUpgradee(UIHandler.Type.NOT_ENOUGH_MONEY, frame);
            lightling.color = new Color(1, 1, 1, 0.5f);
        }
        else
        {
            UIHandler.instance.ChangeSpriteWeaponLastUpgradee(UIHandler.Type.ENOUGH_MONEY, frame);
            lightling.color = Vector4.one;
        }
    }

    public override void Upgrade()
    {
        PlayerHandler.instance.playerInfo.gold -= DataManager.instance.GetPriceUpgradeEnergyConfig(level);
        level++;
        UpgradeHandle();
    }

    public override void UpgradeHandle()
    {
        textPriceUpgrade.text = DataManager.instance.GetPriceUpgradeEnergyConfig(level).ToString();
        textTime.text = DataManager.instance.GetPriceUpgradeEnergyConfig(level).ToString("#0.##", System.Globalization.CultureInfo.GetCultureInfo("vi-VN")) + "/s";
    }
}
