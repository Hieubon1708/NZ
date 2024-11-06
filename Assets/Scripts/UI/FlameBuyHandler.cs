public class FlameBuyHandler : WeaponBuyButton
{
    public override void Buy()
    {
        AudioController.instance.PlaySoundButton(AudioController.instance.buttonClick);
        blockUpgradeHandler.BuyWeapon(GameController.WEAPON.FLAME, 0);
        scBlock.PlusGold(DataManager.instance.GetPriceWeaponConfig(GameController.WEAPON.FLAME));
        BlockController.instance.CheckButtonStateAll();
    }
    
    public override void LoadData()
    {
        textPrice.text = DataManager.instance.GetPriceWeaponConfig(GameController.WEAPON.FLAME).ToString();
    }

    public override void CheckButtonState()
    {
        if (PlayerController.instance.player.gold < DataManager.instance.GetPriceWeaponConfig(GameController.WEAPON.FLAME))
        {
            UIHandler.instance.WeaponButtonChangeState(UIHandler.Type.NOT_ENOUGH_MONEY, frameButton, framePrice);
        }
        else
        {
            UIHandler.instance.WeaponButtonChangeState(UIHandler.Type.ENOUGH_MONEY, frameButton, framePrice);
        }
    }
}
