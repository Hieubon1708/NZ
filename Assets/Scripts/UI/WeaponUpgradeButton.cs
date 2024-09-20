using UnityEngine.EventSystems;

public class WeaponUpgradeButton : ButtonClicker
{
    public WeaponUpgradeHandler weaponUpgradeHandler;

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (weaponUpgradeHandler.textMax.gameObject.activeSelf || DataManager.instance.playerData.gold < weaponUpgradeHandler.priceUpgrades[weaponUpgradeHandler.level][weaponUpgradeHandler.levelUpgrade]) return;
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (weaponUpgradeHandler.textMax.gameObject.activeSelf || DataManager.instance.playerData.gold < weaponUpgradeHandler.priceUpgrades[weaponUpgradeHandler.level][weaponUpgradeHandler.levelUpgrade]) return;
        weaponUpgradeHandler.Upgrade();
        base.OnPointerUp(eventData);
    }
}
