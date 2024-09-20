using UnityEngine.EventSystems;

public class EnergyUpgradeButton : ButtonClicker
{
    public EnergyUpgradeHandler energyUpgradeHandler;

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (DataManager.instance.playerData.gold < DataManager.instance.energyData.priceUpgrades[DataManager.instance.playerData.indexEnergy]) return;
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (DataManager.instance.playerData.gold < DataManager.instance.energyData.priceUpgrades[DataManager.instance.playerData.indexEnergy]) return;
        base.OnPointerUp(eventData);
        energyUpgradeHandler.Upgrade();
    }
}
