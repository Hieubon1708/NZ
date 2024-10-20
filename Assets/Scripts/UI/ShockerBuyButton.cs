using UnityEngine.EventSystems;

public class ShockerBuyButton : ButtonState
{
    public ShockerBuyHandler shockerBuyHandler;

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        shockerBuyHandler.Buy();
        base.OnPointerUp(eventData);
    }
}
