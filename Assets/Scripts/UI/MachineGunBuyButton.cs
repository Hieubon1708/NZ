using UnityEngine.EventSystems;

public class MachineGunBuyButton : ButtonClicker
{
    public MachineGunBuyHandler machineGunBuyHandler;

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (DataManager.instance.playerData.gold < DataManager.instance.GetPriceWeaponConfig(GameController.WEAPON.MACHINE_GUN)) return;
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (DataManager.instance.playerData.gold < DataManager.instance.GetPriceWeaponConfig(GameController.WEAPON.MACHINE_GUN)) return;
        base.OnPointerUp(eventData);
        if (eventData.pointerCurrentRaycast.gameObject == currentObjectSelected) machineGunBuyHandler.Buy();
    }
}
