using UnityEngine.EventSystems;

public class EnergyUpgradeButton : ButtonClicker
{
    public EnergyUpgradeHandler energyUpgradeHandler;

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (PlayerHandler.instance.playerInfo.gold < DataManager.instance.GetPriceUpgradeEnergyConfig(DataManager.instance.dataStorage.energyDataStorage.level)) return;
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (PlayerHandler.instance.playerInfo.gold < DataManager.instance.GetPriceUpgradeEnergyConfig(DataManager.instance.dataStorage.energyDataStorage.level)) return;
        base.OnPointerUp(eventData);
        if (eventData.pointerCurrentRaycast.gameObject == currentObjectSelected) energyUpgradeHandler.Upgrade();
    }
}
