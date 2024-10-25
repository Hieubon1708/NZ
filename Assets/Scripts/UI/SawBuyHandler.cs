public class SawBuyHandler : WeaponBuyButton
{
    public TutorialOject tutorialOject;

    public override void Buy()
    {
        blockUpgradeHandler.BuyWeapon(GameController.WEAPON.SAW, 0);
        scBlock.PlusGold(DataManager.instance.GetPriceWeaponConfig(GameController.WEAPON.SAW));
        UIHandler.instance.tutorial.TutorialButtonBuyWeapon(true);
        BlockController.instance.CheckButtonStateAll();
    }

    public override void LoadData()
    {
        textPrice.text = DataManager.instance.GetPriceWeaponConfig(GameController.WEAPON.SAW).ToString();
    }

    public override void CheckButtonState()
    {
        if (PlayerController.instance.player.gold < DataManager.instance.GetPriceWeaponConfig(GameController.WEAPON.SAW))
        {
            UIHandler.instance.WeaponButtonChangeState(UIHandler.Type.NOT_ENOUGH_MONEY, frameButton, framePrice);
        }
        else
        {
            UIHandler.instance.WeaponButtonChangeState(UIHandler.Type.ENOUGH_MONEY, frameButton, framePrice);
        }
    }
}
