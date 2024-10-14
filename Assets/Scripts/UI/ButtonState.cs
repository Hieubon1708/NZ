using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonState : ButtonScale
{
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        BlockController.instance.CheckButtonStateAll();
    }
}
