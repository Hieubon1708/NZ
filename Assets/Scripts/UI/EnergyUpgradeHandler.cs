using TMPro;
using UnityEngine;

public class EnergyUpgradeHandler : ButtonUpgradee
{
    public int level;
    public TextMeshProUGUI textTime;

    public void LoadData()
    {
        level = DataManager.instance.energyDataStorage.level;
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
        AudioController.instance.PlaySound(AudioController.instance.buttonClick);
        int price = DataManager.instance.GetPriceUpgradeEnergyConfig(level);
        PlayerController.instance.player.gold -= price;
        UIHandler.instance.GoldUpdatee();
        level++;
        UIHandler.instance.tutorial.TutorialButtonUpgradeEnergy(true);
        UpgradeHandle();
        BlockController.instance.CheckButtonStateAll();
        DataManager.instance.SaveEnergy();
    }

    public override void UpgradeHandle()
    {
        textPriceUpgrade.text = DataManager.instance.GetPriceUpgradeEnergyConfig(level).ToString();
        textTime.text = DataManager.instance.GetSecondsUpgradeEnergyConfig(level).ToString("#0.##", System.Globalization.CultureInfo.GetCultureInfo("vi-VN")) + "/s";
    }
}
