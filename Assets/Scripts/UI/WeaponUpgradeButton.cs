using UnityEngine.EventSystems;

public class WeaponUpgradeButton : ButtonState
{
    public WeaponUpgradeHandler weaponUpgradeHandler;

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        weaponUpgradeHandler.Upgrade();
        base.OnPointerUp(eventData);
    }
}
