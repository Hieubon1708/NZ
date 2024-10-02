using UnityEngine.EventSystems;

public class FlameBuyButton : ButtonClicker
{
    public FrameBuyHandler flameBuyHandler;

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (DataManager.instance.playerData.gold < DataManager.instance.GetPriceWeaponConfig(GameController.WEAPON.FLAME)) return;
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (DataManager.instance.playerData.gold < DataManager.instance.GetPriceWeaponConfig(GameController.WEAPON.SAW)) return;
        base.OnPointerUp(eventData);
        if (eventData.pointerCurrentRaycast.gameObject == currentObjectSelected) flameBuyHandler.Buy();
    }
}
