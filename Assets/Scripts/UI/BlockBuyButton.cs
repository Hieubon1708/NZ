using UnityEngine;
using UnityEngine.EventSystems;

public class BlockBuyButton : ButtonState
{
    public BlockBuyHandler blockBuyHandler;

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        blockBuyHandler.Buy();
        base.OnPointerUp(eventData);
    }
}
