using UnityEngine.EventSystems;

public class BlockBuyButton : ButtonClicker
{
    public BlockBuyHandler blockBuyHandler;

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (DataManager.instance.playerData.gold < DataManager.instance.blockData.price) return;
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (DataManager.instance.playerData.gold < DataManager.instance.blockData.price) return;
        base.OnPointerUp(eventData);
        blockBuyHandler.Buy();
    }
}
