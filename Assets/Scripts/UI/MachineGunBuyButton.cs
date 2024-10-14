using UnityEngine.EventSystems;

public class MachineGunBuyButton : ButtonState
{
    public MachineGunBuyHandler machineGunBuyHandler;

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        machineGunBuyHandler.Buy();
    }
}
