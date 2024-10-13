using UnityEngine.EventSystems;

public class EnergyUpgradeButton : ButtonState
{
    public EnergyUpgradeHandler energyUpgradeHandler;

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (PlayerController.instance.player.gold < (DataManager.instance.dataStorage.energyDataStorage != null ? DataManager.instance.GetPriceUpgradeEnergyConfig(DataManager.instance.dataStorage.energyDataStorage.level) : DataManager.instance.energyConfig.startPrice)) return;
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (PlayerController.instance.player.gold < (DataManager.instance.dataStorage.energyDataStorage != null ? DataManager.instance.GetPriceUpgradeEnergyConfig(DataManager.instance.dataStorage.energyDataStorage.level) : DataManager.instance.energyConfig.startPrice)) return;
        if (eventData.pointerCurrentRaycast.gameObject == currentObjectSelected) energyUpgradeHandler.Upgrade();
        base.OnPointerUp(eventData);
    }
}
