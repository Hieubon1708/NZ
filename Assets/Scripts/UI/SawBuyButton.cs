using UnityEngine.EventSystems;

public class SawBuyButton : ButtonState
{
    public SawBuyHandler sawBuyHandler;

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (PlayerController.instance.player.gold < DataManager.instance.GetPriceWeaponConfig(GameController.WEAPON.SAW)) return;
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (PlayerController.instance.player.gold < DataManager.instance.GetPriceWeaponConfig(GameController.WEAPON.SAW)) return;
        if (eventData.pointerCurrentRaycast.gameObject == currentObjectSelected) sawBuyHandler.Buy();
        base.OnPointerUp(eventData);
    }
}
