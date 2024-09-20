using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnergyUpgradeHandler : ButtonUpgradee
{
    public TextMeshProUGUI textTime;
    public Image lightling;

    public void Start()
    {
        LoadData();
    }

    public void LoadData()
    {
        UpgradeHandle();
    }

    public override void CheckButtonState()
    {
        if (DataManager.instance.playerData.gold < DataManager.instance.energyData.priceUpgrades[DataManager.instance.playerData.indexEnergy])
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
        DataManager.instance.playerData.gold -= DataManager.instance.energyData.priceUpgrades[DataManager.instance.playerData.indexEnergy];
        DataManager.instance.playerData.indexEnergy++;
        UpgradeHandle();
    }

    public override void UpgradeHandle()
    {
        textPriceUpgrade.text = DataManager.instance.energyData.priceUpgrades[DataManager.instance.playerData.indexEnergy].ToString();
        textTime.text = DataManager.instance.energyData.times[DataManager.instance.playerData.indexEnergy].ToString("#0.##", System.Globalization.CultureInfo.GetCultureInfo("vi-VN")) + "/s";
    }
}
