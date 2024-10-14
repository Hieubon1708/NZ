using UnityEngine.EventSystems;

public class BlockUpgradeButton : ButtonState
{
    public BlockUpgradeHandler blockUpgradeHandler;

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        blockUpgradeHandler.Upgrade();
        base.OnPointerUp(eventData);
    }
}
