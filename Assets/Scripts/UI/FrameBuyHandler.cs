public class FrameBuyHandler : WeaponBuyButton
{
    public override void Buy()
    {
        blockUpgradeHandler.BuyWeapon(GameController.WEAPON.FLAME, 0);
        scBlock.PlusGold(DataManager.instance.GetPriceWeaponConfig(GameController.WEAPON.FLAME));
    }

    public override void LoadData()
    {
        textPrice.text = DataManager.instance.GetPriceWeaponConfig(GameController.WEAPON.FLAME).ToString();
    }

    public override void CheckButtonState()
    {
        if (PlayerHandler.instance.playerInfo.gold < DataManager.instance.GetPriceWeaponConfig(GameController.WEAPON.FLAME)) UIHandler.instance.WeaponButtonChangeState(UIHandler.Type.NOT_ENOUGH_MONEY, frameButton, frameGold);
        else UIHandler.instance.WeaponButtonChangeState(UIHandler.Type.ENOUGH_MONEY, frameButton, frameGold);
    }
}
