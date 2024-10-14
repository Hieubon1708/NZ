using UnityEngine.EventSystems;

public class EnergyUpgradeButton : ButtonState
{
    public EnergyUpgradeHandler energyUpgradeHandler;

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        energyUpgradeHandler.Upgrade();
        base.OnPointerUp(eventData);
    }
}
