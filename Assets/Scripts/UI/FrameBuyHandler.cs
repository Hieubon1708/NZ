using UnityEngine;

public class FrameBuyHandler : WeaponBuyButton
{
    public override void Buy()
    {
        blockUpgradeHandler.BuyWeapon(GameController.WEAPON.FLAME, 0);
        scBlock.PlusGold(DataManager.instance.flameData.price);
    }

    public override void LoadData()
    {
        textPrice.text = DataManager.instance.flameData.price.ToString();
    }

    public override void CheckButtonState()
    {
        if (DataManager.instance.playerData.gold < DataManager.instance.flameData.price) UIHandler.instance.ChangeSpriteWeaponBuyer(UIHandler.Type.NOT_ENOUGH_MONEY, frameButton, frameGold);
        else UIHandler.instance.ChangeSpriteWeaponBuyer(UIHandler.Type.ENOUGH_MONEY, frameButton, frameGold);
    }
}
