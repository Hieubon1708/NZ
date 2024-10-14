using UnityEngine.EventSystems;

public class SawBuyButton : ButtonState
{
    public SawBuyHandler sawBuyHandler;

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        sawBuyHandler.Buy();
        base.OnPointerUp(eventData);
    }
}
