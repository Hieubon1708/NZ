using UnityEngine.EventSystems;

public class FlameBuyButton : ButtonState
{
    public FrameBuyHandler flameBuyHandler;

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (PlayerController.instance.player.gold < DataManager.instance.GetPriceWeaponConfig(GameController.WEAPON.FLAME)) return;
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (PlayerController.instance.player.gold < DataManager.instance.GetPriceWeaponConfig(GameController.WEAPON.SAW)) return;
        base.OnPointerUp(eventData);
        if (eventData.pointerCurrentRaycast.gameObject == currentObjectSelected) flameBuyHandler.Buy();
    }
}
