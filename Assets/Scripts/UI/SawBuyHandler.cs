public class SawBuyHandler : WeaponBuyButton
{
    public override void Buy()
    {
        blockUpgradeHandler.BuyWeapon(GameController.WEAPON.SAW, 0);
        scBlock.PlusGold(DataManager.instance.GetPriceWeaponConfig(GameController.WEAPON.SAW));
    }

    public override void LoadData()
    {
        textPrice.text = DataManager.instance.GetPriceWeaponConfig(GameController.WEAPON.SAW).ToString();
    }

    public override void CheckButtonState()
    {
        if (PlayerHandler.instance.playerInfo.gold < DataManager.instance.GetPriceWeaponConfig(GameController.WEAPON.MACHINE_GUN)) UIHandler.instance.ChangeSpriteWeaponBuyer(UIHandler.Type.NOT_ENOUGH_MONEY, frameButton, frameGold);
        else UIHandler.instance.ChangeSpriteWeaponBuyer(UIHandler.Type.ENOUGH_MONEY, frameButton, frameGold);
    }
}
