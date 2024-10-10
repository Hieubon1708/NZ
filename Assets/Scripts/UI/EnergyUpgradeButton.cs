using UnityEngine.EventSystems;

public class EnergyUpgradeButton : ButtonState
{
    public EnergyUpgradeHandler energyUpgradeHandler;

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (PlayerHandler.instance.playerInfo.gold < (DataManager.instance.dataStorage.energyDataStorage != null ? DataManager.instance.GetPriceUpgradeEnergyConfig(DataManager.instance.dataStorage.energyDataStorage.level) : DataManager.instance.energyConfig.startPrice)) return;
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (PlayerHandler.instance.playerInfo.gold < (DataManager.instance.dataStorage.energyDataStorage != null ? DataManager.instance.GetPriceUpgradeEnergyConfig(DataManager.instance.dataStorage.energyDataStorage.level) : DataManager.instance.energyConfig.startPrice)) return;
        base.OnPointerUp(eventData);
        if (eventData.pointerCurrentRaycast.gameObject == currentObjectSelected) energyUpgradeHandler.Upgrade();
    }
}
