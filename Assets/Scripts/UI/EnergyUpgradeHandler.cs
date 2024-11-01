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
        if (PlayerController.instance.player.gold < DataManager.instance.GetPriceUpgradeEnergyConfig(level))
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
        level++;
        UpgradeHandle();
        BlockController.instance.CheckButtonStateAll();
    }

    public override void UpgradeHandle()
    {
        textPriceUpgrade.text = DataManager.instance.GetPriceUpgradeEnergyConfig(level).ToString();
        textTime.text = DataManager.instance.GetSecondsUpgradeEnergyConfig(level).ToString("#0.##", System.Globalization.CultureInfo.GetCultureInfo("vi-VN")) + "/s";
    }
}
