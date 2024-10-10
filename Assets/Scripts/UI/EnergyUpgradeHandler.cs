using TMPro;

public class EnergyUpgradeHandler : ButtonUpgradee
{
    public int level;
    public TextMeshProUGUI textTime;

    public void LoadData()
    {
        level = DataManager.instance.dataStorage.energyDataStorage.level;
        UpgradeHandle();
    }

    public override void CheckButtonState()
    {
        if (PlayerHandler.instance.playerInfo.gold < DataManager.instance.GetPriceUpgradeEnergyConfig(level))
        {
            UIHandler.instance.EnergyButtonChangeState(UIHandler.Type.NOT_ENOUGH_MONEY, frame, framePrice);
        }
        else
        {
            UIHandler.instance.EnergyButtonChangeState(UIHandler.Type.ENOUGH_MONEY, frame, framePrice);
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
