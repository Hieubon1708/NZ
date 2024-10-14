using UnityEngine.EventSystems;

public class FlameBuyButton : ButtonState
{
    public FrameBuyHandler flameBuyHandler;

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        flameBuyHandler.Buy();
    }
}
