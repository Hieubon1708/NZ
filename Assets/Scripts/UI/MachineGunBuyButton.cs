using UnityEngine.EventSystems;

public class MachineGunBuyButton : ButtonState
{
    public MachineGunBuyHandler machineGunBuyHandler;

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (PlayerHandler.instance.playerInfo.gold < DataManager.instance.GetPriceWeaponConfig(GameController.WEAPON.MACHINE_GUN)) return;
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (PlayerHandler.instance.playerInfo.gold < DataManager.instance.GetPriceWeaponConfig(GameController.WEAPON.MACHINE_GUN)) return;
        base.OnPointerUp(eventData);
        if (eventData.pointerCurrentRaycast.gameObject == currentObjectSelected) machineGunBuyHandler.Buy();
    }
}
