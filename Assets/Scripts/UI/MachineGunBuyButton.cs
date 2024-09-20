using UnityEngine;
using UnityEngine.EventSystems;

public class MachineGunBuyButton : ButtonClicker
{
    public MachineGunBuyHandler machineGunBuyHandler;

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (DataManager.instance.playerData.gold < DataManager.instance.machineGunData.price) return;
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (DataManager.instance.playerData.gold < DataManager.instance.machineGunData.price) return;
        base.OnPointerUp(eventData);
        machineGunBuyHandler.Buy();
    }
}
