using UnityEngine.EventSystems;

public class WeaponUpgradeButton : ButtonClicker
{
    public WeaponUpgradeHandler weaponUpgradeHandler;

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (weaponUpgradeHandler.textMax.gameObject.activeSelf || DataManager.instance.playerData.gold < DataManager.instance.GetUpgradePriceWeaponConfig(weaponUpgradeHandler.level, weaponUpgradeHandler.levelUpgrade, weaponUpgradeHandler.weaponConfig)) return;
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (weaponUpgradeHandler.textMax.gameObject.activeSelf || DataManager.instance.playerData.gold < DataManager.instance.GetUpgradePriceWeaponConfig(weaponUpgradeHandler.level, weaponUpgradeHandler.levelUpgrade, weaponUpgradeHandler.weaponConfig)) return;
        base.OnPointerUp(eventData);
        if (eventData.pointerCurrentRaycast.gameObject == currentObjectSelected) weaponUpgradeHandler.Upgrade();
    }
}
