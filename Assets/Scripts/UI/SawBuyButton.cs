using UnityEngine;
using UnityEngine.EventSystems;

public class SawBuyButton : ButtonClicker
{
    public SawBuyHandler sawBuyHandler;

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (DataManager.instance.playerData.gold < DataManager.instance.sawData.price) return;
        base.OnPointerUp(eventData);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (DataManager.instance.playerData.gold < DataManager.instance.sawData.price) return;
        base.OnPointerDown(eventData);
        sawBuyHandler.Buy();
    }
}
