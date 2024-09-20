using UnityEngine;
using UnityEngine.EventSystems;

public class FlameBuyButton : ButtonClicker
{
    public FrameBuyHandler flameBuyHandler;

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (DataManager.instance.playerData.gold < DataManager.instance.flameData.price) return;
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (DataManager.instance.playerData.gold < DataManager.instance.flameData.price) return;
        base.OnPointerUp(eventData);
        flameBuyHandler.Buy();
    }
}
