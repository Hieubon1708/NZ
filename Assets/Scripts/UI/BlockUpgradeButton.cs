using UnityEngine.EventSystems;

public class BlockUpgradeButton : ButtonState
{
    public BlockUpgradeHandler blockUpgradeHandler;

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (blockUpgradeHandler.textMax.gameObject.activeSelf || PlayerHandler.instance.playerInfo.gold < DataManager.instance.blockConfig.priceUpgrades[blockUpgradeHandler.blockInfo.level]) return;
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (blockUpgradeHandler.textMax.gameObject.activeSelf || PlayerHandler.instance.playerInfo.gold < DataManager.instance.blockConfig.priceUpgrades[blockUpgradeHandler.blockInfo.level]) return;
        if(eventData.pointerCurrentRaycast.gameObject == currentObjectSelected) blockUpgradeHandler.Upgrade();
        base.OnPointerUp(eventData);
    }
}
