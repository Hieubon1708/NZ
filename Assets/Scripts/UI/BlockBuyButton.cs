using UnityEngine;
using UnityEngine.EventSystems;

public class BlockBuyButton : ButtonState
{
    public BlockBuyHandler blockBuyHandler;

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (PlayerHandler.instance.playerInfo.gold < DataManager.instance.blockConfig.startPrice) return;
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (PlayerHandler.instance.playerInfo.gold < DataManager.instance.blockConfig.startPrice) return;
        base.OnPointerUp(eventData);
        if(eventData.pointerCurrentRaycast.gameObject == currentObjectSelected) blockBuyHandler.Buy();
    }
}
