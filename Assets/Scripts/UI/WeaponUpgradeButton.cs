using UnityEngine.EventSystems;

public class WeaponUpgradeButton : ButtonState
{
    public WeaponUpgradeHandler weaponUpgradeHandler;

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (weaponUpgradeHandler.textMax.gameObject.activeSelf || PlayerController.instance.player.gold < DataManager.instance.GetUpgradePriceWeaponConfig(weaponUpgradeHandler.level, weaponUpgradeHandler.levelUpgrade, weaponUpgradeHandler.weaponConfig)) return;
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (weaponUpgradeHandler.textMax.gameObject.activeSelf || PlayerController.instance.player.gold < DataManager.instance.GetUpgradePriceWeaponConfig(weaponUpgradeHandler.level, weaponUpgradeHandler.levelUpgrade, weaponUpgradeHandler.weaponConfig)) return;
        if (eventData.pointerCurrentRaycast.gameObject == currentObjectSelected) weaponUpgradeHandler.Upgrade();
        base.OnPointerUp(eventData);
    }
}
