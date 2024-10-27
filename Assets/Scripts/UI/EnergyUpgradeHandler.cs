using TMPro;
using UnityEngine;

public class EnergyUpgradeHandler : ButtonUpgradee
{
    public int level;
    public TextMeshProUGUI textTime;

    public void LoadData()
    {
        if (DataManager.instance.dataStorage.energyDataStorage != null) level = DataManager.instance.dataStorage.energyDataStorage.level;
        UpgradeHandle();
    }

    public override void CheckButtonState()
    {
        if (PlayerController.instance.player.gold < (DataManager.instance.dataStorage.energyDataStorage != null ? DataManager.instance.GetPriceUpgradeEnergyConfig(level) : DataManager.instance.energyConfig.startPrice))
        {
            UIHandler.instance.EnergyButtonChangeState(UIHandler.Type.NOT_ENOUGH_MONEY, frame, framePrice, arrow);
        }
        else
        {
            UIHandler.instance.EnergyButtonChangeState(UIHandler.Type.ENOUGH_MONEY, frame, framePrice, arrow);
        }
    }

    public override void Upgrade()
    {
        int price = DataManager.instance.GetPriceUpgradeEnergyConfig(level);
        PlayerController.instance.player.gold -= price;
        UIHandler.instance.GoldUpdatee();
        UIHandler.instance.tutorial.TutorialButtonUpgradeEnergy(true);
        BlockController.instance.CheckButtonStateAll();
        level++;
        UpgradeHandle();
    }

    public override void UpgradeHandle()
    {
        textPriceUpgrade.text = DataManager.instance.GetPriceUpgradeEnergyConfig(level).ToString();
        textTime.text = DataManager.instance.GetSecondsUpgradeEnergyConfig(level).ToString("#0.##", System.Globalization.CultureInfo.GetCultureInfo("vi-VN")) + "/s";
    }
}
