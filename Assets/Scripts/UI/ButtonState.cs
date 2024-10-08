using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonState : ButtonScale
{
    public GameObject currentObjectSelected;

    public override void OnPointerDown(PointerEventData eventData)
    {
        currentObjectSelected = eventData.pointerCurrentRaycast.gameObject;
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        BlockController.instance.CheckButtonStateAll();
    }
}
